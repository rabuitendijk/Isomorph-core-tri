using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flood_Solar {
    int[,,] levels;
    bool[,,] field;
    int width, height, length, layer;
    //int radius;
    int count=0;

    Dictionary<int, Thread_IsoObject> level_altered;
    Dictionary<int, Thread_IsoObject> objects;


    float p; //propegation constant

    public Flood_Solar(int[,,] levels, bool[,,] field, int width, int length, int height, Dictionary<int, Thread_IsoObject> objects, int radius)
    {
        this.levels = levels;
        this.field = field;
        this.width = width;
        this.length = length;
        this.height = height;

        this.objects = objects;
        //this.radius = radius;

        layer = width * length;
        p = Mathf.Pow(255f / 10f, 1f / radius);
    }

    public void flood(Thread_Solar_Job job, Dictionary<int, Thread_IsoObject> level_altered)
    {
        count = 0;
        //Todo
        this.level_altered = level_altered;
        Queue<Iso> targets = new Queue<Iso>();
        

        //Rain down and clear
        for (int i = job.lx; i <= job.ux; i++)
        {
            for (int j = job.ly; j <= job.uy; j++)
            {
                rain(i, j);
            }
        }

        Iso target;
        HashSet<int> checks = new HashSet<int>();
        //Feths initail update list
        for (int i = job.lx; i <= job.ux; i++)
        {
            for (int j = job.ly; j <= job.uy; j++)
            {
                for (int k = height - 1; k >= 0; k--)
                {

                    if (levels[i,j,k] == 0)
                    {
                        //Add new sources if inGrid and v>0
                        
                        target = new Iso(i-1,j,k);//-x
                        if (inGrid(target) && get(target) > 0 && !checks.Contains(hashBox(target)))
                        {
                            targets.Enqueue(target);
                            checks.Add(hashBox(target));
                        }
                        target = new Iso(i + 1, j, k);//+x
                        if (inGrid(target) && get(target) > 0 && !checks.Contains(hashBox(target)))
                        {
                            targets.Enqueue(target);
                            checks.Add(hashBox(target));
                        }

                        target = new Iso(i , j-1, k);//-y
                        if (inGrid(target) && get(target) > 0 && !checks.Contains(hashBox(target)))
                        {
                            targets.Enqueue(target);
                            checks.Add(hashBox(target));
                        }
                        target = new Iso(i , j+1, k);//+y
                        if (inGrid(target) && get(target) > 0 && !checks.Contains(hashBox(target)))
                        {
                            targets.Enqueue(target);
                            checks.Add(hashBox(target));
                        }

                        target = new Iso(i , j, k-1);//-z
                        if (inGrid(target) && get(target) > 0 && !checks.Contains(hashBox(target)))
                        {
                            targets.Enqueue(target);
                            checks.Add(hashBox(target));
                        }
                        target = new Iso(i , j, k+1);//+z
                        if (inGrid(target) && get(target) > 0 && !checks.Contains(hashBox(target)))
                        {
                            targets.Enqueue(target);
                            checks.Add(hashBox(target));
                        }
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
            v = (int)(get(t) * p);
            count++;
            if (!check(t))
            {
                if (v < 255)//Skip too dim tiles adn occpied tiles
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
        
        for (int i = job.lx; i <= job.ux; i++)
        {
            for (int j = job.ly; j <= job.uy; j++)
            {
                for (int k = height - 1; k >= 0; k--)
                {
                    v = levels[i, j, k];
                    if (v != 0) //dead cells do not get added lighting
                        levels[i, j, k] = 256 - v;//+1 correction
                }
            }
        }

        Debug.Log(this);
    }

    /// <summary>
    /// Sets 
    /// </summary>
    void rain(int x, int y)
    {
        int hash;
        bool raining = true;
        Thread_IsoObject ob;
        for (int k = height - 1; k >= 0; k--)
        {

            if (field[x, y, k]) //When an object is encouterd, stop setting light values to lit [1]
                raining = false;

            if (raining)
                levels[x, y, k] = 1;
            else
                levels[x, y, k] = 0;

            hash = hashBox(x, y, k);
            if (objects.TryGetValue(hash, out ob))  //If object exists
            {
                if (!level_altered.ContainsKey(hash))   //Register all for alterations
                    level_altered.Add(hash, ob);
            }

        }
    }

    void process(Iso i, int v, Thread_Solar_Job job, Queue<Iso> targets)
    {
        if (!inGrid(i,job))
            return;
        int c = get(i);
        if (c == 0 || c > v) //Only proceed if this write is lighter or object has not been written to
        {

            targets.Enqueue(i);
            levels[i.x, i.y, i.z] = v;
        }
    }

    bool inGrid(Iso i)
    {

        if (i.x < 0 || i.x >= width)
            return false;
        if (i.y < 0 || i.y >= length)
            return false;
        if (i.z < 0 || i.z >= height)
            return false;
        return true;
        
    }

    bool inGrid(Iso i, Thread_Solar_Job job)
    {
        if (i.z < 0 || i.z >= height)
            return false;
        return job.inBounds(i);
    }

    int get(Iso i)
    {
        return levels[i.x, i.y, i.z];
    }

    bool check(Iso i)
    {
        return field[i.x, i.y, i.z];
    }

    /// <summary>
    /// The hashing code
    /// </summary>
    int hashBox(int x, int y, int z)
    {
        return (z * layer) + (y * width) + x;
    }


    int hashBox(Iso i)
    {
        return (i.z * layer) + (i.y * width) + i.x;
    }

    public override string ToString()
    {
        return "Flood_Solar<Count : "+count+">";
    }

}
