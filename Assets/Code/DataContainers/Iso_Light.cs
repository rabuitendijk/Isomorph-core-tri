﻿
using System.Collections.Generic;
using UnityEngine;
using System;

public class Iso_Light {

	public ulong light_id { get; protected set; }

    //public Iso coord { get; protected set; }
    public int radius { get; protected set; }
    public IsoObject source { get; protected set; }

    public Iso_Light(IsoObject source, int radius)
    {
        //this.coord = coord;
        this.radius = radius;
        this.source = source;
        light_id = LightingControl.light_id++;


        if (onCreate != null)
            onCreate(this);
    }

    public void destroy()
    {
        if (onDestroy != null)
            onDestroy(this);
    }

    static Action<Iso_Light> onCreate;
    public static void registerOnCreate(Action<Iso_Light> funct) { onCreate += funct; }
    public static void removeOnCreate(Action<Iso_Light> funct) { onCreate -= funct; }
    static Action<Iso_Light> onDestroy;
    public static void registerOnDestroy(Action<Iso_Light> funct) { onDestroy += funct; }
    public static void removeOnDestroy(Action<Iso_Light> funct) { onDestroy -= funct; }
}