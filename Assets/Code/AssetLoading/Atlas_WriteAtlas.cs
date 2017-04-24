
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

/// <summary>
/// Writes image data to the atlas
/// </summary>
public static class Atlas_WriteAtlas  {

    static int AtlasSize = 2048;
    static int offset = 16;
    static int miplevels = 2;

    static int unitSize = 256;


    static int size = unitSize + offset* 2;
    static int fitPerRow = Mathf.RoundToInt((AtlasSize - AtlasSize % size) / size);
    static int fitPerAtlas = fitPerRow* fitPerRow;


    /// <summary>
    /// Writes all atlasses
    /// </summary>
    public static void write(string folder, List<SplicingSource> objects, int res, int mipl, int offs)
    {
        offset = offs;
        miplevels = mipl;
        unitSize = res;

        size = unitSize + offset * 2;
        fitPerRow = Mathf.RoundToInt((AtlasSize - AtlasSize % size) / size);
        fitPerAtlas = fitPerRow * fitPerRow;

        makeDirectories(folder);

        for (int i = 0; i < miplevels+1; i++)
        {
            build(i, folder, objects);
        }
    }


    /// <summary>
    /// Forces creation of target directories
    /// </summary>
    static void makeDirectories(string folder)
    {
       
        if (!Directory.Exists(folder))
        {
            try
            {
                Directory.CreateDirectory(folder);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        
    }

    static int count = 0, number = 0;
    static Texture2D currenctAtlas;

    /// <summary>
    /// Build the atlasses
    /// </summary>
    static void build(int miplevel, string folder, List<SplicingSource> objects)
    {

        //Debug.Log("Splcing list contains " + images.Count + " objects.");

        count = 0; number = 0;
        currenctAtlas = new Texture2D((AtlasSize >> miplevel), (AtlasSize >> miplevel), TextureFormat.ARGB32, false);
        List<ProcessingImage> inAtlas = new List<ProcessingImage>();

        foreach (SplicingSource ss in objects)
        {
            for (int i = 0; i < ss.width; i++)
            {
                for (int j = 0; j < ss.length; j++)
                {
                    for (int k = 0; k < ss.height; k++)
                    {
                        if (ss.mips[miplevel][i, j, k] != null)
                        {
                            //Debug.Log("mip = "+miplevel+", ["+i+", "+j+", "+k+"], "+ ss.mips[miplevel][i, j, k].name+", ["+ ss.mips[miplevel][i, j, k] .width+", "+ ss.mips[miplevel][i, j, k] .height+ "]");
                            process(miplevel, folder, ss.mips[miplevel][i, j, k], inAtlas);
                        }

                    }
                }

            }

        }

        writeXML(inAtlas, folder);
        exportAtlas(currenctAtlas, folder, miplevel);
        
    }

    /// <summary>
    /// Proceeses a single image
    /// </summary>
    static void process(int miplevel, string folder, ProcessingImage image, List<ProcessingImage> inAtlas)
    {
        if ( !(count<fitPerAtlas))
        {
            writeXML(inAtlas, folder);
            exportAtlas(currenctAtlas, folder, miplevel);

            currenctAtlas = new Texture2D((AtlasSize >> miplevel), (AtlasSize >> miplevel), TextureFormat.ARGB32, false);

            
            count = 0;
        }

        inAtlas.Add(image);

        writeImage(image, currenctAtlas, count, (size >> miplevel), (offset >> miplevel), (unitSize >> miplevel));
        count++;
        


    }

    /// <summary>
    /// Writes a single image to the atlas
    /// </summary>
    static void writeImage(ProcessingImage i, Texture2D tex, int count, int size, int offset, int unitSize)
    {
        int cx = count % fitPerRow;
        int cy = Mathf.RoundToInt((count - cx) / fitPerRow);

        int ox = cx * size;
        int oy = cy * size;

        //Debug.Log("origin<"+ox+", "+oy+">, count: "+count+", size: "+size+", offset: "+offset);


        for (int j = 0; j < size; j++)
        {
            for (int k = 0; k < size; k++)
            {
                if (!(j < offset) && !(j >= unitSize + offset) && !(k < offset) && !(k >= unitSize + offset))
                {
                    tex.SetPixel(ox + j, oy + k, i.get(j - offset, k - offset));
                }
                else
                {
                    tex.SetPixel(ox + j, oy + k, new Color(0f, 0f, 0f, 0f));
                }
            }
        }
    }

    /// <summary>
    /// Saves the atlas
    /// </summary>
    static void exportAtlas(Texture2D Atlas, string folder, int miplevel)
    {

        Atlas.Apply();

        byte[] bytes = Atlas.EncodeToPNG();
        if (miplevel == 0)
            File.WriteAllBytes(folder + "/" + number + ".png", bytes);
        else
            File.WriteAllBytes(folder + "/" + number + ".m"+miplevel+".png", bytes);
        number++;
    }

    /// <summary>
    /// Writes accompeniing xml
    /// </summary>
    static void writeXML(List<ProcessingImage> images, string folder)
    {
        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";

        xml += "<StreamingSpriteXMLObject miplevels=\""+miplevels+"\" multiSprite = \"1\" rows = \"" + fitPerRow + "\" columns = \"" + fitPerRow + "\" pixelsPerUnit=\"" + unitSize + "\" cellHeight=\"" + unitSize + "\" cellWidth=\"" + unitSize + "\" offset=\"" + offset + "\">\n";
        xml += "<cells>\n";
        int c = 0, cx, cy;


        foreach (ProcessingImage i in images)
        {
            cx = c % fitPerRow;
            cy = Mathf.RoundToInt((c - cx) / fitPerRow);

            
            xml += "\t<StreamingSpriteXMLCell x = \"" + cx + "\" y = \"" + cy + "\" name = \"" + i.name +"\">\n";
     
            xml += "\t</StreamingSpriteXMLCell>\n";

            c++;
        }


        for (int i = c; i < fitPerAtlas; i++)
        {
            cx = i % fitPerRow;
            cy = Mathf.RoundToInt((i - cx) / fitPerRow);

            xml += "\t<StreamingSpriteXMLCell x = \"" + cx + "\" y = \"" + cy + "\" disable = \"1\">\n";
            xml += "\t</StreamingSpriteXMLCell>\n";

        }
        xml += "</cells>\n";
        xml += "</StreamingSpriteXMLObject>";

        File.WriteAllText(folder + "\\" + number + ".xml", xml);
    }

}
