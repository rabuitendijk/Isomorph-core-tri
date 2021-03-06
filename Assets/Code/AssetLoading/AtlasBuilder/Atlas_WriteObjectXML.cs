﻿
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace AssetHandeling_AtlasBuilder
{
    /// <summary>
    /// Writes object xml data 
    /// </summary>
    public class Atlas_WriteObjectXML
    {
        public static int miplevels = 2;

        /// <summary>
        /// Writes all object xml data
        /// </summary>
        public static void write(string folder, List<XMLO_AB_SplicingObject> objects, Dictionary<string, SplicingSource> source, int mipl)
        {
            miplevels = mipl;
            makeDirectories(folder);

            foreach (XMLO_AB_SplicingObject ob in objects)
            {
                writeXML(ob, source, folder);
            }
        }

        /// <summary>
        /// Force directory createin
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

        /// <summary>
        /// Write single xml file
        /// </summary>
        static void writeXML(XMLO_AB_SplicingObject ob, Dictionary<string, SplicingSource> source, string folder)
        {
            string xml;

            xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
            xml += "<XMLO_AL_IsoObejct width=\"" + ob.width + "\" length=\"" + ob.length + "\" height=\"" + ob.height + "\" name=\"" + ob.name + "\" is_light = \"" + (ob.is_light ? "true" : "false") + "\" light_radius = \"" + ob.light_radius + "\" miplevels = \"" + miplevels + "\">\n";
            xml += "<directions>\n";

            string[] dirs = { "N", "E", "S", "W" };
            XMLO_AB_SplicingBody dir_ob;
            SplicingSource source_ob;

            for (int i = 0; i < dirs.Length; i++)
            {
                dir_ob = ob.getDirection(dirs[i]);
                if (dir_ob.link)
                    xml += "\t<XMLO_AL_Direction direction=\"" + dirs[i] + "\" linked = \"true\" source = \"" + dir_ob.source + "\">\n";
                else
                {
                    xml += "\t<XMLO_AL_Direction direction=\"" + dirs[i] + "\" linked = \"false\">\n";
                    if (source.TryGetValue(dir_ob.source, out source_ob))
                        xml += writeDirection(source_ob);
                    else
                        Debug.Log("Atlas_WriteObjectXML: source [" + dir_ob.source + "] missing in directory.");

                }
                xml += "\t</XMLO_AL_Direction>\n";
            }

            xml += "</directions>\n";
            xml += "</XMLO_AL_IsoObejct>\n";
            File.WriteAllText(folder + "/" + ob.name + ".xml", xml);

        }

        /// <summary>
        /// Write the direction component of the object
        /// </summary>
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
                            xml += "\t\t<XMLO_AL_Coord x=\"" + i + "\" y=\"" + j + "\" z=\"" + k + "\" name = \"VOID\">\n";
                        else
                            xml += "\t\t<XMLO_AL_Coord x=\"" + i + "\" y=\"" + j + "\" z=\"" + k + "\" name = \"" + source.mips[0][i, j, k].name + "\">\n";
                        xml += "\t\t</XMLO_AL_Coord>\n";
                    }
                }
            }

            xml += "\t</coords>\n";
            return xml;
        }
    }
}