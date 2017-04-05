using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorLightingControl : LightingControl
{
    Dictionary<ulong, IsoObject> objects_added = new Dictionary<ulong, IsoObject>(); //Does anything need to be stored internally?
    Dictionary<ulong, IsoObject> objects_removed = new Dictionary<ulong, IsoObject>();
    Dictionary<ulong, Iso_Light> lights_added = new Dictionary<ulong, Iso_Light>();
    Dictionary<ulong, Iso_Light> lights_removed = new Dictionary<ulong, Iso_Light>();

    LightingThread thread;

    public EditorLightingControl() : base()
    {

        IsoObject.registerOnCreate(onIsoObjectCreate);
        IsoObject.registerOnDestroy(onIsoObjectDestroy);
        Iso_Light.registerOnCreate(onIso_LightCreate);
        Iso_Light.registerOnDestroy(onIso_LightDestroy);

        thread = new LightingThread(16);
    }

    void onIsoObjectCreate(IsoObject i)
    {
        objects_added.Add(i.id, i);
    }

    void onIsoObjectDestroy(IsoObject i)
    {
        if (objects_added.ContainsKey(i.id)) // if Added since last update remove from tread update list
            objects_added.Remove(i.id);
        else
            objects_removed.Add(i.id, i);
    }


    void onIso_LightCreate(Iso_Light i)
    {
        lights_added.Add(i.light_id, i);
        //Debug.Log("Added light");
    }

    void onIso_LightDestroy(Iso_Light i)
    {
        if (lights_added.ContainsKey(i.light_id))
            lights_added.Remove(i.light_id);
        else
            lights_removed.Add(i.light_id, i);
        //Debug.Log("Removed light");
    }


    public override void update()
    {
        //
        if (!thread.running)
        {
            //Assume it shoul always be running
            if (thread.hasjobs)
            {
                execute_jobs(thread.jobs);
                thread.hasjobs = false;
            }
            
            if (objects_added.Count != 0 || objects_removed.Count != 0 || lights_added.Count != 0 || lights_removed.Count != 0)
            {
                thread.runOnThread(objects_added, objects_removed, lights_added, lights_removed);
                objects_added = new Dictionary<ulong, IsoObject>();
                objects_removed = new Dictionary<ulong, IsoObject>();
                lights_added = new Dictionary<ulong, Iso_Light>();
                lights_removed = new Dictionary<ulong, Iso_Light>();
            } 
        }
    }

    public override void runOnMainThread()
    {
        thread.runOnMain(objects_added, objects_removed, lights_added, lights_removed);
        objects_added = new Dictionary<ulong, IsoObject>();
        objects_removed = new Dictionary<ulong, IsoObject>();
        lights_added = new Dictionary<ulong, Iso_Light>();
        lights_removed = new Dictionary<ulong, Iso_Light>();

        execute_jobs(thread.jobs);
    }

    void execute_jobs(List<Thread_Job> jobs)
    {
        Tile t;
        foreach (Thread_Job j in jobs)
        {
            if (LogicControl.main.exists(j.coord))
            {
                t = LogicControl.main.get(j.coord);

                for(int i=0; i<t.isoObject.coords.Count; i++)
                {
                    if (t.isoObject.getSprite(i) != null)
                        LogicControl.main.get(t.isoObject.coords[i]).graphic.GetComponent<SpriteRenderer>().color = j.color;
                }
            }
        }
    }

    protected override void destructor()
    {
        IsoObject.removeOnCreate(onIsoObjectCreate);
        IsoObject.removeOnDestroy(onIsoObjectDestroy);
        Iso_Light.removeOnCreate(onIso_LightCreate);
        Iso_Light.removeOnDestroy(onIso_LightDestroy);
    }

    public override void delayedConstruction()
    {
        runOnMainThread();
    }
}
