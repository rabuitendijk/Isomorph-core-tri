
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class LinkerObject_XML : IXmlSerializable{

    public string name { get; protected set; }
    public int miplevels { get; protected set; }
    public List<Direction_XML> directions;

    public LinkerObject_XML()
    {
        name = "VOID";
        miplevels = -1;
        directions = new List<Direction_XML>();
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

            XmlSerializer serializer = new XmlSerializer(typeof(Direction_XML));
            reader.ReadToFollowing("directions");

            if (reader.ReadToDescendant("Direction_XML"))
            {
                do
                {
                    //Debug.Log("RAN3: "+reader.Name);
                    directions.Add((Direction_XML)serializer.Deserialize(reader));
                } while (reader.ReadToNextSibling("Direction_XML"));
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

                default:
                    Debug.Log("Warning switch: " + reader.Name);
                    break;
            }
        }
    }

}
