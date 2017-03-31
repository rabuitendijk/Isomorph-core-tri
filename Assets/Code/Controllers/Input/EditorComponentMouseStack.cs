using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// How do I get to the ui information???
/// </summary>
public class EditorComponentMouseStack : ComponentMouse {

    Iso selected;
    Tile hit;
    IsoObject lastObject;
    EditorInputControl input;

    public EditorComponentMouseStack(EditorInputControl input)
    {
        this.input = input;
    }

    public override void update()
    {

        if (catchHitFloor(out selected))
        {
            GraphicsControl.main.hover.unhide();
            GraphicsControl.main.hover.translate(selected);
            return;
        }

        GraphicsControl.main.hover.hide();

    }

    /// <summary>
    /// Racasthit plus hitting empty floor functionallity
    /// </summary>
    bool catchHitFloor(out Iso i)
    {
        //?
        if (Directions.raycastClick(out i, out hit))
            return true;

        Iso target = Directions.mouseToFloor(0) ;

        if (LogicControl.main.inGrid(target) && !LogicControl.main.exists(target))
        {
            i = target;
            return true;
        }
        i = null;
        return false;
    }

    public override void onClick(string mode)
    {
        //Remove at righth mouse click
        if (mode == "right")
        {
            if (hit != null)
                hit.isoObject.destroy();
            else
                Debug.Log("No tile selected for removal.");

            return;
        }

        string name = input.selected;
        if (name == "VOID")
            return;

        //Add at left mouse click
        if (mode == "left")
        {
            if (selected != null)
            {
                new IsoObject(name, selected, GraphicsControl.main.hover.getDirection());
            }
            else
                Debug.Log("No tile selected.");
            return;
        }

        Debug.Log("You clicked:"+mode);
    }

    /// <summary>
    /// Get height for switching
    /// </summary>
    public int getHeight()
    {
        if (selected != null)
            return selected.z;
        return 0;
    }
}
