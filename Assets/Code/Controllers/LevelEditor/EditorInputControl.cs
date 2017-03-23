 
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imput control for level editor mode
/// </summary>
public class EditorInputControl : InputControl {
    EditorComponentUI ui;
    EditorComponentMouseLayer mouseLayer;
    EditorComponentMouseStack mouseStack;

    /// <summary>
    /// Create the common level input controller
    /// </summary>
    public EditorInputControl() : base()
    {
        componentCamera = new BasicComponentCamera();
        ui = new EditorComponentUI();
        mouseStack = new EditorComponentMouseStack(ui);
        componentMouse = mouseStack;

        ui.registerOnClick(componentMouse.callbackClick);
    }

    /// <summary>
    /// Input processing, run this once per frame
    /// </summary>
    public override void update()
    {
        componentCamera.update();

        if (!ui.beingEdited)
        {   //Ignore common keyboard input when in input field

            if (mouseLayer != null)
                shiftLayer();

            if (Input.GetKeyDown(KeyCode.P))
                switchMouseMode();

            rotate();
            mapRotate();
        }


        Tile t;
        componentMouse.update(out t);
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
        ui.removeOnClick(componentMouse.callbackClick);
        //GraphicsControl.main.selector.SetActive(false);
        if (mouseLayer == null) //Go to stack mode
        {
            mouseLayer = new EditorComponentMouseLayer(ui, mouseStack.getHeight());
            componentMouse = mouseLayer;
            mouseStack = null;
            Debug.Log("Switch to Layer mouse mode.");
            ui.registerOnClick(componentMouse.callbackClick);
            return;
        }

        //Disable layer
        if (EditorComponentMouseLayer.enableLayer != null)
            EditorComponentMouseLayer.enableLayer(false, 0);

        mouseStack = new EditorComponentMouseStack(ui);
        componentMouse = mouseStack;
        mouseLayer = null;
        ui.registerOnClick(componentMouse.callbackClick);
        Debug.Log("Switch to Stack mouse mode");

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
        ui.destructor();
        ui.removeOnClick(componentMouse.callbackClick);
        return;
    }
}
