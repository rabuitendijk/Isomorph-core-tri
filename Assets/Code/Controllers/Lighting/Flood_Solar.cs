
using System.Collections.Generic;

namespace Lighting_C
{

    /// <summary>
    /// Computes the suns contribution
    /// </summary>
    public class Flood_Solar
    {
        Lighting_Data data;

        int[,,] levels;
        int count = 0;


        int p; //propegation constant

        /// <summary>
        /// Common constructor
        /// </summary>
        public Flood_Solar(Lighting_Data data, int radius)
        {
            this.data = data;
            levels = data.solar_field;

            //p = Mathf.Pow(10f/ 255f, 1f / (radius));
            p = (255 - 20) / radius;
        }

        /// <summary>
        /// Computes the sun contribution for parameters given in job
        /// </summary>
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

                        if (levels[i, j, k] == 0)
                        {
                            //Add new sources if inGrid and v>0

                            tilecheck(new Iso(i - 1, j, k), targets, checks, job);
                            tilecheck(new Iso(i + 1, j, k), targets, checks, job);
                            tilecheck(new Iso(i, j - 1, k), targets, checks, job);
                            tilecheck(new Iso(i, j + 1, k), targets, checks, job);
                            tilecheck(new Iso(i, j, k - 1), targets, checks, job);
                            tilecheck(new Iso(i, j, k + 1), targets, checks, job);
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

        /// <summary>
        /// Check if a tile sould be added to flood algorithm
        /// </summary>
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
        /// Rains down max light value sets the rest to min light value
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

                data.alter_level_on_coord(new Iso(x, y, k));

            }
        }

        /// <summary>
        /// Process a single target tile
        /// </summary>
        void process(Iso i, int v, Thread_Solar_Job job, Queue<Iso> targets)
        {
            if (!inGrid(i, job))
                return;
            int c = get(i);
            if (c == 0 || c < v) //Only proceed if this write is lighter or object has not been written to
            {

                targets.Enqueue(i);
                levels[i.x, i.y, i.z] = v;
            }
        }

        /// <summary>
        /// See if coord is in grid
        /// </summary>
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

        /// <summary>
        /// See if coord is in job
        /// </summary>
        bool inGrid(Iso i, Thread_Solar_Job job)
        {
            if (i.z < 0 || i.z >= data.height)
                return false;
            return job.inBounds(i);
        }

        /// <summary>
        /// get light level at coord
        /// </summary>
        int get(Iso i)
        {
            return levels[i.x, i.y, i.z];
        }

        /// <summary>
        /// Hashing algorithm for hash set
        /// </summary>
        int hashBox(Iso i)
        {
            return (i.z * data.tiles_per_layer) + (i.y * data.width) + i.x;
        }

        public override string ToString()
        {
            return "Flood_Solar<Count : " + count + ">";
        }

    }
}