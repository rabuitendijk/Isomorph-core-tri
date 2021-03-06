﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using AssetHandeling_AtlasLoader;

/// <summary>
/// version alpha-1
/// Fully automated loader of sprites from StreamingAssets Folder.
/// 
/// Requires custom: XMLO_AL_Sprite, XMLO_AL_Atlas
///
/// Use filename.png.xml to edit propeties
/// tags:
/// <?xml version="1.0" encoding="utf-8"?>
///
/// <XMLO_AL_Atlas multiSprite="1" rows="4" columns="4" pixelsPerUnit="32" cellHeight="32" cellWidth="32">
///	<cells>
///		<XMLO_AL_Sprite x="1" y="2" name="floor">
///		</XMLO_AL_Sprite>
///		<XMLO_AL_Sprite x="3" y="1" name="wall">
///		</XMLO_AL_Sprite>
///		<XMLO_AL_Sprite x="3" y="2" disable="1">
///		</XMLO_AL_Sprite>
///	</cells>
/// </XMLO_AL_Atlas>
///
/// Robin Apollo Buitendijk
/// Late February 2017
/// </summary>
public class StreamingSpriteLoader
{

	static StreamingSpriteLoader main;
	public static StreamingSpriteLoader Main{
		get{
			return main;
		}
	}
	
	int count = 0;
	Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
	
	/// <summary>
    /// Constructor loads sprite upon cration
    /// </summary>
	public StreamingSpriteLoader()
	{
		main = this;
		LoadSprites();
	}
	
	
	/// <summary>
    /// Call to start loading sprites
    /// </summary>
	private void LoadSprites()
	{
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Images");
		RecersiveDirectoryCrawler(filePath);
		
		Debug.Log("Streaming sprites: " +count +", sprites loaded");
	}
	
	/// <summary>
    /// Recersive first load later sprite loader
    /// </summary>
	void RecersiveDirectoryCrawler(string path)
	{
		
		//Debug.Log("RecersiveDirectoryCrawler: "+path);
		
		
		string[] subDirs = Directory.GetDirectories (path);
		
		foreach (string s in subDirs)
		{
			RecersiveDirectoryCrawler(s);
		}
		
		string[] subFiles = Directory.GetFiles (path);
		
		foreach (string f in subFiles)
		{
			if (f.ToLower().EndsWith(".png") || f.ToLower().EndsWith(".jpg"))
				LoadSprite(f.Substring(f.LastIndexOf('\\') +1, f.Length - f.LastIndexOf('\\') -5), f);
		}
	}
	
	/// <summary>
    /// Quills sprite loader, with modifications
    /// </summary>
	void LoadSprite(string spriteName, string filePath) {
		XMLO_AL_Atlas xml;
		
		if (File.Exists(filePath+".xml"))
		{
			XmlSerializer serializer = new XmlSerializer( typeof(XMLO_AL_Atlas));
			XmlReader reader = XmlReader.Create(filePath+".xml");
			xml = (XMLO_AL_Atlas)serializer.Deserialize(reader);
		}
		else
			xml = new XMLO_AL_Atlas();
		
		
		byte[] imageBytes = File.ReadAllBytes( filePath );

		Texture2D imageTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);	// Create some kind of dummy instance of Texture2D
		imageTexture.filterMode = FilterMode.Point;
        imageTexture.wrapMode = TextureWrapMode.Clamp;
        imageTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file
        //imageTexture.Compress(false);
		
		if (xml.multiSprite)
		{
			processMultiSprite(xml, imageTexture, spriteName);
		}
		else
		{
			Rect spriteCoordinates = new Rect(0, 0, imageTexture.width, imageTexture.height);	// In pixels!
			addSprite(imageTexture, spriteCoordinates, spriteName, xml.pivotPoint, xml.pixelsPerUnit);
		}

	}
	
	/// <summary>
    /// Processes cases of multy sprite sheet
    /// </summary>
	void processMultiSprite(XMLO_AL_Atlas xml, Texture2D imageTexture, string spriteName)
	{
		Rect spriteCoordinates;
		XMLO_AL_Sprite cell;
		string trueName;
		
		for	(int i=0; i<xml.columns; i++)
			{
				for (int j=0; j<xml.rows; j++)
				{
					
					cell = xml.getCell(i,j);
					if (cell != null)
					{
						if (!cell.disable)
						{
							if (!cell.name.Equals("NULL"))
								trueName = cell.name;
							else
								trueName = spriteName+"("+i+"_"+j+")"; //Cosider alerations
							
							spriteCoordinates = new Rect(i*(xml.cellWidth+2*xml.offset)+xml.offset, j*(xml.cellHeight+2*xml.offset)+xml.offset, xml.cellWidth, xml.cellHeight);	// In pixels!
							addSprite(imageTexture, spriteCoordinates, trueName, xml.pivotPoint, xml.pixelsPerUnit);
                            //Debug.Log(cell.name);
						}
					}
					else
					{
						spriteCoordinates = new Rect(i * (xml.cellWidth + 2 * xml.offset) + xml.offset, j * (xml.cellHeight + 2 * xml.offset) + xml.offset, xml.cellWidth, xml.cellHeight);	// In pixels!
						addSprite(imageTexture, spriteCoordinates, spriteName+"("+i+"_"+j+")", xml.pivotPoint, xml.pixelsPerUnit);
					}

				}
			}
	}
	
	/// <summary>
    /// Adds and creates sprite to dictionairy
    /// </summary>
	void addSprite(Texture2D imageTexture, Rect spriteCoordinates, string spriteName, Vector2 pivotPoint, int pixelsPerUnit)
	{
		Sprite s = Sprite.Create(imageTexture, spriteCoordinates, pivotPoint, pixelsPerUnit);
	    
		try
		{
			sprites.Add(spriteName, s);
			count++;
		}
		catch (Exception e){
			Debug.Log(e);
		}
	}
	
	/// <summary>
    /// Trys to get Sprite or returns null
    /// </summary>
    /// <param name="name">name of target Sprite</param>
    /// <returns>the Sprite</returns>
    public Sprite getSprite(string name) {
        Sprite sp;
        if (sprites.TryGetValue(name, out sp)) {
            return sp;
        }

        Debug.Log("Sprite: "+name+", not found");
        return null;
    }
}
