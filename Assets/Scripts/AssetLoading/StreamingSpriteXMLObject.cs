using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// version aplha-1
///	Stores data from read sprite .xml file
///
/// Robin Apollo Buitendijk
/// Late February 2017
/// </summary>
public class StreamingSpriteXMLObject : IXmlSerializable
{
	public int pixelsPerUnit {get; protected set;}
	public Vector2 pivotPoint;
	public bool multiSprite {get; protected set;}
	
	public int columns {get; protected set;}
	public int rows {get; protected set;}
	public int cellHeight {get; protected set;}
	public int cellWidth {get; protected set;}
	List<StreamingSpriteXMLCell> cells;
	
	
	
	public StreamingSpriteXMLObject()
	{
		pixelsPerUnit = 32;
		pivotPoint = new Vector2(0f, 0f);
		multiSprite = false;
		columns = 1;
		rows = 1;
		cellHeight = 32;
		cellWidth = 32;
		
		cells = new List<StreamingSpriteXMLCell>();
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
		try{
			
			readerRead(reader);
			
			XmlSerializer serializer = new XmlSerializer( typeof(StreamingSpriteXMLCell));
			reader.ReadToFollowing("cells");
			
			if (reader.ReadToDescendant("StreamingSpriteXMLCell"))
			{
				do
				{
					//Debug.Log("RAN3: "+reader.Name);
					cells.Add((StreamingSpriteXMLCell)serializer.Deserialize(reader));
				} while (reader.ReadToNextSibling("StreamingSpriteXMLCell"));
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
		while(reader.MoveToNextAttribute())
		{
			switch(reader.Name)
				{
					case "pixelsPerUnit":
						pixelsPerUnit = reader.ReadContentAsInt();
						break;
					
					case "multiSprite":
						multiSprite = reader.ReadContentAsBoolean();
						break;
					
					case "rows":
						rows = reader.ReadContentAsInt();
						break;
					
					case "columns":
						columns = reader.ReadContentAsInt();
						break;
					
					case "cellWidth":
						cellWidth = reader.ReadContentAsInt();
						break;
					
					case "cellHeight":
						cellHeight = reader.ReadContentAsInt();
						break;
						
					case "pivotX":
						pivotPoint.x = reader.ReadContentAsFloat();
						break;
					
					case "pivotY":
						pivotPoint.y = reader.ReadContentAsFloat();
						break;
					
					default:
						Debug.Log("Warning switch: "+reader.Name);
						break;
				}
		}
	}
	
	public StreamingSpriteXMLCell getCell(int x, int y)
	{
		foreach(StreamingSpriteXMLCell c in cells)
		{
			if (c.x == x && c.y == y)
				return c;
		}
		return null;
	}
}
