using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sparse logic control for movement systems testing
/// </summary>
public class TestingLogicControl : LogicControl
{

    Tile[,,] grid;
    Level_XML xml;

    /// <summary>
    /// Run base constructor
    /// </summary>
    TestingLogicControl() : base(){ }

    /// <summary>
    /// Constructs Map with given dimentions.
    /// Set action listeners
    /// Sets map as main Map
    /// </summary>
    public TestingLogicControl(int width, int length, int height) : this()
    {
        grid = new Tile[width, length, height];
        this.width = width;
        this.length = length;
        this.height = height;
    }

    /// <summary>
    /// Constructor that builds level from file
    /// </summary>
    public TestingLogicControl(Level_XML xml, string filename) : this(xml.width, xml.length, xml.height)
    {
        this.filename = filename;
        this.xml = xml;
    }

    /// <summary>
    /// Delayed constructor
    /// </summary>
    public override void delayedConstruction()
    {
        if (xml != null)    //Level provided
        {
            foreach (IsoObject_XML o in xml.nodes)
            {
                new IsoObject(o.name, o.origin, o.direction);
            }
        }

        xml = null;
    }

    /// <summary>
    /// Set a Tile in the grid corresponding with its coord
    /// </summary>
    public override void set(Tile t)
    {
        if (inGrid(t.coord))
        {
            grid[t.coord.x, t.coord.y, t.coord.z] = t;
            return;
        }
        Debug.Log("[Map].set: Tile position invalid.");
    }


    /// <summary>
    /// Sets tile in grid without checking if tile is present or coordinate is in bounds.
    /// </summary>
    public void setUnprotected(Iso i, Tile t)
    {
        grid[i.x, i.y, i.z] = t;
    }

    /// <summary>
    /// Unprotected get
    /// </summary>
    public override Tile get(Iso i)
    {
        return grid[i.x, i.y, i.z];
    }

    /// <summary>
    /// Check exists
    /// </summary>
    public override bool exists(Iso i)
    {
        if (grid[i.x, i.y, i.z] == null)
            return false;

        return true;
    }

    /// <summary>
    /// Checks if an coord is inside the Map
    /// </summary>
    public override bool inGrid(Iso i)
    {
        if (i.x < 0 || i.x >= width)
            return false;
        if (i.y < 0 || i.y >= length)
            return false;
        if (i.z < 0 || i.z >= height)
            return false;
        return true;
    }

    /// <summary>
    /// To be removed.
    /// </summary>
    public override void makeLevel(string name)
    {
        Debug.Log("Not implimented.");
    }

    /// <summary>
    /// Respond to tile creation
    /// </summary>
    protected override void onTileCreate(Tile t)
    {
        set(t);
    }

    /// <summary>
    /// Respond to tile destruction
    /// </summary>
    protected override void onTileDestroy(Tile t)
    {
        setUnprotected(t.coord, null);
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    protected override void destructor()
    {
        //save.destroy();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    if (grid[i, j, k] != null)
                        grid[i, j, k].destroy();
                }
            }
        }
    }
}
