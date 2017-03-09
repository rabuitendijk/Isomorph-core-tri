
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// alpha-1
/// 
/// Map that contains all tiles in a 3d grid data stucture.
/// Fed by action listeners
/// 
/// Robin Apollo Butiendijk
/// Late February 2017
/// </summary>
public class Map {

    int Width, Depth, Height;
    public int width { get { return Width; } }
    public int depth { get { return Depth; } }
    public int height { get { return Height; } }

    public static Map main;
    Tile[,,] grid;

	Map(){
        main = this;
        Tile.registerOnCreate(onTileCreate);
    }

    /// <summary>
    /// Constructs Map with given dimentions.
    /// Set action listeners
    /// Sets map as main Map
    /// </summary>
    public Map(int width, int depth, int height) : this()
    {
        grid = new Tile[width, depth, height];
        Width = width;
        Depth = depth;
        Height = height;


    }

    /// <summary>
    /// Set a Tile in the grid corresponding with its coord
    /// </summary>
    void set(Tile t)
    {
        if (inGrid(t.coord))
        {
            grid[t.coord.x, t.coord.y, t.coord.z] = t;
            return;
        }
        Debug.Log("[Map].set: Tile position invalid.");
    }

    /// <summary>
    /// Unprotected get
    /// </summary>
    public Tile get(Iso i)
    {
        return grid[i.x, i.y, i.z];
    }

    /// <summary>
    /// Check exists
    /// </summary>
    public bool exists(Iso i)
    {
        if (grid[i.x, i.y, i.z] == null)
            return false;

        return true;
    }
    /// <summary>
    /// Checks if an coord is inside the Map
    /// </summary>
    public bool inGrid(Iso i)
    {
        if (i.x < 0 || i.x >= Width)
            return false;
        if (i.y < 0 || i.y >= Depth)
            return false;
        if (i.z < 0 || i.z >= Height)
            return false;
        return true;
    }

    public void makeLevel()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                new IsoObject("half", new Iso(i, j, Mathf.FloorToInt((i + j) / 3f)));
                if (i%(j+1)==0)
                    new IsoObject("stones", new Iso(i, j, Mathf.FloorToInt((i + j) / 3f) + 1));
                else
                    new IsoObject("roughStone", new Iso(i, j, Mathf.FloorToInt((i + j) / 3f)+1));
                if (i % (j+1) == 2)
                    new IsoObject("tree", new Iso(i, j, (Mathf.FloorToInt((i + j) / 3f)+3)));
                if (i % (j + 1) == 5)
                    new IsoObject("roseRock", new Iso(i, j, (Mathf.FloorToInt((i + j) / 3f) + 3)));
            }
        }
        for (int i = 0; i < 3; i++)
        {
            new IsoObject("unit", new Iso(0, 3, (2*i)+4));
        }

        new IsoObject("bigBlock", new Iso(3,0,4));
        
    }

    public void makeLevelBox()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    new IsoObject("stones", new Iso(i, j, k));
                }
            }
        }
    }

    public void makeSlabTest()
    {
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                if (i < 3 || j < 3 || i > width - 4 || j > depth - 4)
                    new IsoObject("half", new Iso(i, j, 0));

            }
        }

        for (int i = 1; i < width-1; i++)
        {
            for (int j = 1; j < depth-1; j++)
            {
                if (i < 3 || j < 3 || i > width - 4 || j > depth - 4)
                    new IsoObject("unit", new Iso(i, j, 1));

            }
        }

        for (int i = 2; i < width - 2; i++)
        {
            for (int j = 2; j < depth - 2; j++)
            {

                if (i < 3 || j < 3 || i > width - 4 || j > depth - 4)
                    new IsoObject("half", new Iso(i, j, 3));

            }
        }
        

        new IsoObject("bigBlock", new Iso(4, 4, 0));
        new IsoObject("bigBlock", new Iso(6, 4, 0));
        new IsoObject("bigBlock", new Iso(4, 6, 0));
        new IsoObject("bigBlock", new Iso(6, 6, 0));

        for (int i = 0; i < height; i++)
        {
            new IsoObject("half", new Iso(3,4,i));
        }
    }

    void onTileCreate(Tile t)
    {
        set(t);

    }

    
}
