
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version aplha-1
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

    public Tile (Iso coord, GameObject graphic)
    {
        Coord = coord;
        Graphic = graphic;
    }
}
