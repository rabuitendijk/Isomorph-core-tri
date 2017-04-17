
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class LightingThread  {
    Lighting_Data data;

    public bool running = false, hasjobs = false;
    public List<Thread_Job> jobs;
    int quantisation; //Number of shading levels
    public bool yielding { get { return data.yielding; } }

    //Input for controller
    Dictionary<ulong, IsoObject> objects_added;
    Dictionary<ulong, IsoObject> objects_removed;
    Dictionary<ulong, Iso_Light> lights_added;
    Dictionary<ulong, Iso_Light> lights_removed;

    //Threaded interpitation of map
    Dictionary<ulong, Thread_IsoObject> objects_map = new Dictionary<ulong, Thread_IsoObject>();    //Maps IsoObject id to thread object
    Dictionary<ulong, Thread_Light> lights = new Dictionary<ulong, Thread_Light>();                 //Maps thread lights to IsoLight 

    //Jobs to process
    Dictionary<ulong, Thread_Light> create;
    Dictionary<ulong, Thread_Light> destroy;
    Dictionary<ulong, Thread_Light> recalculate;
    List<Thread_Solar_Job> solar_jobs;

    Flood_Solar solar;

    //int layer;
    int ambiant_value = 0;
    int solar_radius = 5;
    float solar_influence = .4f;

    public LightingThread(int quantisation)
    {
        this.quantisation = quantisation;

        data = new Lighting_Data(LogicControl.main.width, LogicControl.main.length, LogicControl.main.height);
        solar = new Flood_Solar(data, solar_radius);
    }

    public void runOnThread(Dictionary<ulong, IsoObject> objects_added, Dictionary<ulong, IsoObject> objects_removed, Dictionary<ulong, Iso_Light> lights_added, Dictionary<ulong, Iso_Light> lights_removed)
    {
        running = true;
        this.objects_added = objects_added;
        this.objects_removed = objects_removed;
        this.lights_added = lights_added;
        this.lights_removed = lights_removed;
        //Log("Starting thread");

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
    /// Starts yielding to next solar state
    /// </summary>
    public void change_solar_value(float v)
    {
        solar_influence = v;
        data.change_solar_level();
    }

    /// <summary>
    /// Threaded version
    /// </summary>
    void thread_process()
    {
        try {
            process();
            calculate_changes();
            running = false;
            hasjobs = true;
            //Log("Returning thread");
        }
        catch (Exception e)
        {
            Log("Lighting engine broke: "+e.ToString());
        }
    }

    /// <summary>
    /// Main startup version
    /// </summary>
    void main_process()
    {
        process(true);
        calculate_changes();
        running = false;
    }

    void process(bool full_solar_recalculation = false)
    {
        jobs = new List<Thread_Job>();

        solar_jobs = new List<Thread_Solar_Job>();
        if (full_solar_recalculation)
            solar_jobs.Add(new Thread_Solar_Job(data.width, data.length));

        convert();
        
        destroy_lights();
        add_lights();
        recalculate_lights();
        data.go_yield();
        process_solar_jobs();
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
        data.flush();

        foreach (KeyValuePair<ulong, IsoObject> entry in objects_added)
        {
            Thread_IsoObject ob = set(entry.Value);

            data.add_to_level_altered(ob);

            //Lightfield recalculation
            foreach (KeyValuePair<ulong, Thread_Light> light  in lights)
            {
                if (inBouds(ob.origin, light.Value.radius, light.Value.coord))
                    if (!recalculate.ContainsKey(light.Key))
                        recalculate.Add(light.Key, light.Value);
            }
            solar_jobs.Add(new Thread_Solar_Job(ob.origin, solar_radius, data.width, data.length));
        }

        foreach (KeyValuePair<ulong, IsoObject> entry in objects_removed)
        {
            remove(entry.Value);
            solar_jobs.Add(new Thread_Solar_Job(entry.Value.origin, solar_radius, data.width, data.length));
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
        Thread_IsoObject ob = new Thread_IsoObject(o);
        objects_map.Add(o.id, ob);
        data.add_to_field(ob);

        return ob;
    }

    /// <summary>
    /// Removes thres isoobject
    /// </summary>
    void remove(IsoObject o)
    {
        
        Thread_IsoObject ob;
        if (!objects_map.TryGetValue(o.id, out ob))
        {
            Log("Lighting tread, remove isoobject, key does not exist in map.");
            return;
        }

        //Remove mapping
        objects_map.Remove(o.id);
        data.remove_from_field(ob);

        //Add lights to be redrawn
        data.add_recalculates(ob, recalculate);
    }

    /// <summary>
    /// Remove light contribution
    /// </summary>
    void destroy_lights()
    {
        //Log("destroy: "+destroy.Count.ToString());
        foreach (KeyValuePair<ulong, Thread_Light> entry in destroy)
        {
            data.remove_coverage(entry.Value);

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

            new Flood_Light_v2(entry.Value, data);
        }
    }


    void recalculate_lights()
    {
        //Log("recalculate: " + recalculate.Count.ToString());
        foreach (KeyValuePair<ulong, Thread_Light> entry in recalculate)
        {
            //Log("Recalculate count "+recalculate.Count.ToString());
            if (lights.ContainsKey(entry.Key))  //If light has been destroyed by by object destruction
            {
                data.remove_coverage(entry.Value);

                //Build light
                new Flood_Light_v2(entry.Value, data);
            }
        }
    }

    void process_solar_jobs()
    {
        foreach(Thread_Solar_Job job in solar_jobs)
        {
            solar.flood(job);

            if (job.full)   //Stop futher recalculation on full recalc
                return;
        }
    }

    /// <summary>
    /// Processes the final job list
    /// </summary>
    void calculate_changes()
    {
        int level;
        float c, mu;

        foreach (KeyValuePair<ulong, Thread_IsoObject> entry in data.getLevelAltered())
        {
            level = (data.object_coverage(entry.Value, solar_influence) + ambiant_value);
            mu = ((float)level* (float)quantisation) / (255f);
            level = (int)mu;
            if (level != entry.Value.level)
            {
                c = level * 1f / quantisation;
                jobs.Add(new Thread_Job(entry.Value.origin, new Color(c,c,c)));
            }

        }
    }

    /// <summary>
    /// The hashing code
    /// </summary>
    int hashBox(Iso i)
    {
        return (i.z * data.tiles_per_layer) + (i.y * data.width) + i.x;
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
