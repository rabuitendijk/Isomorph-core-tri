
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
    protected int Width, Depth, Height;
    public int width { get { return Width; } }
    public int depth { get { return Depth; } }
    public int height { get { return Height; } }


    public LogicControl()
    {
        main = this;

        Tile.registerOnCreate(onTileCreate);
        Tile.registerOnDestroy(onTileDestroy);
    }

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
