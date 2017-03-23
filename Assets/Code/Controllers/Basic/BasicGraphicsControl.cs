﻿
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// version apha-2
/// Callback based graphics control system,
/// 
/// Robin Apollo Butiendijk
/// Late Febrary 2017
/// </summary>
public class BasicGraphicsControl : GraphicsControl {


    Material mat, redMat;

    /// <summary>
    /// Constructor that initialises references and materials,
    /// </summary>
    public BasicGraphicsControl() : base(new BasicMouseHoverObject())
    {

        mat = new Material(Shader.Find("Sprites/Default"));
    }

    protected override void onTileCreate(Tile t)
    {
        t.graphic = newOb(t, mat);
    }

    protected override void onTileDestroy(Tile t)
    {
        t.graphic = null;
    }

    /// <summary>
    /// Tile based sprite gameobject creator.
    /// </summary>
    GameObject newOb(Tile t, Material mat)
    {
        if (t.sprite == null)
            return null;

        GameObject ret = new GameObject() { name = t.isoObject.name+ "(" + t.coord.x + ", " + t.coord.y + ", " + t.coord.z + ")" };
        ret.transform.position = t.coord.position;
        ret.transform.parent = tileFolder;

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = t.sprite;
        sr.material = mat;
        sr.sortingLayerName = "lengthSort";
        sr.sortingOrder = t.coord.depth;

        return ret;
    }

    /// <summary>
    /// Common sprtie gameobject creator.
    /// </summary>
    GameObject newOb(string objName, ProjIso i, Sprite sprite, Material mat)
    {
        if (sprite == null)
            return null;

        GameObject ret = new GameObject() { name = objName + "(" + i.x + ", " + i.y + ", " + i.z + ")" };
        ret.transform.position = i.position;
        ret.transform.parent = tileFolder;

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = sprite;
        sr.material = mat;
        sr.sortingLayerName = "lengthSort";
        sr.sortingOrder = i.depth;

        return ret;
    }

    protected override void destructor()
    {
        return;
    }

    public override void rotate(Directions.dir direction)
    {
        throw new NotImplementedException();
    }
}

