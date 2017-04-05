
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class LightingThread  {

    public bool running = false, hasjobs = false;
    public List<Thread_Job> jobs;
    int quantisation; //Number of shading levels

    //Input for controller
    Dictionary<ulong, IsoObject> objects_added;
    Dictionary<ulong, IsoObject> objects_removed;
    Dictionary<ulong, Iso_Light> lights_added;
    Dictionary<ulong, Iso_Light> lights_removed;

    //Threaded interpitation of map
    bool[,,] grid;
    Dictionary<int, Thread_IsoObject> objects = new Dictionary<int, Thread_IsoObject>();            //Maps coordinates to Threads objects
    Dictionary<ulong, Thread_IsoObject> objects_map = new Dictionary<ulong, Thread_IsoObject>();    //Maps IsoObject id to thread object
    Dictionary<ulong, Thread_Light> lights = new Dictionary<ulong, Thread_Light>();                 //Maps thread lights to IsoLight 

    //Jobs to process
    Dictionary<ulong, Thread_Light> create;
    Dictionary<ulong, Thread_Light> destroy;
    Dictionary<ulong, Thread_Light> recalculate;
    Dictionary<int, Thread_IsoObject> level_altered;

    int layer;

    public LightingThread(int quantisation)
    {
        this.quantisation = quantisation;
        grid = new bool[LogicControl.main.width, LogicControl.main.length, LogicControl.main.height];
        layer = LogicControl.main.width * LogicControl.main.height;
    }

    public void runOnThread(Dictionary<ulong, IsoObject> objects_added, Dictionary<ulong, IsoObject> objects_removed, Dictionary<ulong, Iso_Light> lights_added, Dictionary<ulong, Iso_Light> lights_removed)
    {
        running = true;
        this.objects_added = objects_added;
        this.objects_removed = objects_removed;
        this.lights_added = lights_added;
        this.lights_removed = lights_removed;
        Debug.Log("Starting thread");

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
        Debug.Log("Returning thread");
    }

    /// <summary>
    /// Main startup version
    /// </summary>
    void main_process()
    {
        process();
        calculate_changes_all();
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
        create = new Dictionary<ulong, Thread_Light>();
        destroy = new Dictionary<ulong, Thread_Light>();
        recalculate = new Dictionary<ulong, Thread_Light>();
        level_altered = new Dictionary<int, Thread_IsoObject>();

        foreach (KeyValuePair<ulong, IsoObject> entry in objects_added)
        {
            set(entry.Value);
            //Lightfield recalculation
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
                Debug.Log("Lighting tread, Light id not in map when removing.");
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
    void set(IsoObject o)
    {
        foreach(Iso i in o.coords)
        {
            set(i, true);
        }
        Thread_IsoObject ob = new Thread_IsoObject(o, hashBox(o.origin));
        objects.Add(ob.hash, ob);
        objects_map.Add(o.id, ob);
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
            Debug.Log("Lighting tread, remove isoobject, key does not exist in map.");
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
        int value;
        foreach (KeyValuePair<ulong, Thread_Light> entry in destroy)
        {
            foreach (KeyValuePair<int, Thread_IsoObject> ob in entry.Value.coverage)
            {
                entry.Value.coverage_value.TryGetValue(ob.Key, out value);
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
        foreach (KeyValuePair<ulong, Thread_Light> entry in create)
        {
            //Build light
        }
    }


    void recalculate_lights()
    {
        int value;
        foreach (KeyValuePair<ulong, Thread_Light> entry in recalculate)
        {
            //Cleanup
            foreach (KeyValuePair<int, Thread_IsoObject> ob in entry.Value.coverage)
            {
                entry.Value.coverage_value.TryGetValue(ob.Key, out value);
                alterValue(ob.Value, -value);
            }
            //Dismiss content
            entry.Value.coverage = new Dictionary<int, Thread_IsoObject>();
            entry.Value.coverage_value = new Dictionary<int, int>();

            //Build light
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
            level = entry.Value.value / quantisation;
            if (level != entry.Value.level)
            {
                c = level * 1f / quantisation;
                jobs.Add(new Thread_Job(entry.Value.origin, new Color(c,c,c)));
            }

        }
    }

    void calculate_changes_all()
    {
        int level;
        float c;
        foreach (KeyValuePair<int, Thread_IsoObject> entry in objects)
        {
            level = entry.Value.value / quantisation;
            if (level != entry.Value.level)
            {
                c = level * 1f / quantisation;
                jobs.Add(new Thread_Job(entry.Value.origin, new Color(c, c, c)));
            }

        }
    }

    /// <summary>
    /// The hashing code
    /// </summary>
    int hashBox(Iso i)
    {
        return (i.z * layer) + (i.y * LogicControl.main.width) + i.x;
    }

    void alterValue(Thread_IsoObject ob, int value)
    {
        ob.value += value;
        if (!level_altered.ContainsKey(ob.hash))
            level_altered.Add(ob.hash, ob);
    }
}
