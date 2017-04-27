
using System.Collections.Generic;
using System.IO;
using Debug =  UnityEngine.Debug;
using System.Xml;
using System.Xml.Serialization;

namespace AssetHandeling_AtlasBuilder
{
    /// <summary>
    /// Get requests to add sprites to Atlas
    /// </summary>
    public static class Atlas_Requests
    {

        public static List<XMLO_AB_SplicingObject> getRequests(string folder)
        {
            List<XMLO_AB_SplicingObject> ret = new List<XMLO_AB_SplicingObject>();

            RecersiveDirectoryCrawler(folder, ret);
            Debug.Log("Atlas_Requests: " + ret.Count + " definitions loaded.");

            return ret;
        }


        /// <summary>
        /// Recersive first load later
        /// </summary>
        static void RecersiveDirectoryCrawler(string folder, List<XMLO_AB_SplicingObject> objects)
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

        /// <summary>
        /// Load single file
        /// </summary>
        static void loadXML(string name, string filename, List<XMLO_AB_SplicingObject> objects)
        {
            XMLO_AB_SplicingObject xml;


            XmlSerializer serializer = new XmlSerializer(typeof(XMLO_AB_SplicingObject));
            XmlReader reader = XmlReader.Create(filename);
            xml = (XMLO_AB_SplicingObject)serializer.Deserialize(reader);
            xml.name = name;
            objects.Add(xml);

            //Debug.Log(xml.ToString());
        }

    }
}