
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
}
