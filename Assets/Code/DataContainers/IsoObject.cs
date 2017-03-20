
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// version alpha-1
/// 
/// Container containing data for the construction of a 1 or multiple tile containing object.
/// 
/// Robin Butiendijk
/// Early Martch 2017
/// </summary>
public class IsoObject : IsoObjectBody
{
    public List<Tile> tiles { get; protected set; }


    /// <summary>
    /// Copy constructor.
    /// Offsets coordinats by origin.
    /// Shares sprite stringlist with prototype
    /// </summary>
    public IsoObject(string prototype, Iso origin, Directions.dir direction=Directions.dir.N) : this(Alias_Loader.main.getObject(prototype), origin, direction) { }

    private IsoObject(IsoObjectBody prototype, Iso origin, Directions.dir direction) : base(prototype.name, prototype.coords, prototype.directions, origin, direction)
    {

        foreach (Iso i in coords)
        {
            i.add(origin);
        }

        safeTileConstruction();
    }


    /// <summary>
    /// Checks for conflics and then build tiles using object data
    /// </summary>
    bool safeTileConstruction()
    {
        if (checkCoordsOccupied())
        {
            Debug.Log("IsoObject["+name+", "+origin.ToString()+"]: one or more tiles blocked during construction.");
            return false;
        }

        tiles = new List<Tile>();


        for (int i=0; i<coords.Count; i++)
        {
            tiles.Add(new Tile(coords[i], directions[(int)direction][i], this));
        }
        //Debug.Log("Construction did run.");

        if (onCreate != null)
            onCreate(this);

        return true;
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    public override void destroy()
    {
        if (onDestroy != null)
            onDestroy(this);

        foreach (Tile t in tiles)
        {
            t.destroy();
        }
    }

    static Action<IsoObject> onCreate;
    static Action<IsoObject> onDestroy;
    public static void registerOnCreate(Action<IsoObject> funct) { onCreate += funct; }
    public static void removeOnCreate(Action<IsoObject> funct) { onCreate -= funct; }
    public static void registerOnDestroy(Action<IsoObject> funct) { onDestroy += funct; }
    public static void removeOnDestroy(Action<IsoObject> funct) { onDestroy -= funct; }


}
