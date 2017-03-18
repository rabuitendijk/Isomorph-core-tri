
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
    /// <summary>
    /// Sets common variable and registers callbacks
    /// </summary>
    public GraphicsControl()
    {
        main = this;

        tileFolder = new GameObject() { name = "TileFolder" }.transform;

        Tile.registerOnCreate(onTileCreate);
        Tile.registerOnDestroy(onTileDestroy);

    }

    protected abstract void onTileCreate(Tile t);
    protected abstract void onTileDestroy(Tile t);

    /// <summary>
    /// Destroy inhereting object, automatically called in base.destroy();
    /// </summary>
    protected abstract void destructor();

    /// <summary>
    /// Destroy the controller so that it can be overwritten in the runner
    /// </summary>
    public void destroy()
    {
        Tile.removeOnCreate(onTileCreate);
        Tile.removeOnDestroy(onTileDestroy);

        destructor();
        GameObject.Destroy(tileFolder.gameObject);
        tileFolder = null; ;
    }
}
