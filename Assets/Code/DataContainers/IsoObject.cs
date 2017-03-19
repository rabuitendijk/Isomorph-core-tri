
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// version alpha-1
/// 
/// Container containing data for the construction of a 1 or multiple tile containing object.
/// 
/// Robin Butiendijk
/// Early Martch 2017
/// </summary>
public class IsoObject
{

    public bool singular { get; protected set; }
    public List<Iso> coords { get; protected set; }
    public List<bool> isVisable { get; protected set; }
    public List<Sprite> sprites { get; protected set; }
    public string name { get; protected set; }
    public List<Tile> tiles { get; protected set; }

    public Iso origin { get; protected set; }

    /// <summary>
    /// Costructor to derrive prototype from XML
    /// </summary>
    public static IsoObject prototype(LinkerObject_XML obj)
    {
        List<Iso> coords = new List<Iso>();
        List<Sprite> sprites = new List<Sprite>();

        foreach (XMLCoord x in obj.directions[0].coords)
        {
            coords.Add(new Iso(x.x, x.y, x.z));
            if (x.spriteName == "VOID")
                sprites.Add(null);
            else
                sprites.Add(Alias_Loader.main.getSprite(x.spriteName));
        }

        return new IsoObject(obj.name, coords, sprites);
    }

    /// <summary>
    /// Copy constructor.
    /// Offsets coordinats by origin.
    /// Shares sprite stringlist with prototype
    /// </summary>
    public IsoObject(string prototype, Iso origin) : this(Alias_Loader.main.getObject(prototype), origin) { }

    private IsoObject(IsoObject prototype, Iso origin):this(prototype.name, prototype.coords, prototype.sprites)
    {
        this.origin = origin;

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
            tiles.Add(new Tile(coords[i], sprites[i], this));
        }
        //Debug.Log("Construction did run.");

        if (onCreate != null)
            onCreate(this);

        return true;
    }

    /// <summary>
    /// Check is coords in map are occupied
    /// </summary>
    public bool checkCoordsOccupied()
    {
        foreach(Iso i in coords)
        {
            //Debug.Log("..., "+i.ToString());
            if (!LogicControl.main.inGrid(i)    ||  LogicControl.main.exists(i))
                return true;
        }

        return false;
    }

    //Proteced constructor
    private IsoObject(string name, List<Iso> coords, List<Sprite> sprites)
    {
        this.coords = clone(coords);
        this.sprites = sprites;
        isVisable = new List<bool>();

        foreach (Sprite s in sprites)
        {
            isVisable.Add((s != null));
        }
        this.name = name;

        singular = false;
        if (coords.Count == 1)
            singular = true;

    }

    List<Iso> clone(List<Iso> list)
    {
        List<Iso> ret = new List<Iso>();

        foreach(Iso i in list)
        {
            ret.Add(new Iso(i.x, i.y, i.z));
        }

        return ret;
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    public void destroy()
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
