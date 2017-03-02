﻿using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildAliasTextures {

    int aliasSize = 2048;
    int offset = 16;

    int unitSize = 256;
    string target;

    int size, fitPerRow, fitPerAlias, number = 0;

    public BuildAliasTextures()
    {
        target = System.IO.Path.Combine(Application.streamingAssetsPath, "Alias");

        
        if (!Directory.Exists(target))
        {
            try {
                Directory.CreateDirectory(target);
            }catch(Exception e)
            {
                Debug.Log(e);
            }
        }

        size = unitSize + offset * 2;
        fitPerRow = Mathf.RoundToInt((aliasSize - aliasSize % size) / size);
        fitPerAlias = fitPerRow * fitPerRow;

        //Debug.Log("size: "+size+", perRow: "+fitPerRow+", perAlias: "+fitPerAlias);

        build();
    }

    void build()
    {
        SplicingScript ss = new SplicingScript();
        List<ProcessingObject> images = ss.Splice();

        //Debug.Log("Splcing list contains " + images.Count + " objects.");

        int count = 0;
        Texture2D currenctAlias = new Texture2D(aliasSize, aliasSize, TextureFormat.ARGB32, false);
        List<ProcessingObject> inAlias = new List<ProcessingObject>();

        foreach(ProcessingObject o  in images)
        {
            if (!fitInTexture(count, o))
            {
                currenctAlias = exportAlias(currenctAlias);
                writeXML(inAlias);
                number++;
                inAlias = new List<ProcessingObject>();
                count = 0;
            }

            inAlias.Add(o);

            foreach(ProcessingImage i in o.images)
            {
                writeImage(i, currenctAlias, count);
                count++;
            }
        }

        exportAlias(currenctAlias);
        writeXML(inAlias);
    }

    bool fitInTexture(int count, ProcessingObject o)
    {
        if (count + o.images.Count > fitPerAlias)
            return false;
        return true;
    }

    Texture2D exportAlias(Texture2D alias)
    {

        alias.Apply();

        byte[] bytes = alias.EncodeToPNG();
        File.WriteAllBytes(target + "\\" + number + ".png", bytes);

        return new Texture2D(aliasSize, aliasSize, TextureFormat.ARGB32, false);
    }

    void writeImage(ProcessingImage i, Texture2D alias, int count)
    {
        int cx = count % fitPerRow;
        int cy = Mathf.RoundToInt((count - cx) / fitPerRow);

        int ox = cx * size;
        int oy = cy * size;

        //Debug.Log("origin<"+ox+", "+oy+">");


        for (int j = 0; j < size; j++)
        {
            for (int k = 0; k < size; k++)
            {
                if (!(j < offset) && !(j >= unitSize + offset) && !(k < offset) && !(k >= unitSize + offset))
                {
                    alias.SetPixel(ox + j, oy + k, i.get(j - offset, k - offset));
                }
                else
                {
                    alias.SetPixel(ox + j, oy + k, new Color(0f, 0f, 0f, 0f));
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
