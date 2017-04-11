
using System.Collections.Generic;
using UnityEngine;

public class Flood_Light_v2 {
    Lighting_Data data;

    Queue<Iso> targets = new Queue<Iso>();
    //Dictionary<int, Thread_IsoObject> level_altered;
    //Dictionary<int, Thread_IsoObject> objects;

    int radius, count = 0;
    int[,,] grid;
    int w, l, h;
    int ux, lx, uy, ly, uz, lz; //Bouds
    Iso origin;
    //bool[,,] map;
    Thread_Light light;
    float p; //propegation constant



    public Flood_Light_v2(Thread_Light light, Lighting_Data data)
    {

        this.light = light;
        //this.map = map;
        //this.objects = objects;
        //this.level_altered = level_altered;
        this.data = data;

        radius = light.radius;
        origin = light.coord;

        p = Mathf.Pow(255f/10f, 1f / radius);


        ux = light.coord.x + radius;
        lx = light.coord.x - radius;
        uy = light.coord.y + radius;
        ly = light.coord.y - radius;
        uz = light.coord.z + radius;
        lz = light.coord.z - radius;

        //Check if we are not ouside the normal map
        boxin();


        int v;
        Iso t;


        process(origin, 10);
        //Dry run
        t = targets.Dequeue();
        v = (int)(get(t) * p);
        process(new Iso(t.x + 1, t.y, t.z), v);//+x
        process(new Iso(t.x - 1, t.y, t.z), v);//-x
        process(new Iso(t.x, t.y + 1, t.z), v);//+y
        process(new Iso(t.x, t.y - 1, t.z), v);//-y
        process(new Iso(t.x, t.y, t.z + 1), v);//Up
        process(new Iso(t.x, t.y, t.z - 1), v);//Down
        count++;

        while (targets.Count > 0) //Main loop
        {
            t = targets.Dequeue();
            v = (int)(get(t) * p);
            count++;
            if (data.check(t))
                v = (100+ v);
            if (v < 255)//Skip too dim tiles adn occpied tiles
            {

                process(new Iso(t.x + 1, t.y, t.z), v);//+x
                process(new Iso(t.x - 1, t.y, t.z), v);//-x
                process(new Iso(t.x, t.y + 1, t.z), v);//+y
                process(new Iso(t.x, t.y - 1, t.z), v);//-y
                process(new Iso(t.x, t.y, t.z + 1), v);//Up
                process(new Iso(t.x, t.y, t.z - 1), v);//Down
            }

        }
        //Log(ToString());
        reverseValues();
    }

    void process(Iso i, int v)
    {
        if (!inGrid(i))
            return;
        int c = get(i);
        if (c == 0 || c > v) //Only proceed if this write is lighter or object has not been written to
        {

            targets.Enqueue(i);
            set(i, v);
        }
    }

    void reverseValues()
    {
        //Loop trough all cells

        Iso pos; 

        for (int i= lx; i<= ux; i++)
        {
            for (int j = ly; j <= uy; j++)
            {
                for (int k = lz; k <= uz; k++)
                {
                    pos = new Iso(i, j, k);
                    if (get(pos) != 0) //Lit tile register
                    {
                        //register(pos, 255-get(pos));
                        data.add_coverage(light, pos, 255 - get(pos));
                    }
                }
            }
        }
    }

    /*void register(Iso i, int v)
    {
        //Register into light if isoobject is present
        Thread_IsoObject ob;
        data.add_coverage(light, i, v);
        if (data.tryGetObject(i, out ob)) //Check if isoobject is here
        {   //Register object changes
            ob.value += v;
            data.add_to_level_altered(ob);


            light.coverage.Add(ob.hash, ob);    //Register light coverage
            light.coverage_value.Add(ob.hash, v);
            

            if (!ob.coverdBy.ContainsKey(light.id))
                ob.coverdBy.Add(light.id, light);
            else
                Log("Adding light to object already covered?");


        }
    }*/

    /// <summary>
    /// Builds boundry box
    /// </summary>
    void boxin()
    {
        if (lx < 0)
            lx = 0;
        if (ux >= data.width)
            ux = data.width - 1;
        if (ly < 0)
            ly = 0;
        if (uy >= data.length)
            uy = data.length - 1;
        if (lz < 0)
            lz = 0;
        if (uz >= data.height)
            uz = data.height - 1;

        w = ux - lx + 1;
        l = uy - ly + 1;
        h = uz - lz + 1;
        grid = new int[w, l, h];


    }

    void set(Iso i, int value)
    {
        grid[i.x - lx, i.y - ly, i.z - lz] = value;
    }

    int get(Iso i)
    {
        return grid[i.x - lx, i.y - ly, i.z - lz];
    }

    bool inGrid(Iso i)
    {
        if (i.x < lx || i.x > ux)
            return false;
        if (i.y < ly || i.y > uy)
            return false;
        if (i.z < lz || i.z > uz)
            return false;
        return true;
    }

    /// <summary>
    /// The hashing code
    /// </summary>
    /*int hashBox(Iso i)
    {
        return (i.z * layer) + (i.y * w) + i.x;
    }*/

    void Log(string m)
    {
        //LightingControl.main.print_queue.Enqueue(m);
        Debug.Log(m);
    }

    public override string ToString()
    {
        return "Flood_light<size[" + w + "," + h + "," + l + "],  targets " + targets.Count + ", bounds[" + lx + "," + ux + "," + ly + "," + uy + "," + lz + "," + uz + "] count[" + count + "/" + w * h * l + "]>";
    }
}
