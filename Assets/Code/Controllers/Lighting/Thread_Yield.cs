

namespace Lighting_C
{

    /// <summary>
    /// Handles the process of yielding to the next solar state
    /// </summary>
    public class Thread_Yield
    {

        Lighting_Data data;
        int yield_count, i = 0, j = 0, k = 0;
        public bool fully_yielded { get; protected set; }

        /// <summary>
        /// Common constuctor
        /// </summary>
        public Thread_Yield(Lighting_Data data, int yield_count)
        {
            this.data = data;
            this.yield_count = yield_count;
        }

        /// <summary>
        /// Yield to next frame
        /// </summary>
        public void yield()
        {
            if (fully_yielded)
                return;

            int count = 0;
            bool set = false;
            Thread_IsoObject ob;

            for (int x = 0; x < data.width; x++)
            {

                for (int y = 0; y < data.length; y++)
                {

                    for (int z = 0; z < data.height; z++)
                    {
                        if (!set)
                        {
                            x = i;
                            y = j;
                            z = k;
                            set = true;
                        }

                        if (data.tryGetObject(new Iso(x, y, z), out ob))
                        {
                            if (count >= yield_count)   //Yield here
                            {
                                i = x;
                                j = y;
                                k = z;
                                //Debug.Log("yc = " + yield_count + " [" + x + "," + y + "," + z + "] {" + i + "," + j + "," + k + "}");
                                //count = 0;
                                return;
                            }
                            count++;
                            data.add_to_level_altered(ob);
                        }
                    }
                }


            }
            //Debug.Log("yc = "+yield_count+" ["+i+","+j+","+k+"]");
            fully_yielded = true;
        }

        /// <summary>
        /// Reset the yielding process
        /// </summary>
        public void reset(int yield_count = -1)
        {
            if (yield_count != -1)
                this.yield_count = yield_count;
            i = 0;
            j = 0;
            k = 0;
            fully_yielded = false;
        }
    }
}