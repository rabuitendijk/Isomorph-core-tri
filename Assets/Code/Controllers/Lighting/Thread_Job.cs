
using Color = UnityEngine.Color;

namespace Lighting_C
{

    /// <summary>
    /// A job that is returned to the main thead to be applied
    /// </summary>
    public class Thread_Job
    {
        public Iso coord { get; protected set; }
        public Color color { get; protected set; }

        /// <summary>
        /// Common constructor
        /// </summary>
        public Thread_Job(Iso coord, Color color)
        {
            this.coord = coord;
            this.color = color;
        }
    }
}