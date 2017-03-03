using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// aplha-2
/// 
/// An alias texture builder.
/// Also creates mip ovveride textures for the generated aliasses.
/// 
/// Robin Apollo Buitendijk
/// Early March 2017
/// </summary>
public class BuildAliasTextures {

    int aliasSize = 2048;
    int offset = 16;

    int unitSize = 256;
    string target, targetMip;

    int size, fitPerRow, fitPerAlias, number = 0;

    public BuildAliasTextures()
    {
        makeDirectories();

        size = unitSize + offset * 2;
        fitPerRow = Mathf.RoundToInt((aliasSize - aliasSize % size) / size);
        fitPerAlias = fitPerRow * fitPerRow;

        //Debug.Log("size: "+size+", perRow: "+fitPerRow+", perAlias: "+fitPerAlias);

        build();
    }

    void makeDirectories()
    {
        target = System.IO.Path.Combine(Application.streamingAssetsPath, "Alias");


        if (!Directory.Exists(target))
        {
            try
            {
                Directory.CreateDirectory(target);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        targetMip = System.IO.Path.Combine(Application.streamingAssetsPath, "AliasMip");


        if (!Directory.Exists(target))
        {
            try
            {
                Directory.CreateDirectory(target);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    void build()
    {
        SplicingScript ss = new SplicingScript();
        List<ProcessingObject> images = ss.Splice();

        //Debug.Log("Splcing list contains " + images.Count + " objects.");

        int count = 0, resi = 0;
        Texture2D currenctAlias = new Texture2D(aliasSize, aliasSize, TextureFormat.ARGB32, false);
        Texture2D currenctMip1 = new Texture2D(aliasSize/2, aliasSize/2, TextureFormat.ARGB32, false);
        Texture2D currenctMip2 = new Texture2D(aliasSize/4, aliasSize/4, TextureFormat.ARGB32, false);
        List<ProcessingObject> inAlias = new List<ProcessingObject>();

        foreach(ProcessingObject o  in images)
        {
            if (!fitInTexture(count, o))
            {
                exportAlias(currenctAlias, currenctMip1, currenctMip2);

                currenctAlias = new Texture2D(aliasSize, aliasSize, TextureFormat.ARGB32, false);
                currenctMip1 = new Texture2D(aliasSize / 2, aliasSize / 2, TextureFormat.ARGB32, false);
                currenctMip2 = new Texture2D(aliasSize / 4, aliasSize / 4, TextureFormat.ARGB32, false);

                writeXML(inAlias);
                number++;
                inAlias = new List<ProcessingObject>();
                count = 0;
            }

            inAlias.Add(o);
            resi = 0;
            foreach(ProcessingImage i in o.images)
            {
                writeImage(i, currenctAlias, count+resi, size, offset, unitSize);
                resi++;
            }
            if (o.mip1 != null)
            {
                resi = 0;
                foreach (ProcessingImage i in o.mip1)
                {
                    writeImage(i, currenctMip1, count+resi, size / 2, offset/2, unitSize/2);
                    resi++;
                }
            }
            if (o.mip2 != null)
            {
                resi = 0;
                foreach (ProcessingImage i in o.mip2)
                {
                    writeImage(i, currenctMip2, count+resi, size / 4, offset/4, unitSize/4);
                    resi++;
                }
            }
            count += o.images.Count;
        }

        exportAlias(currenctAlias, currenctMip1, currenctMip2);
        writeXML(inAlias);
    }

    bool fitInTexture(int count, ProcessingObject o)
    {
        if (count + o.images.Count > fitPerAlias)
            return false;
        return true;
    }

    void exportAlias(Texture2D alias, Texture2D mip1, Texture2D mip2)
    {

        alias.Apply();
        mip1.Apply();
        mip2.Apply();

        byte[] bytes = alias.EncodeToPNG();
        File.WriteAllBytes(target + "\\" + number + ".png", bytes);
        bytes = mip1.EncodeToPNG();
        File.WriteAllBytes(targetMip + "\\" + number + ".m1.png", bytes);
        bytes = mip2.EncodeToPNG();
        File.WriteAllBytes(targetMip + "\\" + number + ".m2.png", bytes);

    }

    void writeImage(ProcessingImage i, Texture2D tex, int count, int size, int offset, int unitSize)
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

    void writeXML(List<ProcessingObject> objects)
    {
        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";

        xml += "<StreamingSpriteXMLObject multiSprite = \"1\" rows = \"" + fitPerRow + "\" columns = \"" + fitPerRow + "\" pixelsPerUnit=\"" + unitSize + "\" cellHeight=\"" + unitSize + "\" cellWidth=\"" + unitSize + "\" offset=\"" + offset + "\">\n";
        xml += "<cells>\n";
        int c = 0, cx, cy;
 

        foreach (ProcessingObject o in objects)
        {
            foreach (ProcessingImage i in o.images)
            {
                cx = c % fitPerRow;
                cy = Mathf.RoundToInt((c - cx) / fitPerRow);

                if (o.images.Count != 1)
                    xml += "\t<StreamingSpriteXMLCell x = \"" + cx + "\" y = \"" + cy + "\" name = \"" + o.name + "[" + i.coord.x + "_" + i.coord.y + "_" + i.coord.z + "]\">\n";
                else
                    xml += "\t<StreamingSpriteXMLCell x = \"" + cx + "\" y = \"" + cy + "\" name = \"" + o.name +"\">\n";
                xml += "\t</StreamingSpriteXMLCell>\n";

                c++;
            }
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

        File.WriteAllText(target+"\\"+number+".png.xml", xml);
    }
}
