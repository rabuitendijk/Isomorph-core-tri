using LogicControl = Logic_C.LogicControl;

using GameObject = UnityEngine.GameObject;
using Debug = UnityEngine.Debug;
using SpriteRenderer = UnityEngine.SpriteRenderer;
using Vector3 = UnityEngine.Vector3;

namespace Graphics_C
{
    /// <summary>
    /// An extended version of Iso that can be projected to the screen [and rotaded]
    /// contains length information
    /// </summary>
    public class ProjIso : Iso
    {

        public static long next_id = 0;

        int depthModifier = 0;
        public int depth { get { return 2 * (2 * x + 2 * y + z) + depthModifier; } }
        public Vector3 position { get { return new Vector3((-x + y) * 0.5f, (-x - y + z) * 0.25f, z + 2 * x + 2 * y); } }




        /// <summary>
        /// Common constructor
        /// </summary>
        public ProjIso(int x, int y, int z, int depthModifier = 0) : base(x, y, z) { this.depthModifier = depthModifier; }

        /// <summary>
        /// Cast constructor
        /// </summary>
        public ProjIso(Iso i, int depthModifier = 0) : base(i) { this.depthModifier = depthModifier; }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public ProjIso(ProjIso i) : base(i) { this.depthModifier = i.depthModifier; }

        /// <summary>
        /// Moves this iso to other and recalculates
        /// </summary>
        public static void moveTo(ProjIso i, GameObject graphic)
        {

            if (graphic == null)
                return;

            graphic.transform.position = i.position;
            graphic.GetComponent<SpriteRenderer>().sortingOrder = i.depth;
        }

        /// <summary>
        /// Return a rotated version of this coord
        /// </summary>
        public ProjIso rotate(Directions.dir dir)
        {
            //Debug.Log("ord");

            switch (dir)
            {
                case Directions.dir.N:
                    return this;
                case Directions.dir.E:
                    return new ProjIso(y, LogicControl.main.length - x - 1, z);
                case Directions.dir.S:
                    return new ProjIso(LogicControl.main.width - x - 1, LogicControl.main.length - y - 1, z);
                case Directions.dir.W:
                    return new ProjIso(LogicControl.main.width - y - 1, x, z);
                default:
                    Debug.Log("ProjIso.rotate: default swidtch: " + dir);
                    return null;
            }
        }

        /// <summary>
        /// Return a rotated version of this coord accounting for sprite roatations
        /// </summary>
        public ProjIso rotate(Directions.dir dir, IsoObjectBody ob)
        {

            if (ob.width == 1 && ob.length == 1)
                return rotate(dir);

            //Debug.Log("special");
            ProjIso ret = new ProjIso(this);
            ret = ret - ob.origin;

            //Sub rotation
            switch (dir)
            {
                case Directions.dir.N:
                    break;
                case Directions.dir.W:
                    ret = new ProjIso(ret.y, ob.length - ret.x - 1, ret.z);
                    break;
                case Directions.dir.S:
                    ret = new ProjIso(ob.width - ret.x - 1, ob.length - ret.y - 1, ret.z);
                    break;
                case Directions.dir.E:
                    ret = new ProjIso(ob.width - ret.y - 1, ret.x, ret.z);
                    break;
                default:
                    Debug.Log("ProjIso.rotate: default swidtch: " + dir);
                    return null;
            }
            ret = ret + ob.origin;

            return ret.rotate(dir);
        }

        /// <summary>
        /// Return this, containing this+that
        /// </summary>
        public static ProjIso operator +(ProjIso lh, Iso rh)
        {
            lh.x += rh.x;
            lh.y += rh.y;
            lh.z += rh.z;
            return lh;
        }

        /// <summary>
        /// Return this, containing this-that
        /// </summary>
        public static ProjIso operator -(ProjIso lh, Iso rh)
        {
            lh.x -= rh.x;
            lh.y -= rh.y;
            lh.z -= rh.z;
            return lh;
        }
    }
}