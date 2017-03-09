
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

    public static GraphicsControl main;
    Material mat, redMat;
    Transform tileFolder;
    public GameObject selector;

    public GraphicsControl()
    {
        main = this;

        mat = new Material(Shader.Find("Sprites/Default"));
        redMat = new Material(Shader.Find("Sprites/Default"));
        redMat.color = new Color(1f, 0f, 0f);
        tileFolder = new GameObject() { name = "TileFolder" }.transform;


        Tile.registerOnCreate(onTileCreate);
        selector = newOb("Selector", new Iso(0, 0, 0, 1), AliasXMLLoader.main.getSprite("selector"), mat);
        selector.SetActive(false);
        //testob(new Iso(0,0,1));
    }

    void onTileCreate(Tile t)
    {
        t.graphic = newOb(t, mat);
    }

    GameObject newOb(Tile t, Material mat)
    {
        if (t.sprite == null)
            return null;

        GameObject ret = new GameObject() { name = t.isoObject.name+ "(" + t.coord.x + ", " + t.coord.y + ", " + t.coord.z + ")" };
        ret.transform.position = t.coord.toPos();
        ret.transform.parent = tileFolder;

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = t.sprite;
        sr.material = mat;
        sr.sortingLayerName = "DepthSort";
        sr.sortingOrder = t.coord.depth;

        return ret;
    }

    GameObject newOb(string objName, Iso i, Sprite sprite, Material mat)
    {
        if (sprite == null)
            return null;

        GameObject ret = new GameObject() { name = objName + "(" + i.x + ", " + i.y + ", " + i.z + ")" };
        ret.transform.position = i.toPos();
        ret.transform.parent = tileFolder;

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = sprite;
        sr.material = mat;
        sr.sortingLayerName = "DepthSort";
        sr.sortingOrder = i.depth;

        return ret;
    }

}
