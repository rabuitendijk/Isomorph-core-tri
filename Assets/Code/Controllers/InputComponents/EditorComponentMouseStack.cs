using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// How do I get to the ui information???
/// </summary>
public class EditorComponentMouseStack : ComponentMouse {

    Iso selected;
    IsoObject lastObject;
    EditorComponentUI ui;

    public EditorComponentMouseStack(EditorComponentUI ui)
    {
        this.ui = ui;
    }

    public override bool update(out Tile t)
    {
        t = null;

        if (catchHitFloor(out selected))
        {
            GraphicsControl.main.selector.SetActive(true);
            Iso.moveTo(selected, GraphicsControl.main.selector);
            return true;
        }

        GraphicsControl.main.selector.SetActive(false);
        return false;

    }

    /// <summary>
    /// Racasthit plus hitting empty floor functionallity
    /// </summary>
    bool catchHitFloor(out Iso i)
    {
        //?
        
        if (raycastClick(out i))
            return true;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float upper_x = (2f * pos.y - pos.x + 1f), upper_y = (2f * pos.y + pos.x + 1f);
        Iso target = new Iso(pos.x, pos.y, 0, 1);

        if (LogicControl.main.inGrid(target) && !LogicControl.main.exists(target))
        {
            i = target;
            return true;
        }
        i = null;
        return false;
    }


    /// <summary>
    /// finds last unoccupied Iso
    /// </summary>
    bool raycastClick(out Iso i)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float upper_z = LogicControl.main.height + 1, delta;
        float upper_x = (2f * pos.y - pos.x - .5f * upper_z + 1f), upper_y = (2f * pos.y + pos.x - .5f * upper_z + 1f);
        Iso target = new Iso(pos.x, pos.y, Mathf.FloorToInt(upper_z), 1);
        target.addUnsafe(0, 0, -1); //Start correction

        i = new Iso(0,0,0);

        while (target.z >= 0)
        {
            delta = nextCell(upper_x, upper_y, upper_z, target);
            upper_z -= 2 * delta; upper_x += delta; upper_y += delta;
            if (LogicControl.main.inGrid(target))
            {
                if (LogicControl.main.exists(target))
                {
                    if (i == null)
                        return false;
                    return true;
                }
                i.set(target);
            }
            
        }

        return false;
    }

    /// <summary>
    /// Finds next cell in simulated raycast for mouse click
    /// </summary>
    float nextCell(float ox, float oy, float oz, Iso coord)
    {
        ox -= coord.x; ox = 1 - ox;
        oy -= coord.y; oy = 1 - oy;
        oz -= coord.z; oz /= 2;

        if (oz <= ox)
        {
            if (oz <= oy)
            {
                //z transition

                coord.addUnsafe(0, 0, -1);
                return oz;
            }
            //y transtion
            coord.addUnsafe(0, 1, 0);
            return oy;
        }
        else if (ox <= oy)
        {
            //x transtion

            coord.addUnsafe(1, 0, 0);
            return ox;
        }
        //y transition
        coord.addUnsafe(0, 1, 0);
        return oy;
    }


    public override void callbackClick(string mode)
    {
        string name = ui.getSelectedObject();
        if (name == "VOID")
            return;

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

        Debug.Log("You clicked:"+mode);
    }
}
