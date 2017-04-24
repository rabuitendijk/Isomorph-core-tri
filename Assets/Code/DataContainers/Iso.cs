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

    public int x { get; protected set; }
    public int y { get; protected set; }
    public int z { get; protected set; }

    protected Iso() { }

    /// <summary>
    /// Constructor from isometric coordinates
    /// </summary>
    public Iso(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    public Iso(Iso i){set(i);}

    /// <summary>
    /// Constructor from world coordinates
    /// (and isometric z)
    /// </summary>
    public Iso(float x, float y, int z) : this(Mathf.FloorToInt(-2f * y - x + .5f * z ), Mathf.FloorToInt(-2f * y + x + .5f * z), z) { }

    /// <summary>
    /// returns new Iso containing sum
    /// </summary>
    public static Iso operator +(Iso lh, Iso rh)
    {
        Iso ret = new Iso(lh.x+rh.x, lh.y+rh.y, lh.z +rh.z);
        return ret;
    }

    /// <summary>
    /// Returns new Iso containing lh-rh
    /// </summary>
    public static Iso operator -(Iso lh, Iso rh)
    {
        Iso ret = new Iso(lh.x - rh.x, lh.y - rh.y, lh.z - rh.z);
        return ret;
    }

    /// <summary>
    /// Add rh to this vector
    /// </summary>
    public void add(Iso rh)
    {
        x += rh.x;
        y += rh.y;
        z += rh.z;
    }

    /// <summary>
    /// add deltas to this vector
    /// </summary>
    public void add(int x, int y, int z)
    {
        this.x += x;
        this.y += y;
        this.z += z;
    }

    /// <summary>
    /// Set this vectors position to the other vectors position
    /// </summary>
    public void set(Iso other)
    {
        x = other.x;
        y = other.y;
        z = other.z;
    }

    public override string ToString()
    {
        return "Iso<"+x+", "+y+", "+z+">";
    }


}
