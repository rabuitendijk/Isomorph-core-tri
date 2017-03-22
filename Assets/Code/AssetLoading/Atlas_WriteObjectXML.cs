
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Atlas_WriteObjectXML {
    public static int miplevels = 2;

	public static void write(string folder, List<SplicingObject_XML> objects, Dictionary<string, SplicingSource> source)
    {
        makeDirectories(folder);

        foreach(SplicingObject_XML ob in objects)
        {
            writeXML(ob, source, folder);
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

    static void writeXML(SplicingObject_XML ob, Dictionary<string, SplicingSource> source, string folder)
    {
        string xml;

        xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
        xml += "<LinkerObject_XML name=\"" + ob.name + "\" miplevels = \""+miplevels+"\">\n";
        xml += "<directions>\n";

        string[] dirs = { "N", "E", "S", "W"};
        SplicingBody_XML dir_ob;
        SplicingSource source_ob;

        for (int i = 0; i < dirs.Length; i++)
        {
            dir_ob = ob.getDirection(dirs[i]);
            if (dir_ob.link)
                xml += "\t<Direction_XML direction=\"" + dirs[i] + "\" linked = \"true\" source = \"" + dir_ob.source + "\">\n";
            else
            {
                xml += "\t<Direction_XML direction=\"" + dirs[i] + "\" linked = \"false\">\n";
                if (source.TryGetValue(dir_ob.source, out source_ob))
                    xml+=writeDirection(source_ob);
                else
                    Debug.Log("Atlas_WriteObjectXML: source ["+dir_ob.source+"] missing in directory.");
                
            }
            xml += "\t</Direction_XML>\n";
        }

        xml += "</directions>\n";
        xml += "</LinkerObject_XML>\n";
        File.WriteAllText(folder+ "/" + ob.name + ".xml", xml);
        
    }

    static string writeDirection(SplicingSource source)
    {
        string xml = "\t<coords>\n";

        for (int i = 0; i < source.width; i++)
        {
            for (int j = 0; j < source.length; j++)
            {
                for (int k = 0; k < source.height; k++)
                {
                    if (source.mips[0][i, j, k] == null) //Void
                        xml += "\t\t<XMLCoord x=\"" + i + "\" y=\"" + j + "\" z=\"" + k + "\" name = \"VOID\">\n";
                    else
                        xml += "\t\t<XMLCoord x=\"" + i + "\" y=\"" + j + "\" z=\"" + k + "\" name = \"" + source.mips[0][i, j, k].name + "\">\n";
                    xml += "\t\t</XMLCoord>\n";
                }
            }
        }

        xml += "\t</coords>\n";
        return xml;
    }
}
