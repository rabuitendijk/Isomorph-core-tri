
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// aplha-1
/// 
/// Modification of the streaming Sprite loader.
/// Sefifically made to load isometric tile alias with obj.xml files and mip (.mx) overrides.
/// 
/// Robin Apollo Buitendijk 
/// Early March 2017
/// </summary>
public class AliasXMLLoader {

    static public AliasXMLLoader main;
    int count = 0;
    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public AliasXMLLoader()
    {
        main = this;
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Alias");
        RecersiveDirectoryCrawler(filePath);
        Debug.Log("Alias Loader: " + count + ", sprites loaded");
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

        Texture2D imageTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);  
        imageTexture.filterMode = FilterMode.Point;
        imageTexture.wrapMode = TextureWrapMode.Clamp;
        imageTexture.LoadImage(imageBytes); 
        //imageTexture.Compress(false);

        if (xml.multiSprite)
        {
            processAlias(xml, imageTexture);
        }

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
}
