
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// Represents a level in the levellouder
/// </summary>
public class Level_XML : IXmlSerializable {

    public int width { get; protected set; }
    public int length { get; protected set; }
    public int height { get; protected set; }

    public List<IsoObject_XML> nodes;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Level_XML()
    {
        width = -1;
        height = -1;
        length = -1;
        nodes = new List<IsoObject_XML>();
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

            XmlSerializer serializer = new XmlSerializer(typeof(IsoObject_XML));
            reader.ReadToFollowing("objects");

            if (reader.ReadToDescendant("IsoObject_XML"))
            {
                do
                {
                    //Debug.Log("RAN3: "+reader.Name);
                    nodes.Add((IsoObject_XML)serializer.Deserialize(reader));
                } while (reader.ReadToNextSibling("IsoObject_XML"));
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
        return "<Level_XML>(width=" + width + ", height=" + height + ", length=" + length + ", objects=" + nodes.Count + ")";
    }
}
