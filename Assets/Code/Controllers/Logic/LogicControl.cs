
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version aplha-1
/// Accesor functions for logic.
/// 
/// Robin Apollo Buitendijk
/// Early March 2017
/// </summary>
public abstract class LogicControl {

    public static LogicControl main;
    public int width { get; protected set; }
    public int length { get; protected set; }
    public int height { get; protected set; }
    public string filename { get; protected set; }

    public LogicControl()
    {
        main = this;
        filename = "";

        Tile.registerOnCreate(onTileCreate);
        Tile.registerOnDestroy(onTileDestroy);
    }

    public abstract void delayedConstruction();

    protected abstract void onTileCreate(Tile t);
    protected abstract void onTileDestroy(Tile t);

    /// <summary>
    /// Set a Tile in the grid corresponding with its coord
    /// </summary>
    public abstract void set(Tile t);

    /// <summary>
    /// Unprotected get
    /// </summary>
    public abstract Tile get(Iso i);


    /// <summary>
    /// Check exists
    /// </summary>
    public abstract bool exists(Iso i);

    /// <summary>
    /// Checks if an coord is inside the Map
    /// </summary>
    public abstract bool inGrid(Iso i);

    /// <summary>
    /// Selectes a build is [not dinamic] level creator based on name.
    /// </summary>
    public abstract void makeLevel(string name);

    protected abstract void destructor();

    public void destroy()
    {
        destructor();

        Tile.removeOnCreate(onTileCreate);
        Tile.removeOnDestroy(onTileDestroy);

    }
}
