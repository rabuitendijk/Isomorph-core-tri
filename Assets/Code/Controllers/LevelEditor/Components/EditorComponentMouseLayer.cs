
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Layer mode, 
/// </summary>
public class EditorComponentMouseLayer : ComponentMouse
{
    public int layer;

    public static Action<bool> moveLayer;
    public static Action<bool, int> enableLayer;
    public static void registerMoveLayer(Action<bool> funct) { moveLayer += funct; }
    public static void registerEnableLayer(Action<bool, int> funct) { enableLayer += funct; }
    public static void removeMoveLayer(Action<bool> funct) { moveLayer -= funct; }
    public static void removeEnableLayer(Action<bool, int> funct) { enableLayer -= funct; }

    EditorComponentUI ui;
    Iso selected;

    /// <summary>
    /// 
    /// </summary>
    public EditorComponentMouseLayer(EditorComponentUI ui, int layer=0)
    {
        this.ui = ui;
        this.layer = layer;

        if (enableLayer != null)
            enableLayer(true, layer);
    }

    public override bool update(out Tile t)
    {
        GraphicsControl.main.selector.SetActive(false);

        if (catchHitFloor(out selected))
        {   // Valid coordinate
            GraphicsControl.main.selector.SetActive(true);
            Iso.moveTo(selected, GraphicsControl.main.selector);
        }

        t = null;
        return false;


    }


    /// <summary>
    /// Racasthit plus hitting empty floor functionallity
    /// </summary>
    bool catchHitFloor(out Iso i)
    {
        

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float upper_x = (2f * pos.y - pos.x - .5f * layer + 1f), upper_y = (2f * pos.y + pos.x - .5f * layer + 1f);
        Iso target = new Iso(pos.x, pos.y, Mathf.FloorToInt(layer), 1);

        if (LogicControl.main.inGrid(target))
        {
            i = target;
            return true;
        }
        i = null;
        return false;
    }

    public override void callbackClick(string mode)
    {
        //Remove at righth mouse click
        if (mode == "right")
        {
            Tile hit;
            if (selected != null && LogicControl.main.exists(selected))
            {
                hit = LogicControl.main.get(selected);
                foreach (Tile t in hit.isoObject.tiles)
                {
                    t.destroy();
                    hit = null;
                }
            }
            else
                Debug.Log("No tile selected for removal.");

            return;
        }

        string name = ui.getSelectedObject();
        if (name == "VOID")
            return;

        //Add at left mouse click
        if (mode == "left")
        {
            if (selected != null)
            {
                new IsoObject(name, selected);
            }
            else
                Debug.Log("No tile selected.");
            return;
        }

        Debug.Log("You clicked:" + mode);
    }
}
