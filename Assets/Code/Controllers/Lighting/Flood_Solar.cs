using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flood_Solar {
    Lighting_Data data;

    int[,,] levels;
    int count=0;


    int  p; //propegation constant

    public Flood_Solar(Lighting_Data data, int radius)
    {
        this.data = data;
        levels = data.solar_field;

        //p = Mathf.Pow(10f/ 255f, 1f / (radius));
        p = (255 - 20) / radius;
    }

    public void flood(Thread_Solar_Job job)
    {
        count = 0;
        //Todo
        Queue<Iso> targets = new Queue<Iso>();
        

        //Rain down and clear
        for (int i = job.lx; i <= job.ux; i++)
        {
            for (int j = job.ly; j <= job.uy; j++)
            {
                rain(i, j);
            }
        }

        HashSet<int> checks = new HashSet<int>();
        //Feths initail update list
        for (int i = job.lx; i <= job.ux; i++)
        {
            for (int j = job.ly; j <= job.uy; j++)
            {
                for (int k = data.height - 1; k >= 0; k--)
                {

                    if (levels[i,j,k] == 0)
                    {
                        //Add new sources if inGrid and v>0
                        
                        tilecheck(new Iso(i - 1, j, k), targets, checks, job);
                        tilecheck(new Iso(i + 1, j, k), targets, checks, job);
                        tilecheck(new Iso(i, j - 1, k), targets, checks, job);
                        tilecheck(new Iso(i, j + 1, k), targets, checks, job);
                        tilecheck(new Iso(i , j, k-1), targets, checks, job);
                        tilecheck(new Iso(i , j, k+1), targets, checks, job);
                    }
                }
            }
        }
        checks = null;

        //Debug.Log(targets.Count);
        //Propegation
        Iso t;
        int v;
        while (targets.Count > 0) //Main loop
        {
            t = targets.Dequeue();
            v = (int)(get(t) - p);
            count++;
            if (!data.check(t))
            {
                if (v > 3)//Skip too dim tiles adn occpied tiles
                {

                    process(new Iso(t.x + 1, t.y, t.z), v, job, targets);//+x
                    process(new Iso(t.x - 1, t.y, t.z), v, job, targets);//-x
                    process(new Iso(t.x, t.y + 1, t.z), v, job, targets);//+y
                    process(new Iso(t.x, t.y - 1, t.z), v, job, targets);//-y
                    process(new Iso(t.x, t.y, t.z + 1), v, job, targets);//Up
                    process(new Iso(t.x, t.y, t.z - 1), v, job, targets);//Down
                }
            }


        }

        //Reverse
        /*
        for (int i = job.lx; i <= job.ux; i++)
        {
            for (int j = job.ly; j <= job.uy; j++)
            {
                for (int k = data.height - 1; k >= 0; k--)
                {
                    v = levels[i, j, k];
                    if (v != 0) //dead cells do not get added lighting
                        levels[i, j, k] = 256 - v;//+1 correction
                }
            }
        }
        */
        //Debug.Log(this);
    }

    void tilecheck(Iso i, Queue<Iso> targets, HashSet<int> set, Thread_Solar_Job job)
    {
        if (!inGrid(i))
            return;

        int v = get(i);

        if (v == 0) //Dim or unset tile
            return;

        /*
        if (!inGrid(i, job))//Outside read light reversed
        {
            v = 255 - v;
            if (v <= 0)
                v = 1;
        }
        */



        if (set.Contains(hashBox(i)))
            return;

        targets.Enqueue(i);
        set.Add(hashBox(i));
        
    }

    /// <summary>
    /// Sets 
    /// </summary>
    void rain(int x, int y)
    {
        bool raining = true;
        for (int k = data.height - 1; k >= 0; k--)
        {


            if (raining)
                levels[x, y, k] = 255;
            else
                levels[x, y, k] = 0;

            if (data.check(x, y, k)) //When an object is encouterd, stop setting light values to lit [1]
                raining = false;

            data.alter_level_on_coord(new Iso(x,y,k));

        }
    }

    void process(Iso i, int v, Thread_Solar_Job job, Queue<Iso> targets)
    {
        if (!inGrid(i,job))
            return;
        int c = get(i);
        if (c == 0 || c < v) //Only proceed if this write is lighter or object has not been written to
        {

            targets.Enqueue(i);
            levels[i.x, i.y, i.z] = v;
        }
    }

    bool inGrid(Iso i)
    {

        if (i.x < 0 || i.x >= data.width)
            return false;
        if (i.y < 0 || i.y >= data.length)
            return false;
        if (i.z < 0 || i.z >= data.height)
            return false;
        return true;
        
    }

    bool inGrid(Iso i, Thread_Solar_Job job)
    {
        if (i.z < 0 || i.z >= data.height)
            return false;
        return job.inBounds(i);
    }

    int get(Iso i)
    {
        return levels[i.x, i.y, i.z];
    }


    int hashBox(Iso i)
    {
        return (i.z * data.tiles_per_layer) + (i.y * data.width) + i.x;
    }

    public override string ToString()
    {
        return "Flood_Solar<Count : "+count+">";
    }

}
