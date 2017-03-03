using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// aplha-2
/// 
/// Splices cource files into unit blocks using data from an xml file.
/// Needs a reference unit cell sprite.
/// Now returns dummy images to be proceesed into texture alias
/// Will need to return object data as well
/// 
/// Robin Apollo Butiendijk
/// Late February 2017
/// </summary>
public class SplicingScript{

    public static SplicingScript main;
    int count = 0;
    Texture2D unitTexture, mip1Texture, mip2Texture;
    string source, sourceMip;


    public SplicingScript()
    {
        main = this;
    }

    public List<ProcessingObject> Splice()
    {

        if (!loadUnitDefinitions())
            return new List<ProcessingObject>(); 

        //Start actual splicing
        List<ProcessingObject> ret = new List<ProcessingObject>();
        RecersiveDirectoryCrawler(ret);

        Debug.Log("Splicing sprites: " + count + ", sprites spliced");

        return ret;
    }

    bool loadUnitDefinitions()
    {
        source = System.IO.Path.Combine(Application.streamingAssetsPath, "UnsplicedSource");
        sourceMip = System.IO.Path.Combine(Application.streamingAssetsPath, "MipOverride");

        //Attempt load unit sprite
        if (!File.Exists(source + "\\UnitDefinition.png"))
        {
            Debug.Log("SplicingScript: UnitDefinition not found!!!, " + source + "\\UnitDefinition.png");
            return false;
        }

        byte[] imageBytes = File.ReadAllBytes(source + "\\UnitDefinition.png");

        unitTexture = new Texture2D(2, 2);   // Create some kind of dummy instance of Texture2D
        unitTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file
        Debug.Log("UnitDefinition loaded{width: " + unitTexture.width + ", height: " + unitTexture.height + "}");

        //Attempt load mip1
        if (!File.Exists(sourceMip + "\\UnitDefinition.m1.png"))
        {
            Debug.Log("SplicingScript: UnitDefinition not found!!!, " + sourceMip + "\\UnitDefinition.m1.png");
            return false;
        }

        imageBytes = File.ReadAllBytes(sourceMip + "\\UnitDefinition.m1.png");

        mip1Texture = new Texture2D(2, 2);   // Create some kind of dummy instance of Texture2D
        mip1Texture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file
        Debug.Log("UnitDefinition.m1 loaded{width: " + mip1Texture.width + ", height: " + mip1Texture.height + "}");

        //Attempt load mip1
        if (!File.Exists(sourceMip + "\\UnitDefinition.m2.png"))
        {
            Debug.Log("SplicingScript: UnitDefinition not found!!!, " + sourceMip + "\\UnitDefinition.m2.png");
            return false;
        }

        imageBytes = File.ReadAllBytes(sourceMip + "\\UnitDefinition.m2.png");

        mip2Texture = new Texture2D(2, 2);   // Create some kind of dummy instance of Texture2D
        mip2Texture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file
        Debug.Log("UnitDefinition.m2 loaded{width: " + mip2Texture.width + ", height: " + mip2Texture.height + "}");

        return true;
    }

    /// <summary>
    /// Recersive first load later
    /// </summary>
    void RecersiveDirectoryCrawler(List<ProcessingObject> sprites)
    {

        //Debug.Log("RecersiveDirectoryCrawler: "+path);

        //Recieve
        string[] subDirs = Directory.GetDirectories(source);

        foreach (string s in subDirs)
        {
            RecersiveDirectoryCrawler(sprites);
        }

        //Load
        string[] subFiles = Directory.GetFiles(source);

        foreach (string f in subFiles)
        {
            if (f.ToLower().EndsWith(".png"))
            {
                LoadSprite(f.Substring(f.LastIndexOf('\\') + 1, f.Length - f.LastIndexOf('\\') - 5), f, sprites);
                //Debug.Log("RecersiveDirectoryCrawler: " + f);
            }

        }
    }

    /// <summary>
    /// Quills sprite loader, with modifications
    /// </summary>
    void LoadSprite(string fileName, string filePath, List<ProcessingObject> sprites)
    {
        SplicingXMLObject xml;

        if (File.Exists(filePath + ".xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SplicingXMLObject));
            XmlReader reader = XmlReader.Create(filePath + ".xml");
            xml = (SplicingXMLObject)serializer.Deserialize(reader);
        }
        else
        {
            Debug.Log("SpriteSplicer, Sprite without xml: "+filePath);
            return;
        }


        byte[] imageBytes = File.ReadAllBytes(filePath);

        Texture2D imageTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);   // Create some kind of dummy instance of Texture2D
        imageTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file

        //Recover name if needed
        string spriteName = fileName;
        if (xml.setName)
            spriteName = xml.name;


        List<ProcessingImage> images = processTexture(imageTexture, xml, spriteName, unitTexture.width, unitTexture);
        List<ProcessingImage> mip1, mip2;

        //Fetch mips
        string mip = sourceMip + "\\" + fileName + ".m1.png";
        if (!File.Exists(mip))
        {
            mip1 = null;
            Debug.Log("SpriteSplicer, Sprite without mip1: "+mip);
        }
        else
        {
            imageBytes = File.ReadAllBytes(mip);
            imageTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);   // Create some kind of dummy instance of Texture2D
            imageTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file
            mip1 = processTexture(imageTexture, xml, spriteName, mip1Texture.width, mip1Texture);
        }

        mip = sourceMip + "\\" + fileName + ".m2.png";
        if (!File.Exists(mip))
        {
            mip2 = null;
            Debug.Log("SpriteSplicer, Sprite without mip2: " + mip);
        }
        else
        {
            imageBytes = File.ReadAllBytes(mip);
            imageTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);   // Create some kind of dummy instance of Texture2D
            imageTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file
            mip2 = processTexture(imageTexture, xml, spriteName, mip2Texture.width, mip2Texture);
        }


        ProcessingObject export = new ProcessingObject(spriteName, images, boxCoords(xml.width, xml.depth, xml.height), mip1, mip2);
        sprites.Add(export);
        count++;
    }

    /// <summary>
    /// Needs replacing
    /// xml size parameters <Net needed anyway>
    /// UnitTexture parameters
    /// </summary>
    List<ProcessingImage> processTexture(Texture2D tex, SplicingXMLObject xml, string name, int size, Texture2D unit)
    {
        int expectedWidth = Mathf.RoundToInt(.5f * size * (xml.depth + xml.width));
        int expectedHeight = Mathf.RoundToInt(.25f * size * (xml.depth + xml.width+2*xml.height));

        int originX = Mathf.RoundToInt(.5f * size * (xml.width) -.5f * size);
        int originY = 0;

        if (!(expectedHeight == tex.height) || !(expectedWidth == tex.width))
        {
            Debug.Log("SpriteSlicer: source image dimentions [" + name + "] do not match expected dimentions. source<" + tex.width + ", " + tex.height + "> expected<" + expectedWidth + ", " + expectedHeight + ">.");
            return new List<ProcessingImage>();
        }

        List<Iso> coords = getOrder(xml.width, xml.depth, xml.height);
        List<ProcessingImage> images = new List<ProcessingImage>();

        
        for (int i = 0; i < coords.Count; i++)
        {
            images.Add(getImage(tex, coords[i], originX, originY, name, size, unit));
        }

        return images;
    }

    /// <summary>
    /// Loads the desired fragment of the image
    /// </summary>
    ProcessingImage getImage(Texture2D tex, Iso coord, int originX, int originY, string name, int size, Texture2D unit) {
        int x = originX + Mathf.RoundToInt(.5f * size * (-coord.x + coord.y));
        int y = originY + Mathf.RoundToInt(.25f * size * (coord.x + coord.y + 2 * coord.z));
        ProcessingImage temp = new ProcessingImage(size, size, name + "[" + coord.x + "_" + coord.y + "_" + coord.z + "]", coord);

        for (int j = 0; j < (size); j++)
        {
            for (int k = 0; k < (size); k++)
            {

                if (unit.GetPixel(j, k).a > .5)
                {
                    //Copy pixel
                    temp.set(j, k, tex.GetPixel(x + j, y + k));
                }
                else
                {
                    //Ignore pixel
                    temp.set(j, k, new Color(0f, 0f, 0f, 0f));
                }


            }
        }
        return temp;
    }

    /// <summary>
    /// Will need empty cells for object file
    /// </summary>
    List<Iso> getOrder(int width, int depth, int height)
    {

        int totaltiles = width * height + (depth * height - height) + (width * depth - width - (depth - 1));

        List<Iso> ret = new List<Iso>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                ret.Add(new Iso(i, 0, j));
            }
        }

        for (int i = 1; i < depth; i++)
        {
            for (int j = 0; j < height; j++)
            {
                ret.Add(new Iso(0, i, j));
            }
        }

        for (int i = 1; i < width; i++)
        {
            for (int j = 1; j < depth; j++)
            {
                ret.Add(new Iso(i, j, height-1));
            }
        }

        /*
        foreach(Iso i in ret)
        {
            Debug.Log(i.ToString());
        }
        */
        return ret;
    }

    /// <summary>
    /// Returns a width by depth by height box of Iso coordinates.
    /// </summary>
    List<Iso> boxCoords(int width, int depth, int height)
    {
        List<Iso> ret = new List<Iso>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    ret.Add(new Iso(i, j, k));
                    
                }
            }
        }

        return ret;
    }

  
}

