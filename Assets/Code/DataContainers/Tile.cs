
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// version aplha-2
/// 
/// Tile container class
/// 
/// Robin Apollo Butiendijk
/// Late February 2017
/// </summary>
public class Tile {

    GameObject Graphic;
    public GameObject graphic
    {
        get { return Graphic; }
        set {
            if (Graphic != null)
                GameObject.Destroy(Graphic);
            Graphic = value;
        }
    }

    public ProjIso coord { get; protected set; }

    public Sprite sprite { get { return isoObject.getSprite(index, Directions.add(isoObject.direction, Directions.currentDirection)); } }
    public IsoObject isoObject { get; protected set; }
    public int index { get; protected set; }

    /// <summary>
    /// Common constructor
    /// </summary>
    public Tile(ProjIso coord, int index, IsoObject isoObject = null)
    {
        this.coord = coord;
        this.index = index;
        this.isoObject = isoObject;

        //Create Tile with no assigned map so push to main Map
        onCreate(this);
    }

    /// <summary>
    /// Lazy constructor
    /// </summary>
    public Tile (Iso coord, int index, IsoObject isoObject = null) : this(new ProjIso(coord), index, isoObject){}

    /// <summary>
    /// Destroy this tile
    /// </summary>
    public void destroy()
    {
        onDestroy(this);
    }
    

    static Action<Tile> onCreate;

    public static void registerOnCreate(Action<Tile> funct){onCreate += funct;}
    public static void removeOnCreate(Action<Tile> funct){onCreate -= funct;}

    static Action<Tile> onDestroy;

    public static void registerOnDestroy(Action<Tile> funct) { onDestroy += funct; }
    public static void removeOnDestroy(Action<Tile> funct) { onDestroy -= funct; }
}
