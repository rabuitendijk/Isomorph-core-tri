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
    Texture2D unitTexture;
    //string targetDir;
    int offset = 2;

    public SplicingScript()
    {
        main = this;
    }

    public List<ProcessingObject> Splice()
    {
        //targetDir = System.IO.Path.Combine(Application.streamingAssetsPath, "Images\\Spliced");
        string sourceDir = System.IO.Path.Combine(Application.streamingAssetsPath, "UnsplicedSource");

        /*
        if (!Directory.Exists(targetDir))
        {
            try {
                Directory.CreateDirectory(targetDir);
            }catch(Exception e)
            {
                Debug.Log(e);
            }
        }
        */

        //Attempt load unit sprite
        if (!File.Exists(sourceDir+"\\UnitDefinition.png"))
        {
            Debug.Log("SplicingScript: UnitDefinition not found!!!, "+ sourceDir + "\\UnitDefinition.png");
            return new List<ProcessingObject>();
        }

        byte[] imageBytes = File.ReadAllBytes(sourceDir + "\\UnitDefinition.png");

        unitTexture = new Texture2D(2, 2);   // Create some kind of dummy instance of Texture2D
        unitTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file
        Debug.Log("UnitDefinition loaded{width: "+unitTexture.width+", height: "+unitTexture.height+"}");

        //Start actual splicing
        List<ProcessingObject> ret = new List<ProcessingObject>();
        RecersiveDirectoryCrawler(sourceDir, ret);

        Debug.Log("Splicing sprites: " + count + ", sprites spliced");

        return ret;
    }

    /// <summary>
    /// Recersive first load later
    /// </summary>
    void RecersiveDirectoryCrawler(string path, List<ProcessingObject> sprites)
    {

        //Debug.Log("RecersiveDirectoryCrawler: "+path);

        //Recieve
        string[] subDirs = Directory.GetDirectories(path);

        foreach (string s in subDirs)
        {
            RecersiveDirectoryCrawler(s, sprites);
        }

        //Load
        string[] subFiles = Directory.GetFiles(path);

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
    void LoadSprite(string spriteName, string filePath, List<ProcessingObject> sprites)
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

        Texture2D imageTexture = new Texture2D(2, 2);   // Create some kind of dummy instance of Texture2D
        imageTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file

        //Recover name if needed
        if (xml.setName)
            spriteName = xml.name;


        processTexture(imageTexture, xml, spriteName, sprites);
    }


    void processTexture(Texture2D tex, SplicingXMLObject xml, string name, List<ProcessingObject> sprites)
    {
        int expectedWidth = Mathf.RoundToInt(.5f * unitTexture.width * (xml.depth + xml.width));
        int expectedHeight = Mathf.RoundToInt(.25f * unitTexture.width * (xml.depth + xml.width+2*xml.height));

        int originX = Mathf.RoundToInt(.5f * unitTexture.width * (xml.width) -.5f * unitTexture.width);
        int originY = 0;

        if (!(expectedHeight == tex.height) || !(expectedWidth == tex.width))
        {
            Debug.Log("SpriteSlicer: source image dimentions [" + name + "] do not match expected dimentions. source<" + tex.width + ", " + tex.height + "> expected<" + expectedWidth + ", " + expectedHeight + ">.");
            return;
        }

        List<Iso> coords = getOrder(xml.width, xml.depth, xml.height);
        List<ProcessingImage> images = new List<ProcessingImage>();
        ProcessingImage temp;

        int x, y;
        for (int i = 0; i < coords.Count; i++)
        {
            x = originX + Mathf.RoundToInt(.5f * unitTexture.width * (-coords[i].x + coords[i].y));
            y = originY + Mathf.RoundToInt(.25f * unitTexture.width * (coords[i].x + coords[i].y + 2 * coords[i].z));
            temp = new ProcessingImage(unitTexture.width, unitTexture.height, name + "[" + coords[i].x + "_" + coords[i].y + "_" + coords[i].z + "]", coords[i]);

            for (int j = 0; j < (unitTexture.width); j++)
            {
                for (int k = 0; k < (unitTexture.height); k++)
                {

                    if (unitTexture.GetPixel(j, k).a > .5)
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
            images.Add(temp);
            
        }

        ProcessingObject export = new ProcessingObject(name, images, boxCoords(xml.width, xml.depth, xml.height));
        sprites.Add(export);
        count++;
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

    /// <summary>
    /// Needs alteration
    /// </summary>
    void writeSpriteloaderXML(string name, List<Iso> list)
    {
        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";

        xml += "<StreamingSpriteXMLObject multiSprite = \"1\" rows = \""+1+"\" columns = \""+list.Count+ "\" pixelsPerUnit=\""+unitTexture.width+"\" cellHeight=\""+unitTexture.height+"\" cellWidth=\""+unitTexture.width+"\" offset=\""+offset+"\">\n";
        xml += "<cells>\n";
        for (int i = 0; i < list.Count; i++)
        {
            xml += "\t<StreamingSpriteXMLCell x = \""+i+"\" y = \""+0+"\" name = \""+name+"["+list[i].x+"_"+list[i].y+"_"+list[i].z+"]\">\n";
            xml += "\t</StreamingSpriteXMLCell>\n";
        }
        xml += "</cells>\n";
        xml += "</StreamingSpriteXMLObject>";

        //File.WriteAllText(targetDir+"\\"+name+".png.xml", xml);
    }
}

/*
if (!(expectedHeight == tex.height) || !(expectedWidth == tex.width))
{
    Debug.Log("SpriteSlicer: source image dimentions ["+name+"] do not match expected dimentions. source<"+tex.width+", "+tex.height+"> expected<"+expectedWidth+", "+expectedHeight+">.");
    return null;
}

List<Iso> coords = getOrder(xml.width, xml.depth, xml.height);
Texture2D export = new Texture2D((unitTexture.width+2*offset) * coords.Count, unitTexture.height + 2 * offset);
int x, y;
for (int i = 0; i < coords.Count; i++)
{
    x = originX + Mathf.RoundToInt(.5f * unitTexture.width * (-coords[i].x+ coords[i].y));
    y = originY + Mathf.RoundToInt(.25f * unitTexture.width * (coords[i].x + coords[i].y + 2 * coords[i].z));

    for (int j = 0; j < (unitTexture.width + 2 * offset); j++)
    {
        for (int k = 0; k < (unitTexture.height + 2 * offset); k++)
        {
            if (!(j < offset) && !(j >= unitTexture.width + offset) && !(k < offset) && !(k >= unitTexture.height + offset))
            {   //Oudside offset
                if (unitTexture.GetPixel(j-offset, k-offset).a > .5)
                {
                    //Copy pixel
                    export.SetPixel(i * (unitTexture.width + 2 * offset) + j, k, tex.GetPixel(x + j - offset, y + k - offset));
                }
                else
                {
                    //Ignore pixel
                    export.SetPixel(i * (unitTexture.width + 2 * offset) + j, k, new Color(0f, 0f, 0f, 0f));
                }
            }
            else
            {   //In offset
                //Ignore pixel
                export.SetPixel(i * (unitTexture.width + 2 * offset) + j, k, new Color(0f, 0f, 0f, 0f));
            }

        }
    }
}

export.Apply();

byte[] bytes = export.EncodeToPNG();
File.WriteAllBytes(targetDir+"\\"+name+".png", bytes);
count++;

writeSpriteloaderXML(name, coords);
*/
