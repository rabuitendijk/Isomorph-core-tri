
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_LevelLoader
{
    /// <summary>
    /// Level loader representation of IsoObject
    /// </summary>
    public class XMLO_LL_IsoObject : IXmlSerializable
    {
        public string name { get; protected set; }

        int x, y, z;
        public Iso origin { get { return new Iso(x, y, z); } }
        string dir;
        public Directions.dir direction { get { return Directions.getDir(dir); } }

        /// <summary>
        /// Default constructor
        /// </summary>
        public XMLO_LL_IsoObject()
        {
            name = "null";
            dir = "null";
            x = -1;
            y = -1;
            z = -1;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {

        }

        public void ReadXml(XmlReader reader)
        {
            try
            {

                readerRead(reader);

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        /// <summary>
        /// Main attribute reader
        /// </summary>
        void readerRead(XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "name":
                        name = reader.ReadContentAsString();
                        break;

                    case "direction":
                        dir = reader.ReadContentAsString();
                        break;

                    case "x":
                        x = reader.ReadContentAsInt();
                        break;

                    case "y":
                        y = reader.ReadContentAsInt();
                        break;

                    case "z":
                        z = reader.ReadContentAsInt();
                        break;

                    default:
                        Debug.Log("Warning switch: " + reader.Name);
                        break;
                }
            }
        }

    }
}