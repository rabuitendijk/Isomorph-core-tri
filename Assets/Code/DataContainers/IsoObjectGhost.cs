using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IsoObjectGhost : IsoObjectBody, MouseHoverObject {

    public List<GameObject> graphic { get; protected set; }
    public List<ProjIso> proj_coords { get; protected set; }
    /// <summary>
    /// Copy constructor.
    /// Offsets coordinats by origin.
    /// Shares sprite stringlist with prototype
    /// </summary>
    public IsoObjectGhost(string prototype, Iso origin, Directions.dir direction = Directions.dir.N) : this(Atlas_Loader.main.getObject(prototype), origin, direction) { }

    private IsoObjectGhost(IsoObjectBody prototype, Iso origin, Directions.dir direction) : base(prototype.name, prototype.coords, prototype.directions, origin, direction)
    {
        graphic = new List<GameObject>();
        foreach (Iso i in coords)
        {
            i.add(origin);
        }

        proj_coords = new List<ProjIso>();
        foreach(Iso c in coords)
        {
            proj_coords.Add(new ProjIso(c));
        }

        if (onCreate != null)
            onCreate(this);
    }



    public void translate(Iso target)
    {
        Iso diff = new Iso(target.x - origin.x, target.y - origin.y, target.z - origin.z);
        origin.add(diff);

        foreach (Iso i in proj_coords)
        {
            i.add(diff);
        }

        if (onTranslate != null)
            onTranslate(this);
    }

    public void rotate(Directions.dir new_direction)
    {
        direction = new_direction;
        if (onRotate != null)
            onRotate(this);
    }

    public void hide()
    {
        foreach (GameObject g in graphic)
        {
            g.SetActive(false);
        }
    }

    public void unhide()
    {
        foreach(GameObject g in graphic)
        {
            g.SetActive(true);
        }
    }


    public override void destroy()
    {
        if (onDestroy != null)
            onDestroy(this);
    }

    public Iso getOrigin()
    {
        return origin;
    }

    public Directions.dir getDirection()
    {
        return direction;
    }

    static Action<IsoObjectGhost> onCreate;
    public static void registerOnCreate(Action<IsoObjectGhost> funct) { onCreate += funct; }
    public static void removeOnCreate(Action<IsoObjectGhost> funct) { onCreate -= funct; }
    static Action<IsoObjectGhost> onDestroy;
    public static void registerOnDestroy(Action<IsoObjectGhost> funct) { onDestroy += funct; }
    public static void removeOnDestroy(Action<IsoObjectGhost> funct) { onDestroy -= funct; }
    static Action<IsoObjectGhost> onTranslate;
    public static void registerOnTranslate(Action<IsoObjectGhost> funct) { onTranslate += funct; }
    public static void removeOnTranslate(Action<IsoObjectGhost> funct) { onTranslate -= funct; }
    static Action<IsoObjectGhost> onRotate;
    public static void registerOnRotate(Action<IsoObjectGhost> funct) { onRotate += funct; }
    public static void removeOnRotate(Action<IsoObjectGhost> funct) { onRotate -= funct; }



}
