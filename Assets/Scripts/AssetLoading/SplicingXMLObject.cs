
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// alpha-1
/// 
/// XML automatic settings loading object
/// 
/// Robin Apollo Butiendijk
/// Late February 2017
/// </summary>
public class SplicingXMLObject : IXmlSerializable
{
    public int width { get; protected set; }
    public int depth { get; protected set; }
    public int height { get; protected set; }
    public string name { get; protected set; }
    public bool setName { get; protected set; }

    public SplicingXMLObject()
    {
        width = 0;
        height = 0;
        depth = 0;
        name = "NULL";
        setName = false;
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
                case "width":
                    width = reader.ReadContentAsInt();
                    break;

                case "height":
                    height = reader.ReadContentAsInt();
                    break;

                case "depth":
                    depth = reader.ReadContentAsInt();
                    break;

                case "name":
                    name = reader.ReadContentAsString();
                    setName = true;
                    break;

                default:
                    Debug.Log("Warning switch: " + reader.Name);
                    break;
            }
        }
    }
}
