
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// aplha-1
/// 
/// Temp datacarrier for moving data to object texture Atlas.
/// 
/// Robin Apollo Buitendijk
/// Early March 2017
/// </summary>
public class ProcessingImage {

    Color[,] data;
    int Width, Height;
    public int height { get { return Height; } }
    public int width { get { return Width; } }
    public string name { get; private set; }
    public Iso coord { get; private set; }

    public ProcessingImage(int width, int height, string name)
    {
        Width = width;
        Height = height;
        this.name = name;

        data = new Color[width, height];
    }

    public ProcessingImage(int width, int height, string name, Iso coord) : this(width, height, name)
    {
        this.coord = coord;
    }

    public Color get(int x, int y)
    {
        return data[x, y];
    }

    public void set(int x, int y, Color value)
    {
        data[x, y] = value;
    }
}
