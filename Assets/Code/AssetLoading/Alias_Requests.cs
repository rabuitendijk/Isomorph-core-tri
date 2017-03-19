
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Get requests
/// </summary>
public static class Alias_Requests  {

    public static List<SplicingObject_XML> getRequests(string folder)
    {
        List<SplicingObject_XML> ret = new List<SplicingObject_XML>();

        RecersiveDirectoryCrawler(folder, ret);
        Debug.Log("Alias_Requests: "+ret.Count+" definitions loaded.");

        return ret;
    }


    /// <summary>
    /// Recersive first load later
    /// </summary>
    static void RecersiveDirectoryCrawler(string folder, List<SplicingObject_XML> objects)
    {


        //Recieve
        string[] subDirs = Directory.GetDirectories(folder);

        foreach (string s in subDirs)
        {
            RecersiveDirectoryCrawler(folder, objects);
        }

        //Load
        string[] subFiles = Directory.GetFiles(folder);

        foreach (string f in subFiles)
        {
            if (f.ToLower().EndsWith(".xml"))
            {
                loadXML(f.Substring(f.LastIndexOf('\\') + 1, f.Length - f.LastIndexOf('\\') - 5), f, objects);
                //Debug.Log("RecersiveDirectoryCrawler: " + f);
            }

        }
    }


    static void loadXML(string name, string filename, List<SplicingObject_XML> objects)
    {
        SplicingObject_XML xml;


        XmlSerializer serializer = new XmlSerializer(typeof(SplicingObject_XML));
        XmlReader reader = XmlReader.Create(filename);
        xml = (SplicingObject_XML)serializer.Deserialize(reader);
        xml.name = name;
        objects.Add(xml);

        //Debug.Log(xml.ToString());
    }

}
