
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Loads all [auto-generated] object xml files
/// </summary>
public static class Atlas_ObjectLoader  {
    static int count = 0;

    /// <summary>
    /// Loads all object xml files and retruns a dictionary containing them
    /// </summary>
    public static Dictionary<string, IsoObjectBody> load(string folder, SortedList<string, IsoObjectBody> objectsList)
    {
        Dictionary<string, IsoObjectBody> prototypes = new Dictionary<string, IsoObjectBody>();


        string[] subFiles = Directory.GetFiles(folder);

        foreach (string f in subFiles)
        {
            if (f.ToLower().EndsWith(".xml"))
                loadObject(f, prototypes, objectsList);
        }

        Debug.Log("Atlas_ObjectLoader: "+count+", objects loaded.");
        return prototypes;
    }

    /// <summary>
    /// Attemps to load single object xml file
    /// </summary>
    static void loadObject(string filename, Dictionary<string, IsoObjectBody> prototypes, SortedList<string, IsoObjectBody> objectsList)
    {
        LinkerObject_XML xml;

        XmlSerializer serializer = new XmlSerializer(typeof(LinkerObject_XML));
        XmlReader reader = XmlReader.Create(filename);
        xml = (LinkerObject_XML)serializer.Deserialize(reader);

        IsoObjectBody ob;

        try
        {
            ob = IsoObjectBody.prototype(xml);
            prototypes.Add(xml.name, ob);
            objectsList.Add(xml.name, ob);

            //Debug.Log("Loaded ["+xml.name+"].");
            count++;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
