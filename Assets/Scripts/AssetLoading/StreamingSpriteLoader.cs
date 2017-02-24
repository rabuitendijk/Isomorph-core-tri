using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// version alpha-1
/// Fully automated loader of sprites from StreamingAssets Folder.
/// 
/// Requires custom: StreamingSpriteXMLCell, StreamingSpriteXMLObject
///
/// Use filename.png.xml to edit propeties
/// tags:
/// <?xml version="1.0" encoding="utf-8"?>
///
/// <StreamingSpriteXMLObject multiSprite="1" rows="4" columns="4" pixelsPerUnit="32" cellHeight="32" cellWidth="32">
///	<cells>
///		<StreamingSpriteXMLCell x="1" y="2" name="floor">
///		</StreamingSpriteXMLCell>
///		<StreamingSpriteXMLCell x="3" y="1" name="wall">
///		</StreamingSpriteXMLCell>
///		<StreamingSpriteXMLCell x="3" y="2" disable="1">
///		</StreamingSpriteXMLCell>
///	</cells>
/// </StreamingSpriteXMLObject>
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
		StreamingSpriteXMLObject xml;
		
		if (File.Exists(filePath+".xml"))
		{
			XmlSerializer serializer = new XmlSerializer( typeof(StreamingSpriteXMLObject));
			XmlReader reader = XmlReader.Create(filePath+".xml");
			xml = (StreamingSpriteXMLObject)serializer.Deserialize(reader);
		}
		else
			xml = new StreamingSpriteXMLObject();
		
		
		byte[] imageBytes = File.ReadAllBytes( filePath );

		Texture2D imageTexture = new Texture2D(2, 2);	// Create some kind of dummy instance of Texture2D
		imageTexture.filterMode = FilterMode.Point;
		imageTexture.LoadImage(imageBytes);	// This will correctly resize the texture based on the image file
		
		
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
	void processMultiSprite(StreamingSpriteXMLObject xml, Texture2D imageTexture, string spriteName)
	{
		Rect spriteCoordinates;
		StreamingSpriteXMLCell cell;
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
							if (!cell.name.Equals(""))
								trueName = cell.name;
							else
								trueName = spriteName+"("+i+"_"+j+")"; //Cosider alerations
							
							spriteCoordinates = new Rect(i*xml.cellWidth, j*xml.cellHeight, xml.cellWidth, xml.cellHeight);	// In pixels!
							addSprite(imageTexture, spriteCoordinates, trueName, xml.pivotPoint, xml.pixelsPerUnit);
						}
					}
					else
					{
						spriteCoordinates = new Rect(i*xml.cellWidth, j*xml.cellHeight, xml.cellWidth, xml.cellHeight);	// In pixels!
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
