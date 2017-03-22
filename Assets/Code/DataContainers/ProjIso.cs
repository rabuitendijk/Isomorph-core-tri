using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An extended version of Iso that can be projected to the screen [and rotaded]
/// contains length information
/// </summary>
public class ProjIso : Iso {

    public static long next_id = 0;

    int depthModifier = 0;
    public int depth { get { return 2 * (2 * x + 2 * y + z) + depthModifier; } }
    public Vector3 position { get { return new Vector3((-x + y) * 0.5f, (-x - y + z) * 0.25f, z + 2 * x + 2 * y); } }

    



    public ProjIso(int x, int y, int z, int depthModifier = 0): base(x,y,z){this.depthModifier = depthModifier;}

    public ProjIso(Iso i, int depthModifier = 0) : base(i){this.depthModifier = depthModifier;}

    /// <summary>
    /// Moves this iso to other and recalculates
    /// </summary>
    public static void moveTo(ProjIso i, GameObject graphic)
    {

        if (graphic == null)
            return;

        graphic.transform.position = i.position;
        graphic.GetComponent<SpriteRenderer>().sortingOrder = i.depth;
    }

   
}
