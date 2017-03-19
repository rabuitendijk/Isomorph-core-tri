
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public static class Alias_SpriteLoader{

    public static int count=0;

	public static Dictionary<string, Sprite> load(string folder)
    {
        Dictionary<string, Sprite> ret = new Dictionary<string, Sprite>();

        string[] subFiles = Directory.GetFiles(folder);

        foreach (string f in subFiles)
        {
            if (f.ToLower().EndsWith(".xml"))
                LoadAlias(f.Substring(0, f.Length-4), ret);
        }

        Debug.Log("Alias_SpriteLoader: " + count + ", sprites loaded.");

        return ret;
    }

    static void LoadAlias(string filename, Dictionary<string, Sprite> dict)
    {
        StreamingSpriteXMLObject xml;

        XmlSerializer serializer = new XmlSerializer(typeof(StreamingSpriteXMLObject));
        XmlReader reader = XmlReader.Create(filename + ".xml");
        xml = (StreamingSpriteXMLObject)serializer.Deserialize(reader);


        if (!File.Exists(filename+".png")){
            Debug.Log("Alias loader missing png: " + filename+".png");
            return;
        }


        byte[] imageBytes = File.ReadAllBytes(filename+".png");

        //Texture2D imageTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);  
        Texture2D imageTexture = new Texture2D(2, 2);
        imageTexture.filterMode = FilterMode.Point;
        imageTexture.wrapMode = TextureWrapMode.Clamp;
        imageTexture.LoadImage(imageBytes);
        //imageTexture.Compress(false);


        overrideMips(imageTexture, filename, xml.miplevels);

        /*
        for (int i = 0; i < imageTexture.mipmapCount; i++)
        {
            Debug.Log("Mip level: "+i+", Expect: "+(2048>>i)*(2048>>i)+", got: "+ imageTexture.GetPixels32(i).Length);
        }
        */


        processAlias(xml, imageTexture, dict);
        

    }

    static void overrideMips(Texture2D tex, string filename, int miplevels)
    {
        byte[] imageBytes;
        Texture2D imageTexture;

        for (int i = 1; i < miplevels+1; i++)
        {
            if (File.Exists(filename + ".m"+i+".png"))
            {
                imageBytes = File.ReadAllBytes(filename + ".m" + i + ".png");
                imageTexture = new Texture2D(2, 2);
                imageTexture.LoadImage(imageBytes);

                tex.SetPixels32(imageTexture.GetPixels32(), i);

            }
            else
            {
                Debug.Log("Mip not found: " + filename + ".m" + i + ".png");
            }
        }

        tex.Apply(false);
    }


    static void processAlias(StreamingSpriteXMLObject xml, Texture2D imageTexture, Dictionary<string, Sprite> dict)
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
                        addSprite(imageTexture, spriteCoordinates, cell.name, xml.pivotPoint, xml.pixelsPerUnit, dict);
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
    static void addSprite(Texture2D imageTexture, Rect spriteCoordinates, string spriteName, Vector2 pivotPoint, int pixelsPerUnit, Dictionary<string, Sprite> dict)
    {
        Sprite s = Sprite.Create(imageTexture, spriteCoordinates, pivotPoint, pixelsPerUnit);

        try
        {
            dict.Add(spriteName, s);
            count++;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
