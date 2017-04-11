
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data on tile coverage by lights
/// </summary>
public class Lighting_Bucket  {

    Dictionary<ulong, Thread_Light> lights = new Dictionary<ulong, Thread_Light>();
    Dictionary<ulong, int> values = new Dictionary<ulong, int>();

    public void add(Thread_Light light, int value)
    {
        lights.Add(light.id, light);
        values.Add(light.id, value);
    }

    public bool contains(Thread_Light light) {
        return lights.ContainsKey(light.id);
    }

    public void remove(Thread_Light light)
    {
        lights.Remove(light.id);
        values.Remove(light.id);
    }

    public bool empty()
    {
        return (lights.Count == 0);
    }

    public void add_recalculates(Dictionary<ulong, Thread_Light> recalculate)
    {
        foreach (KeyValuePair<ulong, Thread_Light> entry in lights)
        {
            if (!recalculate.ContainsKey(entry.Key))
                recalculate.Add(entry.Key, entry.Value);
        }
    } 

    public int value()
    {
        int v = 0;
        foreach (KeyValuePair<ulong, int> entry in values)
        {
            v += entry.Value;
        }
        if (v > 255)
            v = 255;

        return v;
    }
}
