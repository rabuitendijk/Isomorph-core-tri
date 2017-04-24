
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Static class that loads a level file
/// </summary>
public static class LevelLoader {

    /// <summary>
    /// Attemps to load level into xml
    /// </summary>
	public static bool loadFile(string filename, out Level_XML xml)
    {
        xml = null;
        string path = Application.streamingAssetsPath + "/Levels/" + filename + ".xml";
        if (!File.Exists(path))
        {
            Debug.Log("<color=red>Level loader file not found: </color>"+path);
            return false;
        }

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Level_XML));
            XmlReader reader = XmlReader.Create(path);
            xml = (Level_XML)serializer.Deserialize(reader);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }



        return true;
    }
}
