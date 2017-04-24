
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// The hover mouse object for the level editor
/// </summary>
public class IsoObjectGhost : IsoObjectBody, MouseHoverObject {

    public List<GameObject> graphic { get; protected set; }
    public List<ProjIso> proj_coords { get; protected set; }

    /// <summary>
    /// Copy constructor.
    /// Offsets coordinats by origin.
    /// Shares sprite stringlist with prototype
    /// </summary>
    public IsoObjectGhost(string prototype, Iso origin, Directions.dir direction = Directions.dir.N) : this(Atlas_Loader.main.getObject(prototype), origin, direction) { }

    /// <summary>
    /// The actual constructor
    /// </summary>
    private IsoObjectGhost(IsoObjectBody prototype, Iso origin, Directions.dir direction) : base(prototype, origin, direction)
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


    /// <summary>
    /// Translate to target position
    /// </summary>
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

    /// <summary>
    /// Rotate this object
    /// </summary>
    public void rotate(Directions.dir new_direction)
    {
        direction = new_direction;
        if (onRotate != null)
            onRotate(this);
    }

    /// <summary>
    /// Hide this object
    /// </summary>
    public void hide()
    {
        foreach (GameObject g in graphic)
        {
            g.SetActive(false);
        }
    }

    /// <summary>
    /// Unhide this obejct
    /// </summary>
    public void unhide()
    {
        foreach(GameObject g in graphic)
        {
            g.SetActive(true);
        }
    }

    /// <summary>
    /// Destroy this obejct
    /// </summary>
    public override void destroy()
    {
        if (onDestroy != null)
            onDestroy(this);
    }

    /// <summary>
    /// return origin
    /// </summary>
    public Iso getOrigin()
    {
        return origin;
    }

    /// <summary>
    /// Get current direction
    /// </summary>
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
