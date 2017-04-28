
using LogicControl = Logic_C.LogicControl;

using System.Collections.Generic;
using UnityEngine;
using AssetHandeling_AtlasLoader;

/// <summary>
/// Shared body of IsoObejcts, contains information from the xml object files
/// </summary>
public class IsoObjectBody {


    public List<Iso> coords { get; protected set; }
    public Directions.dir direction { get; protected set; }
    public Iso origin { get; protected set; }

    public bool singular { get { return data.singular; } }
    public List<List<Sprite>> directions { get { return data.directions; } } //N, E, S, W
    public List<bool> isVisable { get { return data.isVisable; } }
    public string name { get { return data.name; } }
    public bool is_light { get { return data.is_light; } }
    public int light_radius { get { return data.light_radius; } }

    public int width { get { return data.width; } }
    public int height { get { return data.height; } }
    public int length { get { return data.length; } }


    IsoObjectData data;
    /// <summary>
    /// Check is coords in map are occupied
    /// </summary>
    public bool checkCoordsOccupied()
    {
        foreach (Iso i in coords)
        {
            //Debug.Log("..., "+i.ToString());
            if (!LogicControl.main.inGrid(i) || LogicControl.main.exists(i))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Proteced constructor, called when an ingame IsoObject is created. Copies data.
    /// </summary>
    protected IsoObjectBody(IsoObjectBody prototype, Iso origin=null, Directions.dir direction = Directions.dir.N)
    {
        coords = prototype.data.coords;
        this.origin = origin;
        this.direction = direction;
        this.data = prototype.data;
    }

    /// <summary>
    /// Pivate constructor, Called when a new prototype is created.
    /// </summary>
    private IsoObjectBody(IsoObjectData data)
    {
        this.data = data;

        origin = new Iso(0, 0, 0);
        direction = Directions.dir.N;
        coords = data.coords;
        //Debug.Log(ToString());
    }

    /// <summary>
    /// Costructor to derrive prototype from XML
    /// </summary>
    public static IsoObjectBody prototype(XMLO_AL_IsoObejct obj)
    {
        List<Iso> coords = new List<Iso>();
        List<List<Sprite>> directions = new List<List<Sprite>>();
        for (int i = 0; i < 4; i++)
        {
            directions.Add(null);
        }

        bool setcoords = false;
        //Find nonlinked directions and load them 
        for (int i = 0; i < 4; i++)
        {
            if (!obj.directions[i].linked)
            {
                directions[i] = new List<Sprite>();
                foreach (XMLO_AL_Coord x in obj.directions[i].coords)
                {
                    if (!setcoords)
                        coords.Add(new Iso(x.x, x.y, x.z));


                    if (x.spriteName == "VOID")
                        directions[i].Add(null);
                    else
                        directions[i].Add(Atlas_Loader.main.getSprite(x.spriteName));
                }
                setcoords = true;
            }
        }

        //Link remaining directions
        for (int i = 0; i < 4; i++)
        {
            if (directions[i] == null)
                directions[i] = directions[(int)Directions.getDir(obj.directions[i].source)];
        }


        return new IsoObjectBody(new IsoObjectData(obj.name, coords, directions, obj.is_light, obj.light_radius, obj.width, obj.height, obj.length));
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    public virtual void destroy()
    {
        //empty
    }

    public override string ToString()
    {
        return "IsoObjectBody<" + name + ", [" + width + ", " + length + ", " + height + "]>";
    }

}
