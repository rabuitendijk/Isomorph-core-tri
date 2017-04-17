
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For when no lighting is needed
/// </summary>
public class NoLightingControl : LightingControl
{
    public override void delayedConstruction()
    {
        return;
    }

    public override void runOnMainThread()
    {
        return;
    }

    public override void update()
    {
        return;
    }

    protected override void destructor()
    {
        return;
    }
}
