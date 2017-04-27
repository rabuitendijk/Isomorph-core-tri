
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace AssetHandeling_AtlasLoader
{
    /// <summary>
    /// Loads the sprites of the generated atlas
    /// </summary>
    public static class Atlas_SpriteLoader
    {

        public static int count = 0;

        public static Dictionary<string, Sprite> load(string folder)
        {
            Dictionary<string, Sprite> ret = new Dictionary<string, Sprite>();

            string[] subFiles = Directory.GetFiles(folder);

            foreach (string f in subFiles)
            {
                if (f.ToLower().EndsWith(".xml"))
                    LoadAtlas(f.Substring(0, f.Length - 4), ret);
            }

            Debug.Log("Atlas_SpriteLoader: " + count + ", sprites loaded.");

            return ret;
        }

        /// <summary>
        /// Loads a single atlas
        /// </summary>
        static void LoadAtlas(string filename, Dictionary<string, Sprite> dict)
        {
            XMLO_AL_Atlas xml;

            XmlSerializer serializer = new XmlSerializer(typeof(XMLO_AL_Atlas));
            XmlReader reader = XmlReader.Create(filename + ".xml");
            xml = (XMLO_AL_Atlas)serializer.Deserialize(reader);


            if (!File.Exists(filename + ".png"))
            {
                Debug.Log("Atlas loader missing png: " + filename + ".png");
                return;
            }


            byte[] imageBytes = File.ReadAllBytes(filename + ".png");

            //Texture2D imageTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);  
            Texture2D imageTexture = new Texture2D(2, 2);
            imageTexture.filterMode = FilterMode.Point;
            imageTexture.wrapMode = TextureWrapMode.Clamp;
            imageTexture.LoadImage(imageBytes);
            //imageTexture.Compress(false);


            overrideMips(imageTexture, filename, xml.miplevels);

            /*
            for (int i = 0; i < imageTexture.mipmapCount; i++)
            {
                Debug.Log("Mip level: "+i+", Expect: "+(2048>>i)*(2048>>i)+", got: "+ imageTexture.GetPixels32(i).Length);
            }
            */


            processAtlas(xml, imageTexture, dict);


        }

        /// <summary>
        /// Override the mip levels of the loaded atlas
        /// </summary>
        static void overrideMips(Texture2D tex, string filename, int miplevels)
        {
            byte[] imageBytes;
            Texture2D imageTexture;

            for (int i = 1; i < miplevels + 1; i++)
            {
                if (File.Exists(filename + ".m" + i + ".png"))
                {
                    imageBytes = File.ReadAllBytes(filename + ".m" + i + ".png");
                    imageTexture = new Texture2D(2, 2);
                    imageTexture.LoadImage(imageBytes);

                    tex.SetPixels32(imageTexture.GetPixels32(), i);
                }
                else
                {
                    Debug.Log("Mip not found: " + filename + ".m" + i + ".png");
                }
            }



            tex.Apply(false);
        }


        /// <summary>
        /// Exstracts the sprites form the atlas
        /// </summary>
        static void processAtlas(XMLO_AL_Atlas xml, Texture2D imageTexture, Dictionary<string, Sprite> dict)
        {
            Rect spriteCoordinates;
            XMLO_AL_Sprite cell;

            for (int i = 0; i < xml.columns; i++)
            {
                for (int j = 0; j < xml.rows; j++)
                {

                    cell = xml.getCell(i, j);
                    if (cell != null)
                    {
                        if (!cell.disable)
                        {

                            spriteCoordinates = new Rect(i * (xml.cellWidth + 2 * xml.offset) + xml.offset, j * (xml.cellHeight + 2 * xml.offset) + xml.offset, xml.cellWidth, xml.cellHeight); // In pixels!
                            addSprite(imageTexture, spriteCoordinates, cell.name, xml.pivotPoint, xml.pixelsPerUnit, dict);
                            //Debug.Log(cell.name);
                        }
                    }
                    else
                    {
                        Debug.Log("Cell not found. has the Atlas builder version changed?");
                    }

                }
            }
        }

        /// <summary>
        /// Adds and creates sprite to dictionairy
        /// </summary>
        static void addSprite(Texture2D imageTexture, Rect spriteCoordinates, string spriteName, Vector2 pivotPoint, int pixelsPerUnit, Dictionary<string, Sprite> dict)
        {
            Sprite s = Sprite.Create(imageTexture, spriteCoordinates, pivotPoint, pixelsPerUnit);

            try
            {
                dict.Add(spriteName, s);
                count++;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}