
using System.Collections.Generic;
using UnityEngine;

public class Thread_Light {

    public Dictionary<int, Thread_IsoObject> coverage = new Dictionary<int, Thread_IsoObject>();
    public Dictionary<int, int> coverage_value = new Dictionary<int, int>();
    public ulong id { get; protected set; }
    public int radius { get; protected set; }
    public Iso coord { get; protected set; }

    public Thread_Light(ulong id, int radius, Iso coord)
    {
        this.id = id;
        this.radius = radius;
        this.coord = coord;
    }
}
