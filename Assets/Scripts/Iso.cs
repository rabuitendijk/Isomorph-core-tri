using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version alpha-1
/// 
/// Coordinate for isomeric object
/// The occupies 1 projection unit 
/// 
/// Robin Apollo Butiendijk
/// Late February 2017
/// </summary>
public class Iso{
    public static float projH = Mathf.Sqrt(2) / Mathf.Sqrt(3); //[s2s3] assumed

    int X, Y, Z;    //3d projection coordingas
    public int x { get { return X; } }
    public int y { get { return Y; } }
    public int z { get { return Z; } }
    float WX, WY;    //2d World coordinates 
    public float wx { get { return WX; } }
    public float wy { get { return WY; } }

    int Depth;      //For drawing order
    public int depth { get { return Depth; } }
    int depthModifier = 0;

    /// <summary>
    /// Constructor from isometric coordinates
    /// </summary>
    public Iso(int x, int y, int z, int depthModifier = 0)
    {
        X = x;
        Y = y;
        Z = z;
        this.depthModifier = depthModifier;
        calcWorldCoord();
        calcDepth();
    }

    /// <summary>
    /// Constructor from world coordinates
    /// (and isometric z)
    /// </summary>
    public Iso(float x, float y, int z) : this(Mathf.FloorToInt(2f * y - x - .5f * z +1), Mathf.FloorToInt(2f * y + x - .5f * z +1), z) { }


    /// <summary>
    /// Sets wx, wy trough reference to static class.
    /// </summary>
    private void calcWorldCoord()
    {
        WX = (-x + y) * 0.5f;
        WY = (x + y +z) * 0.25f;
    }

    /// <summary>
    /// Sets the depth in order to determine drawing order
    /// Need work, temp solution
    /// </summary>
    private void calcDepth()
    {
        Depth = 20000-2*(2*X + 2*Y - Z)+depthModifier;
    }

    public Vector3 toPos()
    {
        return new Vector3(WX, WY, 0);
    }

    public void add(Iso other)
    {
        X += other.x;
        Y += other.y;
        Z += other.Z;
        calcWorldCoord();
        calcDepth();
    }

    public void addUnsafe(int x, int y, int z)
    {
        X += x;
        Y += y;
        Z += z;
    }

    public override string ToString()
    {
        return "Iso<"+x+", "+y+", "+z+">";
    }


}
