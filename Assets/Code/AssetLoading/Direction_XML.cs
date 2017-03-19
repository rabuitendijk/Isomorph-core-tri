
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Direction_XML : IXmlSerializable{
    public string direction { get; protected set; }
    public string source { get; protected set; }
    public bool linked { get; protected set; }
    public List<XMLCoord> coords;

    public Direction_XML()
    {
        direction = "VOID";
        source = "VOID";
        linked = false;
        
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

            if (!linked)
            {
                coords = new List<XMLCoord>();
                XmlSerializer serializer = new XmlSerializer(typeof(XMLCoord));
                reader.ReadToFollowing("coords");

                if (reader.ReadToDescendant("XMLCoord"))
                {
                    do
                    {
                        //Debug.Log("RAN3: "+reader.Name);
                        coords.Add((XMLCoord)serializer.Deserialize(reader));
                    } while (reader.ReadToNextSibling("XMLCoord"));
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

                case "direction":
                    direction = reader.ReadContentAsString();
                    break;

                case "source":
                    source = reader.ReadContentAsString();
                    break;

                case "linked":
                    linked = reader.ReadContentAsBoolean();
                    break;

                default:
                    Debug.Log("Warning switch: " + reader.Name);
                    break;
            }
        }
    }

}
