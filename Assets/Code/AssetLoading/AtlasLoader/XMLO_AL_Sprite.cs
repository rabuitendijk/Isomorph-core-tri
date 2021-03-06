﻿

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_AtlasLoader
{
    /// <summary>
    /// version alpha-1
    /// Class for operation of .xml cell modifications
    /// Reprisents the data of a cell in Atlas texture
    /// 
    /// Robin Apollo Buitendijk
    /// Late February 2017
    /// </summary>
    public class XMLO_AL_Sprite : IXmlSerializable
    {

        public int x { get; protected set; }
        public int y { get; protected set; }
        public bool disable { get; protected set; }
        public string name { get; protected set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public XMLO_AL_Sprite()
        {
            name = "NULL";
            disable = false;
            x = -1;
            y = -1;
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
                reader.MoveToAttribute("x");
                x = reader.ReadContentAsInt();
                reader.MoveToAttribute("y");
                y = reader.ReadContentAsInt();

                if (reader.MoveToAttribute("disable"))
                    disable = reader.ReadContentAsBoolean();

                if (reader.MoveToAttribute("name"))
                    name = reader.ReadContentAsString();

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            //Debug.Log(ToString());
        }

        public override string ToString()
        {
            return "XMLO_AL_Sprite<x = " + x + ", y = " + y + ", name = " + name + ", diable = " + disable + ">";
        }
    }
}