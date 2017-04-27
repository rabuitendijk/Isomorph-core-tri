

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_AtlasLoader
{
    /// <summary>
    /// alpha-1
    /// 
    /// Single tile entry xml data for object
    /// 
    /// Robin Apollo Buitendijk
    /// Early March 2017 
    /// </summary>
    public class XMLO_AL_Coord : IXmlSerializable
    {

        public int x { get; protected set; }
        public int y { get; protected set; }
        public int z { get; protected set; }
        public string spriteName { get; protected set; }
        public bool hasSprite { get; protected set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public XMLO_AL_Coord()
        {
            spriteName = "VOID";
            x = -1;
            y = -1;
            z = -1;
            hasSprite = true;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            return;
        }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                reader.MoveToAttribute("x");
                x = reader.ReadContentAsInt();
                reader.MoveToAttribute("y");
                y = reader.ReadContentAsInt();
                reader.MoveToAttribute("z");
                z = reader.ReadContentAsInt();
                reader.MoveToAttribute("name");
                spriteName = reader.ReadContentAsString();

                if (spriteName == "VOID")
                    hasSprite = false;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            //Debug.Log(ToString());
        }

        public override string ToString()
        {
            return "XMLO_AL_Sprite<x = " + x + ", y = " + y + ", z = " + z + ", spriteName = " + spriteName + ">";
        }
    }
}