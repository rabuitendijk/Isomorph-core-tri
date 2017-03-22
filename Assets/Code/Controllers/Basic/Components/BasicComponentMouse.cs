
using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicComponentMouse : ComponentMouse {
    Tile currentTile;
    Material hoverMaterial, lastMaterial;
    IsoObject lastObject;

    public BasicComponentMouse()
    {
        hoverMaterial = new Material(Shader.Find("Sprites/Default"));
        hoverMaterial.color = new Color(.7f, .7f, .7f);
    }

    public override bool update(out Tile t)
    {
        t = null;

        
        return false;

    }

    public override void callbackClick(string mode)
    {
        Debug.Log("BasicComponentMouse.callbackClick: No callback");
    }

}
