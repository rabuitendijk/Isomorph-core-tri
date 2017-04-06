
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class LightingControl : Controller
{
    public static LightingControl main;
    public static ulong light_id;
    public Queue<string> print_queue = new Queue<string>();

    protected LightingControl()
    {
        light_id = 0;
        main = this;
    }

    public abstract void delayedConstruction();

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
