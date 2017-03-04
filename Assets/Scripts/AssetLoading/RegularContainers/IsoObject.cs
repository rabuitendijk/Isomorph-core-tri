
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// alpha-1
/// 
/// Container containing data for the construction of a 1 or multiple tile containing object.
/// 
/// Robin Butiendijk
/// Early Martch 2017
/// </summary>
public class IsoObject {

    public bool singular { get; protected set; }
    public List<Iso> coords { get; protected set; }
    public List<bool> isVisable { get; protected set; }
    public List<string> spriteName { get; protected set; }
    public string name { get; protected set; }

    public Iso origin { get; protected set; }

    /// <summary>
    /// Costructor to derrive prototype from XML
    /// </summary>
    public static IsoObject prototype(XMLObject obj)
    {
        List<Iso> coords = new List<Iso>();
        List<string> spriteName = new List<string>();

        foreach (XMLCoord x in obj.coords)
        {
            coords.Add(new Iso(x.x, x.y, x.z));
            spriteName.Add(x.spriteName);
        }

        return new IsoObject(obj.name, coords, spriteName);
    }

    /// <summary>
    /// Copy constructor.
    /// Offsets coordinats by origin.
    /// Shares sprite stringlist with prototype
    /// </summary>
    public IsoObject(IsoObject prototype, Iso origin) : this(prototype.name, prototype.coords, prototype.spriteName)
    {
        this.origin = origin;

        foreach(Iso i in coords)
        {
            i.add(origin);
        }

        safeTileConstruction();
    }

    /// <summary>
    /// Return tiles held in object
    /// </summary>
    public List<Tile> getTiles()
    {
        List<Tile> ret = new List<Tile>();

        foreach(Iso i in coords)
        {
            ret.Add(Map.main.get(i));
        }

        return ret;
    }

    /// <summary>
    /// Checks for conflics and then build tiles using object data
    /// </summary>
    bool safeTileConstruction()
    {
        if (checkCoordsOccupied())
            return false;

        for(int i=0; i<coords.Count; i++)
        {
            new Tile(coords[i], spriteName[i], this);
        }

        return true;
    }

    /// <summary>
    /// Check is coords in map are occupied
    /// </summary>
    public bool checkCoordsOccupied()
    {
        foreach(Iso i in coords)
        {
            if (Map.main.exists(i))
                return true;
        }

        return false;
    }

    //Proteced constructor
    private IsoObject(string name, List<Iso> coords, List<string> spriteName)
    {
        this.coords = new List<Iso>(coords);
        this.spriteName = spriteName;
        isVisable = new List<bool>();

        foreach (string s in spriteName)
        {
            isVisable.Add((s != "VOID"));
        }
        this.name = name;

        singular = false;
        if (coords.Count == 1)
            singular = true;

    }
}
