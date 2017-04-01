
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

    public int width { get; protected set; }
    public int length { get; protected set; }
    public int height { get; protected set; }
	
	public bool is_light { get; protected set; }
	public int light_radius {get; protected set;}

    public LinkerObject_XML()
    {
        name = "VOID";
        miplevels = -1;
        directions = new List<Direction_XML>();

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

    public Direction_XML getDirection(string dir)
    {
        foreach (Direction_XML d in directions)
        {
            if (dir == d.direction)
                return d;

        }

        Debug.Log("LinkerObject_XML[" + name + "]: Did not find direction: " + dir);
        return null;
    }

}
