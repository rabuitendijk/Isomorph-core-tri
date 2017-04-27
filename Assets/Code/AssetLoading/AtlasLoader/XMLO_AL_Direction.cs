
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_AtlasLoader
{
    /// <summary>
    /// Represents the an image at a given rotation
    /// </summary>
    public class XMLO_AL_Direction : IXmlSerializable
    {
        public string direction { get; protected set; }
        public string source { get; protected set; }
        public bool linked { get; protected set; }
        public List<XMLO_AL_Coord> coords;

        /// <summary>
        /// Default constructor
        /// </summary>
        public XMLO_AL_Direction()
        {
            direction = "VOID";
            source = "VOID";
            linked = false;

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

                if (!linked)
                {
                    coords = new List<XMLO_AL_Coord>();
                    XmlSerializer serializer = new XmlSerializer(typeof(XMLO_AL_Coord));
                    reader.ReadToFollowing("coords");

                    if (reader.ReadToDescendant("XMLO_AL_Coord"))
                    {
                        do
                        {
                            //Debug.Log("RAN3: "+reader.Name);
                            coords.Add((XMLO_AL_Coord)serializer.Deserialize(reader));
                        } while (reader.ReadToNextSibling("XMLO_AL_Coord"));
                    }
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

                    case "direction":
                        direction = reader.ReadContentAsString();
                        break;

                    case "source":
                        source = reader.ReadContentAsString();
                        break;

                    case "linked":
                        linked = reader.ReadContentAsBoolean();
                        break;

                    default:
                        Debug.Log("Warning switch: " + reader.Name);
                        break;
                }
            }
        }

    }
}