
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public static class Alias_WriteAlias  {

    static int aliasSize = 2048;
    static int offset = 16;
    static int miplevels = 2;

    static int unitSize = 256;


    static int size = unitSize + offset* 2;
    static int fitPerRow = Mathf.RoundToInt((aliasSize - aliasSize % size) / size);
    static int fitPerAlias = fitPerRow* fitPerRow;


    public static void write(string folder, List<SplicingSource> objects)
    {
        makeDirectories(folder);

        for (int i = 0; i < miplevels+1; i++)
        {
            build(i, folder, objects);
        }
    }


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
    static Texture2D currenctAlias;
    static void build(int miplevel, string folder, List<SplicingSource> objects)
    {

        //Debug.Log("Splcing list contains " + images.Count + " objects.");

        count = 0; number = 0;
        currenctAlias = new Texture2D((aliasSize >> miplevel), (aliasSize >> miplevel), TextureFormat.ARGB32, false);
        List<ProcessingImage> inAlias = new List<ProcessingImage>();

        foreach (SplicingSource ss in objects)
        {
            for (int i = 0; i < ss.width; i++)
            {
                for (int j = 0; j < ss.depth; j++)
                {
                    for (int k = 0; k < ss.height; k++)
                    {
                        if (ss.mips[miplevel][i, j, k] != null)
                        {
                            //Debug.Log("mip = "+miplevel+", ["+i+", "+j+", "+k+"], "+ ss.mips[miplevel][i, j, k].name+", ["+ ss.mips[miplevel][i, j, k] .width+", "+ ss.mips[miplevel][i, j, k] .height+ "]");
                            process(miplevel, folder, ss.mips[miplevel][i, j, k], inAlias);
                        }

                    }
                }

            }

        }

        writeXML(inAlias, folder);
        exportAlias(currenctAlias, folder, miplevel);
        
    }

    static void process(int miplevel, string folder, ProcessingImage image, List<ProcessingImage> inAlias)
    {
        if ( !(count<fitPerAlias))
        {
            writeXML(inAlias, folder);
            exportAlias(currenctAlias, folder, miplevel);

            currenctAlias = new Texture2D((aliasSize >> miplevel), (aliasSize >> miplevel), TextureFormat.ARGB32, false);

            
            count = 0;
        }

        inAlias.Add(image);

        writeImage(image, currenctAlias, count, (size >> miplevel), (offset >> miplevel), (unitSize >> miplevel));
        count++;
        


    }

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

    static void exportAlias(Texture2D alias, string folder, int miplevel)
    {

        alias.Apply();

        byte[] bytes = alias.EncodeToPNG();
        if (miplevel == 0)
            File.WriteAllBytes(folder + "/" + number + ".png", bytes);
        else
            File.WriteAllBytes(folder + "/" + number + ".m"+miplevel+".png", bytes);
        number++;
    }

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


        for (int i = c; i < fitPerAlias; i++)
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
