﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGraphicsControl : GraphicsControl
{
    Material mat;
    public TestingGraphicsControl() : base(new NoMouseHoverObject())
    {
        mat = new Material(Shader.Find("Iso/CheckEffect"));
    }

    public override void delayedConstruction()
    {
        return;
    }

    public override void rotate(Directions.dir direction)
    {
        return;
    }

    protected override void destructor()
    {
        return;
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

        GameObject ret = new GameObject() { name = t.isoObject.name + "(" + t.coord.x + ", " + t.coord.y + ", " + t.coord.z + ")" };
        ret.transform.position = t.coord.rotate(Directions.currentDirection, t.isoObject).position;
        ret.transform.parent = tileFolder;

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = t.sprite; //TODO
        sr.sharedMaterial = mat;
        sr.sortingLayerName = "lengthSort";
        sr.sortingOrder = t.coord.rotate(Directions.currentDirection, t.isoObject).depth;

        return ret;
    }
}