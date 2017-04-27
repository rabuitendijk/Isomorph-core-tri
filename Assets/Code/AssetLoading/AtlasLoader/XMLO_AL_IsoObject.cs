
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_AtlasLoader
{
    /// <summary>
    /// Body of generated object xml
    /// </summary>
    public class XMLO_AL_IsoObejct : IXmlSerializable
    {

        public string name { get; protected set; }
        public int miplevels { get; protected set; }
        public List<XMLO_AL_Direction> directions;

        public int width { get; protected set; }
        public int length { get; protected set; }
        public int height { get; protected set; }

        public bool is_light { get; protected set; }
        public int light_radius { get; protected set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public XMLO_AL_IsoObejct()
        {
            name = "VOID";
            miplevels = -1;
            directions = new List<XMLO_AL_Direction>();

            width = -1;
            length = -1;
            height = -1;

            is_light = false;
            light_radius = 8;
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

                XmlSerializer serializer = new XmlSerializer(typeof(XMLO_AL_Direction));
                reader.ReadToFollowing("directions");

                if (reader.ReadToDescendant("XMLO_AL_Direction"))
                {
                    do
                    {
                        //Debug.Log("RAN3: "+reader.Name);
                        directions.Add((XMLO_AL_Direction)serializer.Deserialize(reader));
                    } while (reader.ReadToNextSibling("XMLO_AL_Direction"));
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

                    case "miplevels":
                        miplevels = reader.ReadContentAsInt();
                        break;

                    case "name":
                        name = reader.ReadContentAsString();
                        break;


                    case "length":
                        length = reader.ReadContentAsInt();
                        break;


                    case "width":
                        width = reader.ReadContentAsInt();
                        break;


                    case "height":
                        height = reader.ReadContentAsInt();
                        break;

                    case "is_light":
                        is_light = reader.ReadContentAsBoolean();
                        break;

                    case "light_radius":
                        light_radius = reader.ReadContentAsInt();
                        break;

                    default:
                        Debug.Log("Warning switch: " + reader.Name);
                        break;
                }
            }
        }

        public XMLO_AL_Direction getDirection(string dir)
        {
            foreach (XMLO_AL_Direction d in directions)
            {
                if (dir == d.direction)
                    return d;

            }

            Debug.Log("XMLO_AL_IsoObejct[" + name + "]: Did not find direction: " + dir);
            return null;
        }

    }
}