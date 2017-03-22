
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
    ulong proj_id = 123456789;

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

    public Sprite sprite { get; protected set; }
    public IsoObject isoObject { get; protected set; }


    public Tile(ProjIso coord, Sprite sprite = null, IsoObject isoObject = null)
    {
        this.coord = coord;
        this.sprite = sprite;
        this.isoObject = isoObject;

        if (sprite != null)
            proj_id = GraphicsControl.proj_id++;

        //Create Tile with no assigned map so push to main Map
        onCreate(this);
    }

    public Tile (Iso coord, Sprite sprite = null, IsoObject isoObject = null) : this(new ProjIso(coord), sprite, isoObject){}

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
