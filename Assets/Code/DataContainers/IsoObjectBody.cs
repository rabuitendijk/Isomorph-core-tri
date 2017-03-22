
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
    protected IsoObjectBody(string name, List<Iso> coords, List<List<Sprite>> directions, Iso origin=null, Directions.dir direction = Directions.dir.N)
    {
        this.coords = clone(coords);
        this.directions = directions;
        this.origin = origin;
        this.direction = direction;
        isVisable = new List<bool>();

        foreach (Sprite s in directions[(int)direction])
        {
            isVisable.Add((s != null));
        }
        this.name = name;

        singular = false;
        if (coords.Count == 1)
            singular = true;

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

        return new IsoObjectBody(obj.name, coords, directions);
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    public virtual void destroy()
    {
        //empty
    }

}
