
using System.Collections.Generic;
using UnityEngine;

public class Thread_Light {

    public ulong id { get; protected set; }
    public int radius { get; protected set; }
    public Iso coord { get; protected set; }
    public List<Iso> coverage = new List<Iso>();

    public Thread_Light(ulong id, int radius, Iso coord)
    {
        this.id = id;
        this.radius = radius;
        this.coord = coord;
    }
}
