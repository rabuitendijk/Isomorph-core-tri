using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A disabled version of the save sytem
/// </summary>
public class NoSaveControl : SaveControl
{
    public NoSaveControl() : base()
    {
        return;
    }

    public override void delayedConstruction()
    {
        return;
    }

    protected override void destructor()
    {
        return;
    }

    protected override void save(string filename)
    {
        return;
    }
}
