using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sparce input controller for movement testing purposes
/// </summary>
public class TestingInputControl : InputControl
{
    /// <summary>
    /// Constructor
    /// </summary>
    public TestingInputControl() : base()
    {
        componentCamera = new BasicComponentCamera();
    }

    /// <summary>
    /// Delayed constructor
    /// </summary>
    public override void delayedConstruction()
    {
        return;
    }

    /// <summary>
    /// Run once per frame
    /// </summary>
    public override void update()
    {
        componentCamera.update();
    }

    /// <summary>
    /// Destroy this obejct
    /// </summary>
    protected override void destructor()
    {
        return;
    }

    /// <summary>
    /// React to click event
    /// </summary>
    protected override void onClick(string mode)
    {
        throw new NotImplementedException();
    }
}
