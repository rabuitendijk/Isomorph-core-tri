
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects only lit at origin point.
/// Will inform all sprites
/// </summary>
public class Thread_IsoObject  {
    //public ulong id { get; protected set; }
    public int hash { get; protected set; }
    public ushort value = 0;
    public Iso origin { get; protected set; }
    public Dictionary<ulong, Thread_Light> coverdBy = new Dictionary<ulong, Thread_Light>();
    List<Iso> sprites = new List<Iso>();

    Thread_IsoObject(Iso origin)
    {
        this.origin = origin;
    }

    public Thread_IsoObject(IsoObject o, int hash) : this(o.origin)
    {
        this.hash = hash;
        //Fetch sprites cells
        for (int i=0; i< o.coords.Count; i++)
        {
            if (o.getSprite(i) != null)
                sprites.Add(o.coords[i]);
        }

    }
}
