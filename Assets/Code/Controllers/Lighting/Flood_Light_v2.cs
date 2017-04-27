
using System.Collections.Generic;


using Mathf = UnityEngine.Mathf;
using Debug = UnityEngine.Debug;

namespace Lighting_C
{
    /// <summary>
    /// Spreads light contribution
    /// </summary>
    public class Flood_Light_v2
    {
        Lighting_Data data;

        Queue<Iso> targets = new Queue<Iso>();

        int radius, count = 0;
        int[,,] grid;
        int w, l, h;
        int ux, lx, uy, ly, uz, lz; //Bouds
        Iso origin;
        Thread_Light light;
        float p; //propegation constant


        /// <summary>
        /// Constructor computes light contribution and informs relevent objects
        /// </summary>
        public Flood_Light_v2(Thread_Light light, Lighting_Data data)
        {

            this.light = light;
            this.data = data;

            radius = light.radius;
            origin = light.coord;

            p = Mathf.Pow(255f / 10f, 1f / radius);


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
                    v = (100 + v);
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

        /// <summary>
        /// process a tile from the stack
        /// </summary>
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

        /// <summary>
        /// Inverts the light values
        /// </summary>
        void reverseValues()
        {
            //Loop trough all cells

            Iso pos;

            for (int i = lx; i <= ux; i++)
            {
                for (int j = ly; j <= uy; j++)
                {
                    for (int k = lz; k <= uz; k++)
                    {
                        pos = new Iso(i, j, k);
                        if (get(pos) != 0) //Lit tile register
                        {
                            data.add_coverage(light, pos, 255 - get(pos));
                        }
                    }
                }
            }
        }

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

        /// <summary>
        /// Set cell to light value
        /// </summary>
        void set(Iso i, int value)
        {
            grid[i.x - lx, i.y - ly, i.z - lz] = value;
        }

        /// <summary>
        /// get light value
        /// </summary>
        int get(Iso i)
        {
            return grid[i.x - lx, i.y - ly, i.z - lz];
        }

        /// <summary>
        /// Check if conoordinate is in grid
        /// </summary>
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
        /// Old special print function
        /// </summary>
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
}