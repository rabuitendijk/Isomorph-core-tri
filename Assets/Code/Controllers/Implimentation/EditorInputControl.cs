
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
        componentMouse = new BasicComponentMouse();
        ui = new EditorComponentUI();
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
        return;
    }
}
