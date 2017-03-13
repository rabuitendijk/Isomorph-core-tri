 
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imput control for level editor mode
/// </summary>
public class EditorInputControl : InputControl {
    EditorComponentUI ui;

    /// <summary>
    /// Create the common level input controller
    /// </summary>
    public EditorInputControl() : base()
    {
        componentCamera = new BasicComponentCamera();
        ui = new EditorComponentUI();
        componentMouse = new EditorComponentMouseStack(ui);

        ui.registerOnClick(componentMouse.callbackClick);
    }

    /// <summary>
    /// Input processing, run this once per frame
    /// </summary>
    public override void update()
    {
        componentCamera.update();
        Tile t;
        componentMouse.update(out t);
    }

    
    protected override void destructor()
    {
        ui.destructor();
        ui.removeOnClick(componentMouse.callbackClick);
        return;
    }
}
