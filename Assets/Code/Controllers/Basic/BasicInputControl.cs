
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version aplha-1
/// 
/// Currently test version for development.
/// 
/// POSSIBLE BUG:
/// Camera transform not snapping to nearest pixel might be causing graphics glitches.
/// 
/// Robin Apollo Buitendijk
/// Early March 2017
/// </summary>
public class BasicInputControl : InputControl{


    /// <summary>
    /// Create the common level input controller
    /// </summary>
    public BasicInputControl() : base()
    {
        componentCamera = new BasicComponentCamera();
        componentMouse = new BasicComponentMouse();
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
        return;
    }
}
