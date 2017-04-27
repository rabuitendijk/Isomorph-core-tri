

namespace Lighting_C
{

    /// <summary>
    /// Target for solar recalculation
    /// </summary>
    public class Thread_Solar_Job
    {

        public int lx, ux, ly, uy;
        public int w, l;
        public bool full { get; protected set; }

        /// <summary>
        /// Full coverage recalculation job
        /// </summary>
        public Thread_Solar_Job(int width, int length)
        {
            lx = 0;
            ly = 0;
            ux = width - 1;
            uy = length - 1;

            w = width;
            l = length;
            full = true;
        }

        /// <summary>
        /// Partial recalculation job
        /// </summary>
        public Thread_Solar_Job(Iso coord, int radius, int width, int length)
        {
            ux = coord.x + radius;
            lx = coord.x - radius;
            uy = coord.y + radius;
            ly = coord.y - radius;

            boxin(width, length);
            full = false;
        }

        /// <summary>
        /// Sets the bounds of the job
        /// </summary>
        void boxin(int width, int length)
        {
            if (lx < 0)
                lx = 0;
            if (ux >= width)
                ux = width - 1;
            if (ly < 0)
                ly = 0;
            if (uy >= length)
                uy = length - 1;

            w = ux - lx + 1;
            l = uy - ly + 1;
        }

        /// <summary>
        /// check if coord is in bounds
        /// </summary>
        public bool inBounds(Iso i)
        {
            if (i.x < lx || i.x > ux)
                return false;
            if (i.y < ly || i.y > uy)
                return false;
            return true;
        }
    }
}