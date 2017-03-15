
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
        t = null;
        return false;
    }

    public override void callbackClick(string mode)
    {
        throw new NotImplementedException();
    }
}
