
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

    Iso Coord;
    public Iso coord { get { return Coord; } }

    public Sprite sprite { get; protected set; }
    public IsoObject isoObject { get; protected set; }

    public Tile (Iso coord, Sprite sprite = null, IsoObject isoObject = null)
    {
        Coord = coord;
        this.sprite = sprite;
        this.isoObject = isoObject;

        //Create Tile with no assigned map so push to main Map
        onCreate(this);
    }


    

    static Action<Tile> onCreate;

    /// <summary>
    /// On create call
    /// </summary>
    public static void registerOnCreate(Action<Tile> funct)
    {
        onCreate += funct;
    }
}
