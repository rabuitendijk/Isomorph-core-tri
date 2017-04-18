using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingLogicControl : LogicControl
{

    Tile[,,] grid;
    Level_XML xml;


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

    public TestingLogicControl(Level_XML xml, string filename) : this(xml.width, xml.length, xml.height)
    {
        this.filename = filename;
        this.xml = xml;
    }

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

    public override void makeLevel(string name)
    {
        Debug.Log("Not implimented.");
    }

    protected override void onTileCreate(Tile t)
    {
        set(t);
    }

    protected override void onTileDestroy(Tile t)
    {
        setUnprotected(t.coord, null);
    }

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
