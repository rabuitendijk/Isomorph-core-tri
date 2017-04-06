
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class LightingThread  {

    public bool running = false, hasjobs = false;
    public List<Thread_Job> jobs;
    int quantisation; //Number of shading levels
    int width, height, length;

    //Input for controller
    Dictionary<ulong, IsoObject> objects_added;
    Dictionary<ulong, IsoObject> objects_removed;
    Dictionary<ulong, Iso_Light> lights_added;
    Dictionary<ulong, Iso_Light> lights_removed;

    //Threaded interpitation of map
    bool[,,] grid;
    int[,,] values;
    Dictionary<int, Thread_IsoObject> objects = new Dictionary<int, Thread_IsoObject>();            //Maps coordinates to Threads objects
    Dictionary<ulong, Thread_IsoObject> objects_map = new Dictionary<ulong, Thread_IsoObject>();    //Maps IsoObject id to thread object
    Dictionary<ulong, Thread_Light> lights = new Dictionary<ulong, Thread_Light>();                 //Maps thread lights to IsoLight 

    //Jobs to process
    Dictionary<ulong, Thread_Light> create;
    Dictionary<ulong, Thread_Light> destroy;
    Dictionary<ulong, Thread_Light> recalculate;
    Dictionary<int, Thread_IsoObject> level_altered;

    int layer;
    int ambiant_value = 60;

    public LightingThread(int quantisation)
    {
        this.quantisation = quantisation;
        width = LogicControl.main.width;
        length = LogicControl.main.length;
        height = LogicControl.main.height;
        grid = new bool[width, length, height];
        values = new int[width, length, height];
        layer = width * height;
    }

    public void runOnThread(Dictionary<ulong, IsoObject> objects_added, Dictionary<ulong, IsoObject> objects_removed, Dictionary<ulong, Iso_Light> lights_added, Dictionary<ulong, Iso_Light> lights_removed)
    {
        running = true;
        this.objects_added = objects_added;
        this.objects_removed = objects_removed;
        this.lights_added = lights_added;
        this.lights_removed = lights_removed;
        Log("Starting thread");

        //TODO
        Thread myTread = new Thread (thread_process);
        myTread.Start();
        //return jobs;
    }

    public void runOnMain(Dictionary<ulong, IsoObject> objects_added, Dictionary<ulong, IsoObject> objects_removed, Dictionary<ulong, Iso_Light> lights_added, Dictionary<ulong, Iso_Light> lights_removed)
    {
        running = true;
        this.objects_added = objects_added;
        this.objects_removed = objects_removed;
        this.lights_added = lights_added;
        this.lights_removed = lights_removed;

        main_process();
        //return jobs;
    }

    /// <summary>
    /// Threaded version
    /// </summary>
    void thread_process()
    {
        process();
        calculate_changes();
        running = false;
        hasjobs = true;
        Log("Returning thread");
    }

    /// <summary>
    /// Main startup version
    /// </summary>
    void main_process()
    {
        process();
        calculate_changes();
        running = false;
    }

    void process()
    {
        jobs = new List<Thread_Job>();
        convert();

        destroy_lights();
        add_lights();
        recalculate_lights();
        
    }

    /// <summary>
    /// Convert provided objects into Thread packages
    /// </summary>
    void convert()
    {
        //Log("Reached convert");

        create = new Dictionary<ulong, Thread_Light>();
        destroy = new Dictionary<ulong, Thread_Light>();
        recalculate = new Dictionary<ulong, Thread_Light>();
        level_altered = new Dictionary<int, Thread_IsoObject>();

        foreach (KeyValuePair<ulong, IsoObject> entry in objects_added)
        {
            Thread_IsoObject ob = set(entry.Value);
            
            if (!level_altered.ContainsKey(ob.hash))
                level_altered.Add(ob.hash, ob);

            //Lightfield recalculation
            foreach (KeyValuePair<ulong, Thread_Light> light  in lights)
            {
                if (inBouds(ob.origin, light.Value.radius, light.Value.coord))
                    if (!recalculate.ContainsKey(light.Key))
                        recalculate.Add(light.Key, light.Value);
            }

        }

        foreach (KeyValuePair<ulong, IsoObject> entry in objects_removed)
        {
            remove(entry.Value);
        }

        foreach (KeyValuePair<ulong, Iso_Light> entry in lights_added)
        {
            create.Add(entry.Key, new Thread_Light(entry.Key, entry.Value.radius, entry.Value.source.origin));
        }


        foreach (KeyValuePair<ulong, Iso_Light> entry in lights_removed)
        {
            Thread_Light l;
            if (!lights.TryGetValue(entry.Key, out l))
            {
                Log("Lighting tread, Light id not in map when removing.");
                return;
            }

            destroy.Add(entry.Key, l);
        }

        //Input can be forgotten
        objects_added = null;
        objects_removed = null;
        lights_added = null;
        lights_removed = null;
    }

    /// <summary>
    /// Sets new Thread IsoObject
    /// </summary>
    Thread_IsoObject set(IsoObject o)
    {
        foreach(Iso i in o.coords)
        {
            set(i, true);
        }
        Thread_IsoObject ob = new Thread_IsoObject(o, hashBox(o.origin));
        objects.Add(ob.hash, ob);
        objects_map.Add(o.id, ob);

        return ob;
    }

    /// <summary>
    /// Removes thres isoobject
    /// </summary>
    void remove(IsoObject o)
    {
        foreach(Iso i in o.coords)
        {
            set(i, false);
        }
        Thread_IsoObject ob;
        if (!objects_map.TryGetValue(o.id, out ob))
        {
            Log("Lighting tread, remove isoobject, key does not exist in map.");
            return;
        }

        //Remove mapping
        objects_map.Remove(o.id);
        objects.Remove(ob.hash);

        //Add lights to be redrawn
        foreach (KeyValuePair<ulong, Thread_Light> entry in ob.coverdBy)
        {
            if (!recalculate.ContainsKey(entry.Key))
               recalculate.Add(entry.Key, entry.Value);
        }
    }

    void set(Iso i, bool value)
    {
        grid[i.x, i.y, i.z] = value;
    }

    /// <summary>
    /// Remove light contribution
    /// </summary>
    void destroy_lights()
    {
        //Log("destroy: "+destroy.Count.ToString());
        int value;
        foreach (KeyValuePair<ulong, Thread_Light> entry in destroy)
        {
            foreach (KeyValuePair<int, Thread_IsoObject> ob in entry.Value.coverage)
            {
                entry.Value.coverage_value.TryGetValue(ob.Key, out value);
                ob.Value.coverdBy.Remove(entry.Key);
                alterValue(ob.Value, -value);
            }
            //Dismiss content
            entry.Value.coverage = null;
            entry.Value.coverage_value = null;

            lights.Remove(entry.Key);
        }

    }

    void add_lights()
    {
        //Log("add: " + create.Count.ToString());
        foreach (KeyValuePair<ulong, Thread_Light> entry in create)
        {
            lights.Add(entry.Key, entry.Value);
            //Build light

            new Flood_Light(entry.Value, grid, width, length, height, level_altered, objects);
        }
    }


    void recalculate_lights()
    {
        //Log("recalculate: " + recalculate.Count.ToString());
        int value;
        foreach (KeyValuePair<ulong, Thread_Light> entry in recalculate)
        {
            //Log("Recalculate count "+recalculate.Count.ToString());
            if (lights.ContainsKey(entry.Key))  //If light has been destroyed by by object destruction
            {
                //Cleanup
                foreach (KeyValuePair<int, Thread_IsoObject> ob in entry.Value.coverage)
                {
                    entry.Value.coverage_value.TryGetValue(ob.Key, out value);
                    ob.Value.coverdBy.Remove(entry.Key);
                    alterValue(ob.Value, -value);
                }
                //Dismiss content
                entry.Value.coverage = new Dictionary<int, Thread_IsoObject>();
                entry.Value.coverage_value = new Dictionary<int, int>();

                //Build light
                new Flood_Light(entry.Value, grid, width, length, height, level_altered, objects);
            }
        }
    }

    /// <summary>
    /// Processes the final job list
    /// </summary>
    void calculate_changes()
    {
        int level;
        float c;
        foreach (KeyValuePair<int, Thread_IsoObject> entry in level_altered)
        {
            level = (getValue(entry.Value.origin)+entry.Value.value + ambiant_value) / (255 / quantisation);
            if (level != entry.Value.level)
            {
                c = level * 1f / quantisation;
                jobs.Add(new Thread_Job(entry.Value.origin, new Color(c,c,c)));
            }

        }
    }

    int getValue(Iso i)
    {
        return values[i.x, i.y, i.z];
    }

    /// <summary>
    /// The hashing code
    /// </summary>
    int hashBox(Iso i)
    {
        return (i.z * layer) + (i.y * width) + i.x;
    }

    /// <summary>
    /// Alter light value and register object for recalculation
    /// </summary>
    void alterValue(Thread_IsoObject ob, int value)
    {
        ob.value += value;
        if (!level_altered.ContainsKey(ob.hash))
            level_altered.Add(ob.hash, ob);
    }

    /// <summary>
    /// Check if pion is in bouds of light
    /// </summary>
    bool inBouds(Iso origin, int radius, Iso target)
    {
        if (target.x < origin.x - radius || target.x > origin.x + radius)
            return false;

        if (target.y < origin.y - radius || target.y > origin.y + radius)
            return false;

        if (target.z < origin.z - radius || target.z > origin.z + radius)
            return false;

        return true;
    }

    void Log(string m)
    {
        //LightingControl.main.print_queue.Enqueue(m);
        Debug.Log(m);
    }
}
