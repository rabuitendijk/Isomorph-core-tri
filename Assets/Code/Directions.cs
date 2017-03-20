
using System.Collections.Generic;
using UnityEngine;

public static class Directions  {

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
}
