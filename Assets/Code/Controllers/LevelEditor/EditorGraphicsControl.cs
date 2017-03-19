 
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

    Material mat, redMat;
    List<Transform> folders = new List<Transform>();
    GLDrawLoop gld;
    GLInstruction gliGrid, gliStruct;
    int height = 0;

    /// <summary>
    /// Constructor that initialises references and materials,
    /// </summary>
    public EditorGraphicsControl() : base()
    {

        mat = new Material(Shader.Find("Sprites/Default"));
        redMat = new Material(Shader.Find("Sprites/Default"));
        redMat.color = new Color(1f, 0f, 0f);

        selector = newOb("Selector", new Iso(0, 0, 0, 1), Alias_Loader.main.getSprite("Selector[0_0_0]"), mat);
        selector.SetActive(false);

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
        coords.Add(new Vector3(0f, -.5f, 1f));
        coords.Add(new Vector3(LogicControl.main.width*-.5f, LogicControl.main.width * .25f-.5f, 0));
        coords.Add(new Vector3(0, (LogicControl.main.width + LogicControl.main.depth) * .25f-.5f, 0));
        coords.Add(new Vector3(LogicControl.main.depth * .5f, LogicControl.main.depth * .25f-.5f, 0));
        gld.add(new GLInstruction(coords, Color.red));
    }

    /// <summary>
    /// Creates some GL gui information
    /// </summary>
    void setGLIStruct(int h)
    {
        List<Vector3> coords = new List<Vector3>();

        coords.Add(new Vector3(0f,h*.25f -.5f, 1f));
        coords.Add(new Vector3(LogicControl.main.width * -.5f, (LogicControl.main.width+h) * .25f - .5f, 0));
        coords.Add(new Vector3(0, (LogicControl.main.width + LogicControl.main.depth+h) * .25f - .5f, 0));
        coords.Add(new Vector3(LogicControl.main.depth * .5f, (LogicControl.main.depth+h) * .25f - .5f, 0));
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
            coords.Add(new Vector3((-i) * .5f, (i + h) * .25f - .5f, 0));
            coords.Add(new Vector3((-i+LogicControl.main.depth) * .5f, (i + LogicControl.main.depth + h) * .25f - .5f, 0));
        }

        for (int j = 0; j < LogicControl.main.depth; j++)
        {

            coords.Add(new Vector3((j) * .5f, (j + h) * .25f - .5f, 0));
            coords.Add(new Vector3((j-LogicControl.main.width) * .5f, (LogicControl.main.width+j + h) * .25f - .5f, 0));
        }

        gliGrid = new GLInstruction(coords, Color.gray, false, false);
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
        ret.transform.position = t.coord.toPos();
        ret.transform.parent = folders[t.coord.z];

        SpriteRenderer sr = ret.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = t.sprite;
        sr.material = mat;
        sr.sortingLayerName = "DepthSort";
        sr.sortingOrder = t.coord.depth;

        return ret;
    }

    /// <summary>
    /// Common sprtie gameobject creator.
    /// </summary>
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

    protected override void destructor()
    {
        GameObject.Destroy(selector);
        selector = null;

       
        GameObject.Destroy(gld.gameObject);

        EditorComponentMouseLayer.removeEnableLayer(enableLayer);
        EditorComponentMouseLayer.removeMoveLayer(moveLayer);
        return;
    }
}
