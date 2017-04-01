 
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imput control for level editor mode
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

    void onChangeSelected(string name)
    {
        selected = name;
    }

    public override void delayedConstruction()
    {
        //Empty
    }

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

    protected override void destructor()
    {
        EditorUIControl.removeOnChangeSelected(onChangeSelected);
        return;
    }
}
