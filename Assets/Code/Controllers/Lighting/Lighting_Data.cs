
using System.Collections.Generic;
using UnityEngine;

public class Lighting_Data  {

	public int width { get; protected set; }
    public int length { get; protected set; }
    public int height { get; protected set; }
    public int tiles_per_layer { get; protected set; }

    Thread_IsoObject[,,] objects_field;
    Lighting_Bucket[,,] buckets;
    public int[,,] solar_field { get; protected set; }
    Dictionary<ulong, Thread_IsoObject> level_altered;

    public Lighting_Data(int width, int length, int height)
    {
        this.width = width;
        this.length = length;
        this.height = height;
        tiles_per_layer = width * length;

        objects_field = new Thread_IsoObject[width, length, height];
        buckets = new Lighting_Bucket[width, length, height];
        solar_field = new int[width, length, height];

    }

    /// <summary>
    /// Get the average light level of the object
    /// </summary>
    public int object_coverage(Thread_IsoObject ob)
    {
        float v = 0;
        foreach(Iso i in ob.coords)
        {
            v += get_bucket_value(i);
        }
        v /= ob.coords.Count;

        return (int)v;
    }

    public void add_recalculates(Thread_IsoObject ob, Dictionary<ulong, Thread_Light> recalculate)
    {
        foreach(Iso i in ob.coords)
        {
            if (buckets[i.x, i.y, i.z] != null)
                buckets[i.x, i.y, i.z].add_recalculates(recalculate);
        }
    }

    /// <summary>
    /// Get the total light level of the bucked
    /// </summary>
    public int get_bucket_value(Iso i)
    {
        if (buckets[i.x, i.y, i.z] == null)
            return 0;

        return buckets[i.x, i.y, i.z].value();
    }

    /// <summary>
    /// Add a coverage point
    /// </summary>
    public void add_coverage(Thread_Light light, Iso i, int value)
    {
        if (buckets[i.x, i.y, i.z] == null)
            buckets[i.x, i.y, i.z] = new Lighting_Bucket();

        buckets[i.x, i.y, i.z].add(light, value);

        light.coverage.Add(i);
        alter_level_on_coord(i);
    }

    /// <summary>
    /// Completely remove coverage
    /// </summary>
    /// <param name="light"></param>
    public void remove_coverage(Thread_Light light)
    {
        foreach(Iso i in light.coverage)
        {
            buckets[i.x, i.y, i.z].remove(light);
            if (buckets[i.x, i.y, i.z].empty())
                buckets[i.x, i.y, i.z] = null;
            alter_level_on_coord(i);
        }

        light.coverage = new List<Iso>();
    }

    /// <summary>
    /// Resets once per call data structures
    /// </summary>
    public void flush()
    {
        level_altered = new Dictionary<ulong, Thread_IsoObject>();
    }

    /// <summary>
    /// Adds light blocking tiles to field
    /// </summary>
    public void add_to_field(Thread_IsoObject ob)
    {
        //TODO
        foreach(Iso i in ob.coords)
        {
            objects_field[i.x, i.y, i.z] = ob;
        }
    }

    /// <summary>
    /// Removes ligth blocking from field
    /// </summary>
    public void remove_from_field(Thread_IsoObject ob)
    {
        //TODO
        foreach (Iso i in ob.coords)
        {
            objects_field[i.x, i.y, i.z] = null;
        }
    }

    /// <summary>
    /// Check if an light blocking tile is present
    /// </summary>
    public bool check(Iso i)
    {
        return (objects_field[i.x, i.y, i.z] != null);
    }

    /// <summary>
    /// Check if an light blocking tile is present
    /// </summary>
    public bool check(int x, int y, int z)
    {
        return (objects_field[x, y, z] != null);
    }

    /// <summary>
    /// Add object for color recalculation
    /// </summary>
    public void add_to_level_altered(Thread_IsoObject ob)
    {
        if (!level_altered.ContainsKey(ob.id))
            level_altered.Add(ob.id, ob);
    }

    /// <summary>
    /// Add object, if exists, for color recalculation
    /// </summary>
    public void alter_level_on_coord(Iso i)
    {
        Thread_IsoObject ob;
        if (tryGetObject(i, out ob))
            add_to_level_altered(ob);

    }

    /// <summary>
    /// Try to get object at coord
    /// </summary>
    public bool tryGetObject(Iso i, out Thread_IsoObject ob)
    {
        ob = null;
        if (!check(i))
            return false;

        ob = objects_field[i.x, i.y, i.z];
        return true;
    }

    public int get_solar(Iso i,float v)
    {
        return (int)(v * solar_field[i.x, i.y, i.z]);
    }

    public Dictionary<ulong, Thread_IsoObject> getLevelAltered()
    {
        return level_altered;
    }
}
