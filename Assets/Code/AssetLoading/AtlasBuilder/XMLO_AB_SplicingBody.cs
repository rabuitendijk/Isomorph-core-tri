
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_AtlasBuilder { 
    /// <summary>
    /// Body of source object xml, contains link to source image
    /// </summary>
    public class XMLO_AB_SplicingBody : IXmlSerializable
    {

        public string direction { get; protected set; }
        public bool link { get; protected set; }
        public string source { get; protected set; }

        /// <summary>
        /// Common constructor
        /// </summary>
        public XMLO_AB_SplicingBody()
        {
            direction = "VOID";
            source = "VOID";
            link = false;
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
                    case "direction":
                        direction = reader.ReadContentAsString();
                        break;

                    case "source":
                        source = reader.ReadContentAsString();
                        break;

                    case "link":
                        link = reader.ReadContentAsBoolean();
                        break;

                    default:
                        Debug.Log("Warning switch: " + reader.Name);
                        break;
                }
            }
        }
    }
}