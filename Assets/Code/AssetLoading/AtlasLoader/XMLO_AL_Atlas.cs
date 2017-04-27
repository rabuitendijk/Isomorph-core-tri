
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Vector2 = UnityEngine.Vector2;
using Debug = UnityEngine.Debug;

namespace AssetHandeling_AtlasLoader
{
    /// <summary>
    /// version aplha-1
    ///	Stores data from read sprite .xml file
    /// Contains names of all cells contained in this atlas
    ///
    /// Robin Apollo Buitendijk
    /// Late February 2017
    /// </summary>
    public class XMLO_AL_Atlas : IXmlSerializable
    {
        public int pixelsPerUnit { get; protected set; }
        public int miplevels { get; protected set; }
        public Vector2 pivotPoint;
        public bool multiSprite { get; protected set; }

        public int columns { get; protected set; }
        public int rows { get; protected set; }
        public int cellHeight { get; protected set; }
        public int cellWidth { get; protected set; }

        public int offset { get; protected set; }
        List<XMLO_AL_Sprite> cells;


        /// <summary>
        /// Default constructor
        /// </summary>
        public XMLO_AL_Atlas()
        {
            pixelsPerUnit = 32;
            pivotPoint = new Vector2(0.5f, 0.5f);
            multiSprite = false;
            columns = 1;
            rows = 1;
            cellHeight = 32;
            cellWidth = 32;
            offset = 0;
            miplevels = -1;

            cells = new List<XMLO_AL_Sprite>();
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

                XmlSerializer serializer = new XmlSerializer(typeof(XMLO_AL_Sprite));
                reader.ReadToFollowing("cells");

                if (reader.ReadToDescendant("XMLO_AL_Sprite"))
                {
                    do
                    {
                        //Debug.Log("RAN3: "+reader.Name);
                        cells.Add((XMLO_AL_Sprite)serializer.Deserialize(reader));
                    } while (reader.ReadToNextSibling("XMLO_AL_Sprite"));
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

                    case "offset":
                        offset = reader.ReadContentAsInt();
                        break;

                    case "miplevels":
                        miplevels = reader.ReadContentAsInt();
                        break;


                    default:
                        Debug.Log("Warning switch: " + reader.Name);
                        break;
                }
            }
        }

        public XMLO_AL_Sprite getCell(int x, int y)
        {
            foreach (XMLO_AL_Sprite c in cells)
            {
                if (c.x == x && c.y == y)
                    return c;
            }
            return null;
        }
    }
}