
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// Root of source object xml
/// </summary>
public class SplicingObject_XML : IXmlSerializable
{

    public int width { get; protected set; }
    public int trueHeight { get; protected set; }
    public int length { get; protected set; }
    public int height { get; protected set; }
    public bool boxform { get; protected set; }
	public bool is_light { get; protected set; }
	public int light_radius {get; protected set;}
    public string name = "VOID"; 

    public List<SplicingSpace_XML> space { get; protected set; }
    public List<SplicingSource_XML> source { get; protected set; }
    public List<SplicingBody_XML> body { get; protected set; }

    /// <summary>
    /// Common constructor
    /// </summary>
    public SplicingObject_XML()
    {
        width = -1;
        trueHeight = 1;
        height = -1;
        length = -1;
        boxform = true;
		is_light = false;
		light_radius = 8;

        space = new List<SplicingSpace_XML>();
        source = new List<SplicingSource_XML>();
        body = new List<SplicingBody_XML>();

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

            XmlSerializer serializer = new XmlSerializer(typeof(SplicingSource_XML));
            reader.ReadToFollowing("source");

            if (reader.ReadToDescendant("SplicingSource_XML"))
            {
                do
                {
                    //Debug.Log("RAN3: "+reader.Name);
                    source.Add((SplicingSource_XML)serializer.Deserialize(reader));
                } while (reader.ReadToNextSibling("SplicingSource_XML"));
            }


            serializer = new XmlSerializer(typeof(SplicingBody_XML));
            reader.ReadToFollowing("body");

            if (reader.ReadToDescendant("SplicingBody_XML"))
            {
                do
                {
                    //Debug.Log("RAN3: "+reader.Name);
                    body.Add((SplicingBody_XML)serializer.Deserialize(reader));
                } while (reader.ReadToNextSibling("SplicingBody_XML"));
            }

            if (!boxform)
            {
                serializer = new XmlSerializer(typeof(SplicingSpace_XML));
                reader.ReadToFollowing("space");

                if (reader.ReadToDescendant("SplicingSpace_XML"))
                {
                    do
                    {
                        //Debug.Log("RAN3: "+reader.Name);
                        space.Add((SplicingSpace_XML)serializer.Deserialize(reader));
                    } while (reader.ReadToNextSibling("SplicingSpace_XML"));
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
        return "<SplicingObject_XML>(width = "+width+", height = "+height+", length = "+length+", boxform = "+boxform+", source = "+source.Count+", body = "+body.Count+", space = "+space.Count+")";
    }

    public SplicingBody_XML getDirection(string dir)
    {
        foreach(SplicingBody_XML b in body)
        {
            if (dir == b.direction)
                return b;

        }

        Debug.Log("SplicingObject_XML["+name+"]: Did not find direction: "+dir);
        return null;
    }
}
