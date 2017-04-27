
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_LevelLoader
{
    /// <summary>
    /// Represents a level in the levellouder
    /// </summary>
    public class XMLO_LL_Level : IXmlSerializable
    {

        public int width { get; protected set; }
        public int length { get; protected set; }
        public int height { get; protected set; }

        public List<XMLO_LL_IsoObject> nodes;

        /// <summary>
        /// Default constructor
        /// </summary>
        public XMLO_LL_Level()
        {
            width = -1;
            height = -1;
            length = -1;
            nodes = new List<XMLO_LL_IsoObject>();
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

                //Debug.Log(ToString());

                XmlSerializer serializer = new XmlSerializer(typeof(XMLO_LL_IsoObject));
                reader.ReadToFollowing("objects");

                if (reader.ReadToDescendant("XMLO_LL_IsoObject"))
                {
                    do
                    {
                        //Debug.Log("RAN3: "+reader.Name);
                        nodes.Add((XMLO_LL_IsoObject)serializer.Deserialize(reader));
                    } while (reader.ReadToNextSibling("XMLO_LL_IsoObject"));
                }

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


                    case "width":
                        width = reader.ReadContentAsInt();
                        break;

                    case "height":
                        height = reader.ReadContentAsInt();
                        break;

                    case "length":
                        length = reader.ReadContentAsInt();
                        break;

                    default:
                        Debug.Log("Warning switch: " + reader.Name);
                        break;
                }
            }
        }

        public override string ToString()
        {
            return "<XMLO_LL_Level>(width=" + width + ", height=" + height + ", length=" + length + ", objects=" + nodes.Count + ")";
        }
    }
}