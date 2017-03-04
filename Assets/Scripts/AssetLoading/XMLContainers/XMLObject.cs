
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// aplha-1
/// 
/// First version of object data xml
/// bonds together an object with a name, coordinates and its sprites.
/// 
/// Robin Apollo Butiendijk
/// Early March 2017 
/// </summary>
public class XMLObject : IXmlSerializable
{

    public List<XMLCoord> coords { get; protected set; }
    public string name { get; protected set; }


    public XMLObject()
    {
        
        coords = new List<XMLCoord>();
        name = "NULL";
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
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Main attribute reader
    /// Unused currently
    /// </summary>
    void readerRead(XmlReader reader)
    {
        while (reader.MoveToNextAttribute())
        {
            switch (reader.Name)
            {
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
