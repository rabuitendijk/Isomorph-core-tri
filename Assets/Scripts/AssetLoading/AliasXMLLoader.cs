
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// aplha-2
/// 
/// Modification of the streaming Sprite loader.
/// Sefifically made to load isometric tile alias with obj.xml files and mip (.mx) overrides.
/// 
/// Robin Apollo Buitendijk 
/// Early March 2017
/// </summary>
public class AliasXMLLoader {

    static public AliasXMLLoader main;
    int count = 0, objCount = 0;
    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    Dictionary<string, IsoObject> prototypes = new Dictionary<string, IsoObject>();
    string mipPath;

    public AliasXMLLoader()
    {
        main = this;
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Alias");
        mipPath = System.IO.Path.Combine(Application.streamingAssetsPath, "AliasMip");
        RecersiveDirectoryCrawler(filePath);
        loadObjects(filePath+"\\Objects");
        Debug.Log("Alias Loader: " + count + ", sprites loaded. "+objCount+", Objects loaded.");

        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Recersive first load later sprite loader
    /// </summary>
    void RecersiveDirectoryCrawler(string path)
    {

        //Debug.Log("RecersiveDirectoryCrawler: "+path);


        string[] subDirs = Directory.GetDirectories(path);

        foreach (string s in subDirs)
        {
            RecersiveDirectoryCrawler(s);
        }

        string[] subFiles = Directory.GetFiles(path);

        foreach (string f in subFiles)
        {
            if (f.ToLower().EndsWith(".png"))
                LoadSprite(f);
        }
    }


    void LoadSprite(string filePath)
    {
        StreamingSpriteXMLObject xml;

        if (File.Exists(filePath + ".xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(StreamingSpriteXMLObject));
            XmlReader reader = XmlReader.Create(filePath + ".xml");
            xml = (StreamingSpriteXMLObject)serializer.Deserialize(reader);
        }
        else
        {
            Debug.Log("Alias loader missing xml: "+filePath);
            return;
        }


        byte[] imageBytes = File.ReadAllBytes(filePath);

        //Texture2D imageTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);  
        Texture2D imageTexture = new Texture2D(2, 2);
        imageTexture.filterMode = FilterMode.Point;
        imageTexture.wrapMode = TextureWrapMode.Clamp;
        imageTexture.LoadImage(imageBytes);
        //imageTexture.Compress(false);
        overrideMips(imageTexture, filePath.Substring(filePath.LastIndexOf('\\') + 1, filePath.Length - filePath.LastIndexOf('\\') - 5));

        /*
        for (int i = 0; i < imageTexture.mipmapCount; i++)
        {
            Debug.Log("Mip level: "+i+", Expect: "+(2048>>i)*(2048>>i)+", got: "+ imageTexture.GetPixels32(i).Length);
        }
        */
        if (xml.multiSprite)
        {
            processAlias(xml, imageTexture);
        }

    }

    void overrideMips(Texture2D tex, string fileName)
    {
        byte[] imageBytes;
        Texture2D imageTexture;

        if (File.Exists(mipPath + "\\" + fileName + ".m1.png"))
        {
            imageBytes = File.ReadAllBytes(mipPath + "\\" + fileName + ".m1.png");
            imageTexture = new Texture2D(2, 2);
            imageTexture.LoadImage(imageBytes);

            tex.SetPixels32(imageTexture.GetPixels32(), 1);
            
        }
        else
        {
            Debug.Log("Mip not found: "+ mipPath + "\\" + fileName + ".m1.png");
        }

        if (File.Exists(mipPath + "\\" + fileName + ".m2.png")) {
            imageBytes = File.ReadAllBytes(mipPath + "\\" + fileName + ".m2.png");
            imageTexture = new Texture2D(2, 2);
            imageTexture.LoadImage(imageBytes);

            tex.SetPixels32(imageTexture.GetPixels32(), 2);
        }
        else
        {
            Debug.Log("Mip not found: " + mipPath + "\\" + fileName + ".m2.png");
        }
        tex.Apply(false);

        
    }


    void processAlias(StreamingSpriteXMLObject xml, Texture2D imageTexture)
    {
        Rect spriteCoordinates;
        StreamingSpriteXMLCell cell;

        for (int i = 0; i < xml.columns; i++)
        {
            for (int j = 0; j < xml.rows; j++)
            {

                cell = xml.getCell(i, j);
                if (cell != null)
                {
                    if (!cell.disable)
                    {

                        spriteCoordinates = new Rect(i * (xml.cellWidth + 2 * xml.offset) + xml.offset, j * (xml.cellHeight + 2 * xml.offset) + xml.offset, xml.cellWidth, xml.cellHeight); // In pixels!
                        addSprite(imageTexture, spriteCoordinates, cell.name, xml.pivotPoint, xml.pixelsPerUnit);
                        //Debug.Log(cell.name);
                    }
                }
                else
                {
                    Debug.Log("Cell not found. has the Alias builder version changed?");
                }

            }
        }
    }

    /// <summary>
    /// Adds and creates sprite to dictionairy
    /// </summary>
    void addSprite(Texture2D imageTexture, Rect spriteCoordinates, string spriteName, Vector2 pivotPoint, int pixelsPerUnit)
    {
        Sprite s = Sprite.Create(imageTexture, spriteCoordinates, pivotPoint, pixelsPerUnit);

        try
        {
            sprites.Add(spriteName, s);
            count++;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }


    void loadObjects(string path)
    {
        string[] subFiles = Directory.GetFiles(path);

        foreach (string f in subFiles)
        {
            if (f.ToLower().EndsWith(".obj.xml"))
                loadObject(f);
        }
    }

    void loadObject(string filePath)
    {
        XMLObject xml;

        XmlSerializer serializer = new XmlSerializer(typeof(XMLObject));
        XmlReader reader = XmlReader.Create(filePath);
        xml = (XMLObject)serializer.Deserialize(reader);

        try
        {
            prototypes.Add(xml.name, IsoObject.prototype(xml));
            objCount++;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Trys to get Sprite or returns null
    /// </summary>
    /// <param name="name">name of target Sprite</param>
    /// <returns>the Sprite</returns>
    public Sprite getSprite(string name)
    {
        Sprite sp;
        if (sprites.TryGetValue(name, out sp))
        {
            return sp;
        }

        Debug.Log("Sprite: " + name + ", not found");
        return null;
    }

    /// <summary>
    /// Trys to get IsoObject or returns null
    /// </summary>
    public IsoObject getObject(string name)
    {
        IsoObject ob;
        if (prototypes.TryGetValue(name, out ob))
        {
            return ob;
        }

        Debug.Log("Object: " + name + ", not found");
        return null;
    }
}
