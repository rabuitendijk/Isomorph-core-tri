
using System.Collections.Generic;
using UnityEngine;

public static class Directions  {

    public static dir currentDirection = dir.N;
    public enum dir { N, E, S, W };

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

    public static dir add(dir d1, dir d2)
    {
        return (dir)(((int)d1 + (int)d2) % 4);
    }

    public static dir subtract(dir d1, dir d2)
    {
        int i = (int)d1 - (int)d2;
        if (i < 0)
            i = 4 + i;

        return (dir)i;
    }

    public static dir increment(dir d1)
    {
        return (dir)(((int)d1 + 1) % 4);
    }

    public static dir decrement(dir d1)
    {
        int i = (int)d1 - 1;
        if (i < 0)
            i = 4 + i;

        return (dir)i;
    }

    public static Iso mouseToFloor(int z)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Iso i = null;
        Iso target;

        if (currentDirection == dir.E)
        {
            pos += new Vector3(-LogicControl.main.length*.5f, (LogicControl.main.length)*.25f, 0f);
            target = new Iso(Mathf.FloorToInt(2f * pos.y - pos.x), Mathf.FloorToInt(-2f * pos.y - pos.x), z);
        }
        else
            target = new Iso(pos.x, pos.y, z);

        if (LogicControl.main.inGrid(target))
        {
            i = target;
            return i;
        }
        return i;
    }
}
