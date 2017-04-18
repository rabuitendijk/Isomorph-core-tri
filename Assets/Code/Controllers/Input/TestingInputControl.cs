using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingInputControl : InputControl
{
    public TestingInputControl() : base()
    {
        componentCamera = new BasicComponentCamera();
    }

    public override void delayedConstruction()
    {
        return;
    }

    public override void update()
    {
        componentCamera.update();
    }

    protected override void destructor()
    {
        return;
    }

    protected override void onClick(string mode)
    {
        throw new NotImplementedException();
    }
}
