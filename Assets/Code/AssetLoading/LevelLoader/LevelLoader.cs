

using Application =  UnityEngine.Application;
using Debug = UnityEngine.Debug;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace AssetHandeling_LevelLoader
{
    /// <summary>
    /// Static class that loads a level file
    /// </summary>
    public static class LevelLoader
    {

        /// <summary>
        /// Attemps to load level into xml
        /// </summary>
        public static bool loadFile(string filename, out XMLO_LL_Level xml)
        {
            xml = null;
            string path = Application.streamingAssetsPath + "/Levels/" + filename + ".xml";
            if (!File.Exists(path))
            {
                Debug.Log("<color=red>Level loader file not found: </color>" + path);
                return false;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XMLO_LL_Level));
                XmlReader reader = XmlReader.Create(path);
                xml = (XMLO_LL_Level)serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }



            return true;
        }
    }
}