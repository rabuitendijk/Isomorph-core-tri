
using System.Collections.Generic;
using Sprite = UnityEngine.Sprite;

/// <summary>
/// Uneditable prototype data;
/// </summary>
public class IsoObjectData {

    public readonly string name;
    readonly List<Iso> Coords;   //Clonable
    public List<Iso> coords { get { return clone(Coords); } }

    public readonly bool singular;
    public readonly List<List<Sprite>> directions; //N, E, S, W
    public readonly List<bool> isVisable;   //Needed?
    public readonly bool is_light;
    public readonly int light_radius;

    public readonly int width;
    public readonly int height;
    public readonly int length;

    public IsoObjectData(string name, List<Iso> coords, List<List<Sprite>> directions, bool is_light, int light_radius, int width, int height, int length)
    {
        this.name = name;
        this.Coords = coords;

        singular = false;
        if (coords.Count == 1)
            singular = true;

        this.directions = directions;

        isVisable = new List<bool>();
        foreach (Sprite s in directions[0])
        {
            isVisable.Add((s != null));
        }

        this.is_light = is_light;
        this.light_radius = light_radius;
        this.width = width;
        this.height = height;
        this.length = length;
    }


    /// <summary>
    /// Clones a coordinate list to avoid changing original coordinates
    /// </summary>
    List<Iso> clone(List<Iso> list)
    {
        List<Iso> ret = new List<Iso>();

        foreach (Iso i in list)
        {
            ret.Add(new Iso(i));
        }

        return ret;
    }

}
