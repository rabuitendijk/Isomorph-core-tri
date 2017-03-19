
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Alias_SourceLoader {

    public static int mipcount = 2;
    public static int resolution = 256;
    public static int count = 0;

    public static List<SplicingSource> entries;

	public static Dictionary<string, SplicingSource> loadSource(string folder, List<SplicingObject_XML> objects)
    {
        //
        entries = new List<SplicingSource>();
        Dictionary<string, SplicingSource> ret = new Dictionary<string, SplicingSource>();
        List<Texture2D> unitDefinitions = loadUnitDefinitions(folder);

        foreach (SplicingObject_XML ob in objects)
        {
            foreach (SplicingSource_XML s in ob.source)
            {
                if (!ret.ContainsKey(ob.name))
                    loadSource(folder + "/" + s.source, s.source, ob, ret, unitDefinitions);
            }
        }

        Debug.Log("Alias_SourceLoader "+count+", slices loaded.");
        return ret;
    }


    static List<Texture2D> loadUnitDefinitions(string folder)
    {
        List<Texture2D> ret = new List<Texture2D>();

        //Attempt load unit sprite
        if (!File.Exists(folder + "/UnitDefinition.png"))
        {
            Debug.Log("Alias_SourceLoader: UnitDefinition not found!!!, " + folder + "/UnitDefinition.png");
            return ret;
        }

        byte[] imageBytes = File.ReadAllBytes(folder + "/UnitDefinition.png");

        Texture2D unitTexture = new Texture2D(2, 2);   // Create some kind of dummy instance of Texture2D
        unitTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file
        Debug.Log("UnitDefinition loaded{width: " + unitTexture.width + ", height: " + unitTexture.height + "}");
        ret.Add(unitTexture);

        for (int i = 1; i < mipcount + 1; i++)
        {
            if (!File.Exists(folder + "/UnitDefinition.m" + i + ".png"))
            {
                Debug.Log("Alias_SourceLoader: UnitDefinition not found!!!, " + folder + "/UnitDefinition.m" + i + ".png");
                return ret;
            }

            imageBytes = File.ReadAllBytes(folder + "/UnitDefinition.m" + i + ".png");

            unitTexture = new Texture2D(2, 2);   // Create some kind of dummy instance of Texture2D
            unitTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file
            Debug.Log("UnitDefinition.m" + i + " loaded{width: " + unitTexture.width + ", height: " + unitTexture.height + "}");
            ret.Add(unitTexture);
        }


        return ret;

    }

    static void loadSource(string filename, string name, SplicingObject_XML ob, Dictionary<string, SplicingSource> dir, List<Texture2D> unitDefinitions)
    {

        if (!File.Exists(filename))
        {
            Debug.Log("File not found: "+filename);
            return;
        }

        SplicingSource ss = new SplicingSource(mipcount, ob.width, ob.depth, ob.trueHeight);

        ss.mips.Add(loadMip(0, filename, ob, unitDefinitions[0]));
        for (int i = 1; i < mipcount+1; i++)
        {
            ss.mips.Add(loadMip(i, filename.Substring(0, filename.Length - 4)+".m"+i+".png", ob, unitDefinitions[i]));
        }

        dir.Add(name, ss);
        entries.Add(ss);
        
    }

    static ProcessingImage[,,] loadMip(int mipLevel, string filename, SplicingObject_XML ob, Texture2D unitDefinition)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("Mip level not found: "+filename);
            return new ProcessingImage[ob.width,ob.depth,ob.trueHeight];
        }

        byte[] imageBytes = File.ReadAllBytes(filename);

        Texture2D imageTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);   // Create some kind of dummy instance of Texture2D
        imageTexture.LoadImage(imageBytes); // This will correctly resize the texture based on the image file

        if (mipLevel == 0)
            filename = filename.Substring(filename.LastIndexOf('/') + 1, filename.Length - filename.LastIndexOf('/') - 5);
        else
            filename = filename.Substring(filename.LastIndexOf('/') + 1, filename.Length - filename.LastIndexOf('/') - 8);

        return processTexture(imageTexture, ob, unitDefinition, (resolution >> mipLevel), filename);
    }


    /// <summary>
    /// Needs replacing
    /// xml size parameters <Net needed anyway>
    /// UnitTexture parameters
    /// </summary>
    static ProcessingImage[,,] processTexture(Texture2D tex, SplicingObject_XML ob, Texture2D unit, int res, string filename)
    {
        int expectedWidth = Mathf.RoundToInt(.5f * res * (ob.depth + ob.width));
        int expectedHeight = Mathf.RoundToInt(.25f * res * (ob.depth + ob.width + 2 * ob.height));

        int originX = Mathf.RoundToInt(.5f * res * (ob.width) - .5f * res);
        int originY = 0;

        if (!(expectedHeight == tex.height) || !(expectedWidth == tex.width))
        {
            Debug.Log("SpriteSlicer: source image dimentions [" + ob.name + "] do not match expected dimentions. source<" + tex.width + ", " + tex.height + "> expected<" + expectedWidth + ", " + expectedHeight + ">.");
            return new ProcessingImage[ob.width, ob.depth, ob.trueHeight];
        }

        ProcessingImage[,,] ret = new ProcessingImage[ob.width, ob.depth, ob.trueHeight];

        for (int i = 0; i < ob.width; i++)
        {
            for (int j = 0; j < ob.depth; j++)
            {
                for (int k = 0; k < ob.trueHeight; k++)
                {
                    if (isVoid(i, j, k, ob.trueHeight))
                        ret[i, j, k] = null;
                    else
                        ret[i, j, k] = getImage(tex, new Iso(i,j,k), originX, originY, filename, res, unit);
                }
            }
        }


        return ret;
    }

    /// <summary>
    /// Loads the desired fragment of the image
    /// </summary>
    static ProcessingImage getImage(Texture2D tex, Iso coord, int originX, int originY, string name, int size, Texture2D unit)
    {
        int x = originX + Mathf.RoundToInt(.5f * size * (-coord.x + coord.y));
        int y = originY + Mathf.RoundToInt(.25f * size * (coord.x + coord.y + coord.z));
        ProcessingImage temp = new ProcessingImage(size, size, name + "[" + coord.x + "_" + coord.y + "_" + coord.z + "]", new Iso(coord.x, coord.y, coord.z * 2));

        for (int j = 0; j < (size); j++)
        {
            for (int k = 0; k < (size); k++)
            {

                if (unit.GetPixel(j, k).a > .5)
                {
                    //Copy pixel
                    temp.set(j, k, tex.GetPixel(x + j, y + k));
                }
                else
                {
                    //Ignore pixel
                    temp.set(j, k, new Color(0f, 0f, 0f, 0f));
                }


            }
        }
        count++;
        return temp;
    }


    /// <summary>
    /// Second true height coord always added in voids
    /// </summary>
    static bool isVoid(int i, int j, int k, int height)
    {


        if (k % 2 == 1) //Offest height
            return true;  //Always add in offset
        if (!(k == height - 2) && !(i == 0) && !(j == 0)) // If now sides or top, ?dont know why -2?
            return true;  //Add

        return false;
    }
}
