
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// Represents a coordinate in box space of source object xml.
/// Currently unused.
/// </summary>
public class SplicingSpace_XML : IXmlSerializable
{

    public int x { get; protected set; }
    public int y { get; protected set; }
    public int z { get; protected set; }
    public Iso coord { get { return new Iso(x, y, z); } }


    /// <summary>
    /// Common constructor
    /// </summary>
    public SplicingSpace_XML()
    {
        x = -1;
        y = -1;
        z = -1;
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

                case "x":
                    x = reader.ReadContentAsInt();
                    break;

                case "y":
                    y = reader.ReadContentAsInt();
                    break;

                case "z":
                    z = reader.ReadContentAsInt();
                    break;

                default:
                    Debug.Log("Warning switch: " + reader.Name);
                    break;
            }
        }
    }
}
