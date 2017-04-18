using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoUIControl : UIControl
{
    public NoUIControl() : base()
    {
        return;
    }

    public override void delayedConstruction()
    {
        return;
    }

    public override bool usesKeys()
    {
        return false;
    }

    protected override void destructor()
    {
        return;
    }
}
