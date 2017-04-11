
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects only lit at origin point.
/// Will inform all sprites
/// </summary>
public class Thread_IsoObject  {
    public ulong id { get; protected set; }
    public int hash { get; protected set; }
    //public int value = 0;
    public int level = -1;
    public Iso origin { get; protected set; }
    //public Dictionary<ulong, Thread_Light> coverdBy = new Dictionary<ulong, Thread_Light>();
    List<Iso> sprites = new List<Iso>();
    public List<Iso> coords = new List<Iso>();

    Thread_IsoObject(Iso origin)
    {
        this.origin = origin;
    }

    public Thread_IsoObject(IsoObject o, int hash) : this(o.origin)
    {
        this.hash = hash;
        id = o.id;

        //Fetch sprites cells
        for (int i=0; i< o.coords.Count; i++)
        {
            coords.Add(new Iso(o.coords[i]));
            if (o.getSprite(i) != null)
                sprites.Add(new Iso(o.coords[i]));
        }

    }
}
