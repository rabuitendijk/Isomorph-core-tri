using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the GLD in graphics control
/// </summary>
public class EditorComonentGLD {

    List<Transform> folders = new List<Transform>();
    GLDrawLoop gld;
    GLInstruction gliGrid, gliStruct;
    int height = 0;


    public EditorComonentGLD(List<Transform> folders)
    {
        this.folders = folders;

        GameObject temp = new GameObject() { name = "GLD" };
        gld = temp.AddComponent<GLDrawLoop>();
        

        EditorComponentMouseLayer.registerEnableLayer(enableLayer);
        EditorComponentMouseLayer.registerMoveLayer(moveLayer);
    }

    public void delayedConstruction() {
        setGLD();
    }
    
    /// <summary>
    /// Creates the GLD
    /// </summary>
    void setGLD()
    {


        List<Vector3> coords = new List<Vector3>();
        coords.Add(new ProjIso(0, 0, 0).position);
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

        for (int i = height + 1; i < folders.Count; i++)
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

    public void destroy()
    {
        gld.GetComponent<GLDrawLoop>().clear();
        GameObject.Destroy(gld.gameObject);

        EditorComponentMouseLayer.removeEnableLayer(enableLayer);
        EditorComponentMouseLayer.removeMoveLayer(moveLayer);
    }

}
