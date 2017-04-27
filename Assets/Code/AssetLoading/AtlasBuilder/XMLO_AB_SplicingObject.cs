
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_AtlasBuilder
{
    /// <summary>
    /// Root of source object xml
    /// </summary>
    public class XMLO_AB_SplicingObject : IXmlSerializable
    {

        public int width { get; protected set; }
        public int trueHeight { get; protected set; }
        public int length { get; protected set; }
        public int height { get; protected set; }
        public bool boxform { get; protected set; }
        public bool is_light { get; protected set; }
        public int light_radius { get; protected set; }
        public string name = "VOID";

        public List<XMLO_AB_SplicingSpace> space { get; protected set; }
        public List<XMLO_AB_SplicingSource> source { get; protected set; }
        public List<XMLO_AB_SplicingBody> body { get; protected set; }

        /// <summary>
        /// Common constructor
        /// </summary>
        public XMLO_AB_SplicingObject()
        {
            width = -1;
            trueHeight = 1;
            height = -1;
            length = -1;
            boxform = true;
            is_light = false;
            light_radius = 8;

            space = new List<XMLO_AB_SplicingSpace>();
            source = new List<XMLO_AB_SplicingSource>();
            body = new List<XMLO_AB_SplicingBody>();

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

                XmlSerializer serializer = new XmlSerializer(typeof(XMLO_AB_SplicingSource));
                reader.ReadToFollowing("source");

                if (reader.ReadToDescendant("XMLO_AB_SplicingSource"))
                {
                    do
                    {
                        //Debug.Log("RAN3: "+reader.Name);
                        source.Add((XMLO_AB_SplicingSource)serializer.Deserialize(reader));
                    } while (reader.ReadToNextSibling("XMLO_AB_SplicingSource"));
                }


                serializer = new XmlSerializer(typeof(XMLO_AB_SplicingBody));
                reader.ReadToFollowing("body");

                if (reader.ReadToDescendant("XMLO_AB_SplicingBody"))
                {
                    do
                    {
                        //Debug.Log("RAN3: "+reader.Name);
                        body.Add((XMLO_AB_SplicingBody)serializer.Deserialize(reader));
                    } while (reader.ReadToNextSibling("XMLO_AB_SplicingBody"));
                }

                if (!boxform)
                {
                    serializer = new XmlSerializer(typeof(XMLO_AB_SplicingSpace));
                    reader.ReadToFollowing("space");

                    if (reader.ReadToDescendant("XMLO_AB_SplicingSpace"))
                    {
                        do
                        {
                            //Debug.Log("RAN3: "+reader.Name);
                            space.Add((XMLO_AB_SplicingSpace)serializer.Deserialize(reader));
                        } while (reader.ReadToNextSibling("XMLO_AB_SplicingSpace"));
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
                    case "boxform":
                        boxform = reader.ReadContentAsBoolean();
                        break;

                    case "is_light":
                        is_light = reader.ReadContentAsBoolean();
                        break;

                    case "light_radius":
                        light_radius = reader.ReadContentAsInt();
                        break;

                    case "width":
                        width = reader.ReadContentAsInt();
                        break;

                    case "length":
                        length = reader.ReadContentAsInt();
                        break;

                    case "height":
                        height = reader.ReadContentAsInt();
                        trueHeight = height;
                        height = (height + height % 2) / 2;
                        break;

                    default:
                        Debug.Log("Warning switch: " + reader.Name);
                        break;
                }
            }
        }

        public override string ToString()
        {
            return "<XMLO_AB_SplicingObject>(width = " + width + ", height = " + height + ", length = " + length + ", boxform = " + boxform + ", source = " + source.Count + ", body = " + body.Count + ", space = " + space.Count + ")";
        }

        public XMLO_AB_SplicingBody getDirection(string dir)
        {
            foreach (XMLO_AB_SplicingBody b in body)
            {
                if (dir == b.direction)
                    return b;

            }

            Debug.Log("XMLO_AB_SplicingObject[" + name + "]: Did not find direction: " + dir);
            return null;
        }
    }
}