using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An extended version of Iso that can be projected to the screen [and rotaded]
/// contains depth information
/// </summary>
public class ProjIso : Iso {

    public static long next_id = 0;

    public long id { get; protected set; }

    public ProjIso()
    {
        id = next_id++;

        if (onCreate != null)
            onCreate(this);
    }

    public void destroy()
    {
        if (onDestroy != null)
            onDestroy(this);
    }

    Action<ProjIso> onCreate;
    public void registerOnCreate(Action<ProjIso> funct) { onCreate += funct; }
    public void removeOnCreate(Action<ProjIso> funct) { onCreate -= funct; }

    Action<ProjIso> onDestroy;
    public void registerOnDestroy(Action<ProjIso> funct) { onDestroy += funct; }
    public void removeOnDestroy(Action<ProjIso> funct) { onDestroy -= funct; }
}
