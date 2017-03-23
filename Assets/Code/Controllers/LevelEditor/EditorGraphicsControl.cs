﻿ 
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

    Material mat, ghostMat;
    List<Transform> folders = new List<Transform>();
    Dictionary<ulong, Tile> projectionTable = new Dictionary<ulong, Tile>();
    GLDrawLoop gld;
    GLInstruction gliGrid, gliStruct;
    int height = 0;

    /// <summary>
    /// Constructor that initialises references and materials,
    /// </summary>
    public EditorGraphicsControl() : base(new IsoObjectGhost("Selector", new Iso(0,0,0)))
    {

        mat = new Material(Shader.Find("Iso/CheckEffect"));
        ghostMat = new Material(Shader.Find("Sprites/Default"));
        ghostMat.color = new Color(1f, 1f, 1f, .6f);

        //Create Folder per height
        GameObject temp;
        for (int i = 0; i < LogicControl.main.height; i++)
        {
            temp = new GameObject() { name = "Layer[" + i + "]" };
            temp.transform.SetParent(tileFolder);
            folders.Add(temp.transform);
        }

        setGLD();

        EditorComponentMouseLayer.registerEnableLayer(enableLayer);
        EditorComponentMouseLayer.registerMoveLayer(moveLayer);

        IsoObjectGhost.registerOnCreate(onGhostCreate);
        IsoObjectGhost.registerOnDestroy(onGhostDestroy);
        IsoObjectGhost.registerOnRotate(onGhostRotate);
        IsoObjectGhost.registerOnTranslate(onGhostTranlate);
        EditorComponentUI.registerOnChangeSelected(onHoverChangeSelected);
    }


    /// <summary>
    /// Enables the layers upto height
    /// </summary>
    protected void enableLayer(bool isOn, int height)
    {
        this.height = height;
        if (gliStruct != null)
            gld.remove(gliStruct);
        if (gliGrid != null)
            gld.remove(gliGrid);

        if (!isOn)  //Show everything
        {
            enableLayers();
            return;
        }


        //Add additional line gui
        setGLIGrid(height);
        setGLIStruct(height);
        gld.add(gliGrid);
        gld.add(gliStruct);

        //Hide layers
        enableLayers(height);
    }

    /// <summary>
    /// Relativly moves layer up or down
    /// </summary>
    protected void moveLayer(bool isUp)
    {
        enableLayers(isUp);

        if (gliStruct != null)
            gld.remove(gliStruct);
        if (gliGrid != null)
            gld.remove(gliGrid);

        //Add additional line gui
        setGLIGrid(height);
        setGLIStruct(height);
        gld.add(gliGrid);
        gld.add(gliStruct);

    }

    /// <summary>
    /// Creates the GLD
    /// </summary>
    void setGLD()
    {

        GameObject temp = new GameObject() { name = "GLD" };
        gld = temp.AddComponent<GLDrawLoop>();
        List<Vector3> coords = new List<Vector3>();
        coords.Add(new ProjIso(0,0,0).position);
        coords.Add(new ProjIso(0, LogicControl.main.length, 0).position);
        coords.Add(new ProjIso(LogicControl.main.width, LogicControl.main.length, 0).position);
        coords.Add(new ProjIso(LogicControl.main.width, 0, 0).position);
        gld.add(new GLInstruction(coords, Color.red));
    }

    /// <summary>
    /// Creates some GL gui information
    /// </summary>
    void setGLIStruct(int h)
    {
        List<Vector3> coords = new List<Vector3>();
        coords.Add(new ProjIso(0, 0, h).position);
        coords.Add(new ProjIso(0, LogicControl.main.length, h).position);
        coords.Add(new ProjIso(LogicControl.main.width, LogicControl.main.length, h).position);
        coords.Add(new ProjIso(LogicControl.main.width, 0, h).position);
        gliStruct = new GLInstruction(coords, Color.blue);
    }

    /// <summary>
    /// Enable all layers
    /// </summary>
    void enableLayers()
    {
        foreach (Transform t in folders)
            t.gameObject.SetActive(true);
    }

    /// <summary>
    /// Enable all layers upto height
    /// </summary>
    void enableLayers(int height)
    {
        for (int i = 0; i == height; i++)
        {
            folders[i].gameObject.SetActive(true);
        }

        for (int i = height+1; i < folders.Count; i++)
        {
            folders[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Relativly changes the number ovf visible layers
    /// </summary>
    void enableLayers(bool isUp)
    {
        if (isUp)
        {
            folders[++height].gameObject.SetActive(true);
            return;
        }
        folders[height--].gameObject.SetActive(false);
        return;

    }

    void setGLIGrid(int h)
    {
        List<Vector3> coords = new List<Vector3>();

        for (int i = 1; i < LogicControl.main.width; i++)
        {
            coords.Add(new ProjIso(i, 0, h).position);
            coords.Add(new ProjIso(i, LogicControl.main.length, h).position);
        }

        for (int j = 0; j < LogicControl.main.length; j++)
        {
            coords.Add(new ProjIso(0, j, h).position);
            coords.Add(new ProjIso(LogicControl.main.width, j, h).position);
        }

        gliGrid = new GLInstruction(coords, Color.gray, false, false);
    }

    public override void rotate(Directions.dir dir)
    {
        Directions.currentDirection = dir;
        ProjIso i;

        foreach (KeyValuePair<ulong, Tile> entry in projectionTable)
        {
            i  = entry.Value.coord.rotate(dir);
            entry.Value.graphic.transform.position = i.position;
            entry.Value.graphic.GetComponent<SpriteRenderer>().sortingOrder = i.depth;
        }
    }

    protected override void onTileCreate(Tile t)
    {
        t.graphic = newOb(t, mat);
        if (t.sprite != null)
            projectionTable.Add(t.proj_id, t);
    }

    protected override void onTileDestroy(Tile t)
    {
        t.graphic = null;
        if (t.sprite != null)
            projectionTable.Remove(t.proj_id);
    }


    protected void onGhostCreate(IsoObjectGhost g)
    {
        for (int i = 0; i < g.coords.Count; i++)
        {
            if (g.isVisable[i])
                g.graphic.Add(newOb(g.name, g.proj_coords[i], g.directions[(int)g.direction][i], ghostMat));
        }
    }
    protected void onGhostDestroy(IsoObjectGhost g)
    {
        foreach(GameObject ob in g.graphic)
        {
            GameObject.Destroy(ob);
        }
        g.graphic.Clear();
    }
    protected void onGhostRotate(IsoObjectGhost g)
    {
        if (g.graphic.Count == 0)
            return;

        int j = 0;

        for (int i = 0; i < g.coords.Count; i++)
        {
            if (g.isVisable[i])
            {
                g.graphic[j].GetComponent<SpriteRenderer>().sprite = g.directions[(int)g.direction][i];
                j++;
            }
        }
    }
    protected void onGhostTranlate(IsoObjectGhost g)
    {
        if (g.graphic.Count == 0)
            return;

        int j = 0;

        for (int i = 0; i < g.coords.Count; i++)
        {
            if (g.isVisable[i])
            {
                g.graphic[j].transform.position = g.proj_coords[i].position;
                g.graphic[j].GetComponent<SpriteRenderer>().sortingOrder = g.proj_coords[i].depth;
                j++;
            }
        }
    }

    protected void onHoverChangeSelected(string name)
    {
        if (hover == null)
        {
            Debug.Log("Editor graphics contol: hover is null for some reason.");
            return;
        }

        IsoObjectGhost g = new IsoObjectGhost(name, hover.getOrigin(), hover.getDirection());
        
        hover.destroy();
        hover = g;
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
        sr.sharedMaterial = mat;
        sr.sortingLayerName = "lengthSort";
        sr.sortingOrder = i.depth;

        return ret;
    }

    protected override void destructor()
    {
       
        GameObject.Destroy(gld.gameObject);

        EditorComponentMouseLayer.removeEnableLayer(enableLayer);
        EditorComponentMouseLayer.removeMoveLayer(moveLayer);

        IsoObjectGhost.removeOnCreate(onGhostCreate);
        IsoObjectGhost.removeOnDestroy(onGhostDestroy);
        IsoObjectGhost.removeOnRotate(onGhostRotate);
        IsoObjectGhost.removeOnTranslate(onGhostTranlate);
        EditorComponentUI.removeOnChangeSelected(onHoverChangeSelected);
        return;
    }
}
