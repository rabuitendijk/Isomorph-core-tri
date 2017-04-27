
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_AtlasBuilder
{
    /// <summary>
    /// Contains link to a source image for source object xml.
    /// </summary>
    public class XMLO_AB_SplicingSource : IXmlSerializable
    {
        public string source { get; protected set; }

        /// <summary>
        /// Common constructor
        /// </summary>
        public XMLO_AB_SplicingSource()
        {
            source = "VOID";
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

                    case "source":
                        source = reader.ReadContentAsString();
                        break;

                    default:
                        Debug.Log("Warning switch: " + reader.Name);
                        break;
                }
            }
        }
    }
}