 
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version aplha-1
/// 
/// Grphics control for level editor.
/// 
/// Robin Apollo Butiendijk
/// Early March 2017
/// </summary>
public class EditorGraphicsControl : GraphicsControl {

    Material mat;
    List<Transform> folders;
    Dictionary<ulong, IsoObject> projectionTable = new Dictionary<ulong, IsoObject>();
    EditorComonentGLD gld;
    EditorComponentGhost ghost;

    /// <summary>
    /// Constructor that initialises references and materials,
    /// </summary>
    public EditorGraphicsControl() : base(new IsoObjectGhost("Selector", new Iso(0,0,0)))
    {

        mat = new Material(Shader.Find("Iso/CheckEffect"));

        //Create Folder per height
        folders = new List<Transform>();
        GameObject temp;
        for (int i = 0; i < LogicControl.main.height; i++)
        {
            temp = new GameObject() { name = "Layer[" + i + "]" };
            temp.transform.SetParent(tileFolder);
            folders.Add(temp.transform);
        }

        gld = new EditorComonentGLD(folders);
        ghost = new EditorComponentGhost(this, tileFolder);

        IsoObject.registerOnCreate(onIsoObjectCreate);
        IsoObject.registerOnDestroy(onIsoObjectDestroy);
    }

    public override void delayedConstruction()
    {
        gld.delayedConstruction();
    }


    public override void rotate(Directions.dir dir)
    {
        Directions.currentDirection = dir;
        Tile t; ProjIso p;

        foreach (KeyValuePair<ulong, IsoObject> entry in projectionTable)
        {
            for (int i = 0; i < entry.Value.coords.Count; i++)
            {
                //entry.Value.setDirection(dir);
                if (entry.Value.getSprite(i) != null)
                {
                    t = LogicControl.main.get(entry.Value.coords[i]);

                    p = t.coord.rotate(dir, entry.Value);
                    t.graphic.transform.position = p.position;
                    t.graphic.GetComponent<SpriteRenderer>().sortingOrder = p.depth;
                    t.graphic.GetComponent<SpriteRenderer>().sprite = entry.Value.getSprite(i, Directions.subtract(entry.Value.direction, dir));
                }
            }

        }
    }

    protected override void onTileCreate(Tile t)
    {
        t.graphic = newOb(t, mat);
    }

    protected override void onTileDestroy(Tile t)
    {
        t.graphic = null;
    }

    void onIsoObjectCreate(IsoObject i)
    {
        projectionTable.Add(i.id, i);
    }

    void onIsoObjectDestroy(IsoObject i)
    {
        projectionTable.Remove(i.id);
    }


    /// <summary>
    /// Tile based sprite gameobject creator.
    /// </summary>
    GameObject newOb(Tile t, Material mat)
    {
        if (t.sprite == null)
            return null;

        GameObject ret = new GameObject() { name = t.isoObject.name + "(" + t.coord.x + ", " + t.coord.y + ", " + t.coord.z + ")" };
        ret.transform.position = t.coord.position;
        ret.transform.parent = folders[t.coord.z];

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = t.sprite;
        sr.sharedMaterial = mat;
        sr.sortingLayerName = "lengthSort";
        sr.sortingOrder = t.coord.depth;

        return ret;
    }

    protected override void destructor()
    {
        gld.destroy();
        ghost.destroy();

        IsoObject.removeOnCreate(onIsoObjectCreate);
        IsoObject.removeOnDestroy(onIsoObjectDestroy);

        return;
    }
}
