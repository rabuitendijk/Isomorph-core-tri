
using System.Collections.Generic;
using UnityEngine;

public class FloatingIso {

    public float x, y, z;
    public Iso iso{get{ return new Iso((int)x, (int)y, (int)z); }}
    int depthModifier;

    public FloatingIso(float x, float y, float z, int depthModifier=0) 
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.depthModifier = depthModifier;
    }

    public FloatingIso(Iso i) : this(i.x, i.y, i.z) { }

    public int depth { get { return 2 * (2 * (int)x + 2 *(int) y + (int)z) + depthModifier; } }
    public Vector3 position { get { return new Vector3((-x + y) * 0.5f, (-x - y + z) * 0.25f, z + 2 * x + 2 * y); } }
}
