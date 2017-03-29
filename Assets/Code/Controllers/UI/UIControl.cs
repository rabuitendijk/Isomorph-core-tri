using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UIControl {
    public static UIControl main;
    protected UIControl()
    {
        main = this;
    }

    public abstract void delayedConstruction();

    protected static Action<string> onMouseClick;
    public static void registerOnMouseClick(Action<string> funct) { onMouseClick += funct; }
    public static void removeOnMouseClick(Action<string> funct) { onMouseClick -= funct; }

    /// <summary>
    /// Check if keys are being absorbed
    /// </summary>
    public abstract bool usesKeys();

    protected abstract void destructor();
    public void destroy()
    {
        destructor();
    }
}
