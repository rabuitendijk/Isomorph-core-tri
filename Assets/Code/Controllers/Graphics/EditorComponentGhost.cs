using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the mouse hover in the graphics control
/// </summary>
public class EditorComponentGhost
{
    Material ghostMat;
    EditorGraphicsControl main;
    Transform tileFolder;

    public EditorComponentGhost(EditorGraphicsControl main, Transform tileFolder)
    {
        this.main = main;
        this.tileFolder = tileFolder;


        ghostMat = new Material(Shader.Find("Sprites/Default"));
        ghostMat.color = new Color(1f, 1f, 1f, .6f);

        IsoObjectGhost.registerOnCreate(onGhostCreate);
        IsoObjectGhost.registerOnDestroy(onGhostDestroy);
        IsoObjectGhost.registerOnRotate(onGhostRotate);
        IsoObjectGhost.registerOnTranslate(onGhostTranlate);

        EditorUIControl.registerOnChangeSelected(onHoverChangeSelected);
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
        foreach (GameObject ob in g.graphic)
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
        if (main.hover == null)
        {
            Debug.Log("Editor graphics contol: hover is null for some reason.");
            return;
        }

        IsoObjectGhost g = new IsoObjectGhost(name, main.hover.getOrigin(), main.hover.getDirection());

        main.hover.destroy();
        main.hover = g;
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

    public void destroy()
    {
        IsoObjectGhost.removeOnCreate(onGhostCreate);
        IsoObjectGhost.removeOnDestroy(onGhostDestroy);
        IsoObjectGhost.removeOnRotate(onGhostRotate);
        IsoObjectGhost.removeOnTranslate(onGhostTranlate);

        EditorUIControl.removeOnChangeSelected(onHoverChangeSelected);
    }	
}
