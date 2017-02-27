
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// apha-1
/// Callback based graphics control system,
/// 
/// Robin Apollo Butiendijk
/// Late Febrary 2017
/// </summary>
public class GraphicsControl {

    SpriteHashLoader sprites;
    public static GraphicsControl main;
    Material mat, redMat;
    Transform tileFolder;

    public GraphicsControl(SpriteHashLoader sprites)
    {
        main = this;

        mat = new Material(Shader.Find("Sprites/Default"));
        redMat = new Material(Shader.Find("Sprites/Default"));
        redMat.color = new Color(1f, 0f, 0f);
        tileFolder = new GameObject() { name = "TileFolder" }.transform;

        this.sprites = sprites;

        Tile.registerOnCreate(onTileCreate);
    }

    void onTileCreate(Tile t)
    {
        t.graphic = newOb(t.coord, sprites.getSprite("stoneBlock"), mat);
    }

    GameObject newOb(Iso coord, Sprite sp, Material mat)
    {
        GameObject ret = new GameObject() { name = "isotile(" + coord.x + ", " + coord.y + ", " + coord.z + ")" };
        ret.transform.position = coord.toPos();
        ret.transform.parent = tileFolder;

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = sp;
        sr.material = mat;
        sr.sortingLayerName = "DepthSort";
        sr.sortingOrder = coord.depth;

        return ret;
    }
}
