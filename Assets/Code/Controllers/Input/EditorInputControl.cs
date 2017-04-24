 
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input control for level editor mode
/// </summary>
public class EditorInputControl : InputControl {
    EditorComponentMouseLayer mouseLayer;
    EditorComponentMouseStack mouseStack;
    public string selected = "VOID";

    /// <summary>
    /// Create the common level input controller
    /// </summary>
    public EditorInputControl() : base()
    {
        componentCamera = new BasicComponentCamera();
        mouseStack = new EditorComponentMouseStack(this);
        componentMouse = mouseStack;

        EditorUIControl.registerOnChangeSelected(onChangeSelected);
    }

    /// <summary>
    /// Respond to change selected in ui
    /// </summary>
    void onChangeSelected(string name)
    {
        selected = name;
    }

    /// <summary>
    /// Delayed constructor
    /// </summary>
    public override void delayedConstruction()
    {
        //Empty
    }

    /// <summary>
    /// Call onclick in mouse object
    /// </summary>
    protected override void onClick(string mode)
    {
        if (componentMouse != null)
            componentMouse.onClick(mode);

    }

    /// <summary>
    /// Input processing, run this once per frame
    /// </summary>
    public override void update()
    {
        componentCamera.update();

        if (!UIControl.main.usesKeys())
        {   //Ignore common keyboard input when in input field

            if (mouseLayer != null)
                shiftLayer();

            if (Input.GetKeyDown(KeyCode.P))
                switchMouseMode();

            rotate();
            mapRotate();
        }


        componentMouse.update();
    }

    /// <summary>
    /// Rotate mouse hover
    /// </summary>
    void rotate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GraphicsControl.main.hover.rotate((Directions.dir)(((int)GraphicsControl.main.hover.getDirection()+1)%4));
            return;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            int temp = (int)GraphicsControl.main.hover.getDirection() - 1;
            if (temp < 0)
                temp = 4 + temp;

            GraphicsControl.main.hover.rotate((Directions.dir)temp);
            return;
        }
    }

    /// <summary>
    /// Rotate map
    /// </summary>
    void mapRotate()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GraphicsControl.main.rotate((Directions.dir)(((int)Directions.currentDirection + 1) % 4));
            return;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            int temp = ((int)Directions.currentDirection) - 1;
            if (temp < 0)
                temp = 4 + temp;

            GraphicsControl.main.rotate((Directions.dir)temp);
            return;
        }
    }

    /// <summary>
    /// Change mouse mode
    /// </summary>
    void switchMouseMode()
    {
        //GraphicsControl.main.selector.SetActive(false);
        if (mouseLayer == null) //Go to stack mode
        {
            mouseLayer = new EditorComponentMouseLayer(this, mouseStack.getHeight());
            componentMouse = mouseLayer;
            mouseStack = null;
            //Debug.Log("Switch to Layer mouse mode.");
            return;
        }

        //Disable layer
        if (EditorComponentMouseLayer.enableLayer != null)
            EditorComponentMouseLayer.enableLayer(false, 0);

        mouseStack = new EditorComponentMouseStack(this);
        componentMouse = mouseStack;
        mouseLayer = null;
        //Debug.Log("Switch to Stack mouse mode");

    }

    /// <summary>
    /// Check if layer needs to be shifted up or down
    /// </summary>
    void shiftLayer()
    {
        if (Input.GetKeyDown(KeyCode.Q) && mouseLayer.layer > 0 && EditorComponentMouseLayer.moveLayer != null)
        {
            EditorComponentMouseLayer.moveLayer(false);
            mouseLayer.layer--;
        }
        else if (Input.GetKeyDown(KeyCode.E) && mouseLayer.layer < LogicControl.main.height - 1 && EditorComponentMouseLayer.moveLayer != null)
        {
            EditorComponentMouseLayer.moveLayer(true);
            mouseLayer.layer++;
        }
    }

    /// <summary>
    /// Destroy this obejct
    /// </summary>
    protected override void destructor()
    {
        EditorUIControl.removeOnChangeSelected(onChangeSelected);
        return;
    }
}
