
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

    public GraphicsControl()
    {
        main = this;

        mat = new Material(Shader.Find("Sprites/Default"));
        redMat = new Material(Shader.Find("Sprites/Default"));
        redMat.color = new Color(1f, 0f, 0f);
        tileFolder = new GameObject() { name = "TileFolder" }.transform;


        Tile.registerOnCreate(onTileCreate);

        testob(new Iso(0,0,1));
    }

    void onTileCreate(Tile t)
    {
        t.graphic = newOb(t.coord, "unitBlock", mat);
    }

    GameObject newOb(Iso coord, string name, Material mat)
    {
        GameObject ret = new GameObject() { name = name+"(" + coord.x + ", " + coord.y + ", " + coord.z + ")" };
        ret.transform.position = coord.toPos();
        ret.transform.parent = tileFolder;

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = StreamingSpriteLoader.Main.getSprite(name);
        sr.material = mat;
        sr.sortingLayerName = "DepthSort";
        sr.sortingOrder = coord.depth;

        return ret;
    }

    void testob(Iso offset)
    {
        Iso t;

        t = new Iso(0, 0, 0); t.add(offset);
        newOb(t, "bigBlock[" + 0 + "_" + 0 + "_" + 0 + "]", mat);
        t = new Iso(1, 0, 0); t.add(offset);
        newOb(t, "bigBlock[" + 1 + "_" + 0 + "_" + 0 + "]", mat);
        t = new Iso(0, 0, 1); t.add(offset);
        newOb(t, "bigBlock[" + 0 + "_" + 0 + "_" + 1 + "]", mat);
        t = new Iso(1, 0, 1); t.add(offset);
        newOb(t, "bigBlock[" + 1 + "_" + 0 + "_" + 1 + "]", mat);
        t = new Iso(0, 1, 0); t.add(offset);
        newOb(t, "bigBlock[" + 0 + "_" + 1 + "_" + 0 + "]", mat);
        t = new Iso(0, 1, 1); t.add(offset);
        newOb(t, "bigBlock[" + 0 + "_" + 1 + "_" + 1 + "]", mat);
        t = new Iso(1, 1, 1); t.add(offset);
        newOb(t, "bigBlock[" + 1 + "_" + 1 + "_" + 1 + "]", mat);
    }
}
