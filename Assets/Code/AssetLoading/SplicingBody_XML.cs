
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class SplicingBody_XML : IXmlSerializable
{

    public string direction { get; protected set; }
    public bool link { get; protected set; }
    public string source { get; protected set; }

    public SplicingBody_XML()
    {
        direction = "VOID";
        source = "VOID";
        link = false;
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
                case "direction":
                    direction = reader.ReadContentAsString();
                    break;

                case "source":
                    source = reader.ReadContentAsString();
                    break;

                case "link":
                    link = reader.ReadContentAsBoolean();
                    break;

                default:
                    Debug.Log("Warning switch: " + reader.Name);
                    break;
            }
        }
    }
}
