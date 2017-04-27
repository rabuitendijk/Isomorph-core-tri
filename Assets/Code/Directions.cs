using LogicControl = Logic_C.LogicControl;

using UnityEngine;

/// <summary>
/// Static class containing information on and fucntions dealing with the current direction
/// </summary>
public static class Directions  {

    public static dir currentDirection = dir.N;
    public enum dir { N, E, S, W };

    /// <summary>
    /// String to dir
    /// </summary>
	public static dir getDir(string s)
    {
        switch (s)
        {
            case "N":
                return dir.N;
            case "E":
                return dir.E;
            case "S":
                return dir.S;
            case "W":
                return dir.W;
            default:
                Debug.Log("Directions.getDir("+s+"), not a know dir.");
                return dir.N;
        }
    }

    /// <summary>
    /// Sum of 2 directions
    /// </summary>
    public static dir add(dir d1, dir d2)
    {
        return (dir)(((int)d1 + (int)d2) % 4);
    }

    /// <summary>
    /// Subtraction of 2 directions
    /// </summary>
    public static dir subtract(dir d1, dir d2)
    {
        int i = (int)d1 - (int)d2;
        if (i < 0)
            i = 4 + i;

        return (dir)i;
    }

    /// <summary>
    /// Increment direction by 1
    /// </summary>
    public static dir increment(dir d1)
    {
        return (dir)(((int)d1 + 1) % 4);
    }

    /// <summary>
    /// Subtract 1 from teh given direction
    /// </summary>
    public static dir decrement(dir d1)
    {
        int i = (int)d1 - 1;
        if (i < 0)
            i = 4 + i;

        return (dir)i;
    }

    /// <summary>
    /// Correct mouse in game space collision with isometric layer of height z.
    /// </summary>
    public static Iso mouseToFloor(int z)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Iso target;

        if (currentDirection == dir.E)
        {
            pos += new Vector3(-LogicControl.main.length*.5f, (LogicControl.main.length)*.25f, 0f);
            target = new Iso(Mathf.FloorToInt(2f * pos.y - pos.x -.5f*z), Mathf.FloorToInt(-2f * pos.y - pos.x +.5f*z), z);
        }
        else if (currentDirection == dir.S)
        {
            pos += new Vector3(0, (LogicControl.main.length+ LogicControl.main.width) * .25f, 0f);
            target = new Iso(Mathf.FloorToInt(2f * pos.y + pos.x - .5f * z), Mathf.FloorToInt(2f * pos.y - pos.x - .5f * z), z);
        }
        else if (currentDirection == dir.W)
        {
            pos += new Vector3(LogicControl.main.width * .5f, (LogicControl.main.width) * .25f, 0f);
            target = new Iso(Mathf.FloorToInt(-2f * pos.y + pos.x + .5f * z), Mathf.FloorToInt(2f * pos.y + pos.x - .5f * z), z);
        }
        else
            target = new Iso(pos.x, pos.y, z);

        return target;
    }



    
    /// <summary>
    /// finds last unoccupied Iso
    /// </summary>
    public static bool raycastClick(out Iso i, out Tile hit)
    {

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float upper_n, upper_p, upper_z;

        if (currentDirection == dir.E)
        {
            //Starting coords E
            pos += new Vector3(-LogicControl.main.length * .5f, (LogicControl.main.length) * .25f, 0f);
            upper_z = LogicControl.main.height + 1;
            upper_n = (2f * pos.y - pos.x - .5f * upper_z);
            upper_p = (-2f * pos.y - pos.x + .5f * upper_z);

            return raycastClickHit(out i, out hit, upper_n, upper_p, upper_z, 1, -1);
        }

        if (currentDirection == dir.S)
        {
            //Starting coords S
            pos += new Vector3(0, (LogicControl.main.length + LogicControl.main.width) * .25f, 0f);
            upper_z = LogicControl.main.height + 1;
            upper_n = (2f * pos.y + pos.x - .5f * upper_z);
            upper_p = (2f * pos.y - pos.x - .5f * upper_z);

            return raycastClickHit(out i, out hit, upper_n, upper_p, upper_z, 1, 1);
        }

        if (currentDirection == dir.W)
        {
            //Starting coords W
            pos += new Vector3(LogicControl.main.width * .5f, (LogicControl.main.width) * .25f, 0f);
            upper_z = LogicControl.main.height + 1;
            upper_n = (-2f * pos.y + pos.x + .5f * upper_z);
            upper_p = (2f * pos.y + pos.x - .5f * upper_z);

            return raycastClickHit(out i, out hit, upper_n, upper_p, upper_z, -1, 1);
        }

        //Starting coords N
        upper_z = LogicControl.main.height + 1;
        upper_n = (-2f * pos.y - pos.x + .5f * upper_z);
        upper_p = (-2f * pos.y + pos.x + .5f * upper_z);

        return raycastClickHit(out  i, out hit, upper_n, upper_p, upper_z, -1, -1);
    }

    /// <summary>
    /// The functions that actually processes a mouse raycast after its location and direction have been determined
    /// </summary>
    static bool raycastClickHit(out Iso i, out Tile hit, float upper_n, float upper_p, float upper_z, int dn, int dp)
    {
        Iso target = new Iso(Mathf.FloorToInt(upper_n), Mathf.FloorToInt(upper_p), Mathf.FloorToInt(upper_z));
        target.add(0, 0, -1); //Start correction

        float delta;
        i = new Iso(0, 0, 0);
        bool setFlag = false;


        while (target.z >= 0)
        {
            delta = nextCell(upper_n, upper_p, upper_z, target, dn, dp);    //Internally dependent

            //Dependent
            upper_z -= 2 * delta; upper_n += dn*delta; upper_p += dp*delta;


            if (LogicControl.main.inGrid(target))
            {
                if (LogicControl.main.exists(target))
                {
                    hit = LogicControl.main.get(target);
                    if (!setFlag)
                        return false;
                    return true;
                }
                setFlag = true;
                i.set(target);
            }

        }

        hit = null;
        return false;
    }

    /// <summary>
    /// Finds next cell in simulated raycast for mouse click
    /// </summary>
    static float nextCell(float ox, float oy, float oz, Iso coord, int dn, int dp)
    {
        ox -= coord.x;
        oy -= coord.y;
        oz -= coord.z; oz /= 2;

        if (dn == 1)
            ox = 1 - ox;

        if (dp == 1)
            oy = 1 - oy;


        

        if (oz <= ox)
        {
            if (oz <= oy)
            {
                //z transition

                coord.add(0, 0, -1);
                return oz;
            }
            //y transtion
            coord.add(0, dp, 0);
            return oy;
        }
        else if (ox <= oy)
        {
            //x transtion

            coord.add(dn, 0, 0);
            return ox;
        }
        //y transition
        coord.add(0, dp, 0);
        return oy;
    }

}
