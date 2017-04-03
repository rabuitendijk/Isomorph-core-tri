
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class LightingControl : Controller
{
    public static ulong light_id;

    protected LightingControl()
    {
        light_id = 0;
    }

    public void delayedConstruction()
    {
        //Empty
    }

    protected static Action onLightingProcessed;

    public static void registerOnLightingProcessed(Action funct) { onLightingProcessed += funct; }
    public static void removeOnLightingProcessed(Action funct) { onLightingProcessed -= funct; }

    protected abstract void destructor();
    public abstract void update();
    public abstract void runOnMainThread();

    public void destroy()
    {
        destructor();
    }
}
