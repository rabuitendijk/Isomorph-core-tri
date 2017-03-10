
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version aplha-1
/// 
/// Accessor functions for Grapics.
/// 
/// Robin Apollo Buitendijk
/// Early March 2017
/// </summary>
public abstract class GraphicsControl{

    public static GraphicsControl main;
    public GameObject selector;

    protected Transform tileFolder;

    public GraphicsControl()
    {
        main = this;

        tileFolder = new GameObject() { name = "TileFolder" }.transform;

        Tile.registerOnCreate(onTileCreate);

    }

    protected abstract void onTileCreate(Tile t);
	
}
