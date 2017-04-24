
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A moveble object like a unit or main caracter
/// </summary>
public class IsoTranslatable {

    public List<Iso> coords { get; protected set; }
    public List<Sprite> sprites { get; protected set; }
    public List<GameObject> objects { get; protected set; }
    public int width { get; protected set; }
    public int length { get; protected set; }
    public int height { get; protected set; }
    public FloatingIso origin { get; protected set; }


    /// <summary>
    /// Constructor implementation
    /// </summary>
	private IsoTranslatable(IsoObjectBody prototype)
    {
        for (int i = 0; i < prototype.coords.Count; i++)
        {
            if (prototype.directions[0][i] != null)
            {
                sprites.Add(prototype.directions[0][i]);
                coords.Add(new Iso(prototype.coords[i]));
            }
        }
        width = prototype.width;
        height = prototype.height;
        length = prototype.length;

        origin = new FloatingIso(prototype.origin);
        onCreate(this);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public IsoTranslatable(string prototype) : this(Atlas_Loader.main.getObject(prototype)) { }


    static Action<IsoTranslatable> onCreate;
    public static void registerOnCreate(Action<IsoTranslatable> funct) { onCreate += funct; }
    public static void removeOnCreate(Action<IsoTranslatable> funct) { onCreate -= funct; }

    static Action<IsoTranslatable> onDestroy;
    public static void registerOnDestroy(Action<IsoTranslatable> funct) { onDestroy += funct; }
    public static void removeOnDestroy(Action<IsoTranslatable> funct) { onDestroy -= funct; }

    /// <summary>
    /// Destroy this obejct
    /// </summary>
    public void destroy()
    {
        onDestroy(this);
    }
}
