
using System.Collections.Generic;

using Vector3 = UnityEngine.Vector3;
using Color = UnityEngine.Color;

namespace Graphics_C
{

    /// <summary>
    /// version aplha-2
    /// 
    /// Instructions for GLDrawLoop.
    /// Contains a list of point to draw lines between.
    /// 
    /// Robin Apollo Buitendijk
    /// Early March 2017
    /// </summary>
    public class GLInstruction
    {

        public List<Vector3> points { get; protected set; }

        public bool relativeToCamera { get; protected set; }
        public bool loop { get; protected set; }
        public bool autoconnect { get; protected set; }

        public Color color { get; protected set; }

        /// <summary>
        /// Common constructor
        /// </summary>
        public GLInstruction(List<Vector3> points, Color color, bool autoconnect = true, bool loop = true, bool relativeToCamera = false)
        {
            this.color = color;
            this.points = points;
            this.autoconnect = autoconnect;
            this.loop = loop;
            this.relativeToCamera = relativeToCamera;
            this.color = color;
        }
    }
}