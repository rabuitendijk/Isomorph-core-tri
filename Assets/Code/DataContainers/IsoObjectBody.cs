
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shard body of IsoObejcts
/// </summary>
public class IsoObjectBody {

    public bool singular { get; protected set; }
    public List<Iso> coords { get; protected set; }
    public List<List<Sprite>> directions { get; protected set; } //N, E, S, W
    public List<bool> isVisable { get; protected set; }
    public Directions.dir direction { get; protected set; }
    public string name { get; protected set; }
    public Iso origin { get; protected set; }
    public bool is_light { get; protected set; }
    public int light_radius { get; protected set; }

    public int width { get; protected set; }
    public int height { get; protected set; }
    public int length { get; protected set; }

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

    //Proteced constructor
    protected IsoObjectBody(IsoObjectBody prototype, Iso origin=null, Directions.dir direction = Directions.dir.N)
    {
        coords = clone(prototype.coords);
        directions = prototype.directions;
        this.origin = origin;
        this.direction = direction;
        width = prototype.width;
        length = prototype.length;
        height = prototype.height;
        is_light = prototype.is_light;
        light_radius = prototype.light_radius;
        isVisable = new List<bool>();

        foreach (Sprite s in directions[(int)direction])
        {
            isVisable.Add((s != null));
        }
        name = prototype.name;

        singular = false;
        if (coords.Count == 1)
            singular = true;

    }

    //Pivate constructor
    private IsoObjectBody(string name, List<Iso> coords, List<List<Sprite>> directions, int width, int length, int height, bool is_light, int light_radius)
    {
        this.coords = clone(coords);
        this.directions = directions;
        this.width = width;
        this.length = length;
        this.height = height;
        this.is_light = is_light;
        this.light_radius = light_radius;
        isVisable = new List<bool>();

        foreach (Sprite s in directions[(int)direction])
        {
            isVisable.Add((s != null));
        }
        this.name = name;

        singular = false;
        if (coords.Count == 1)
            singular = true;

        //Debug.Log(ToString());
    }

    /// <summary>
    /// Clones a coordinate list to avoid changing original coordinates
    /// </summary>
    protected List<Iso> clone(List<Iso> list)
    {
        List<Iso> ret = new List<Iso>();

        foreach (Iso i in list)
        {
            ret.Add(new Iso(i.x, i.y, i.z));
        }

        return ret;
    }

    /// <summary>
    /// Costructor to derrive prototype from XML
    /// </summary>
    public static IsoObjectBody prototype(LinkerObject_XML obj)
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
                foreach (XMLCoord x in obj.directions[i].coords)
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

        return new IsoObjectBody(obj.name, coords, directions, obj.width, obj.length, obj.height, obj.is_light, obj.light_radius);
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
