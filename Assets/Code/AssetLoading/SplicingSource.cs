
using System.Collections.Generic;
using UnityEngine;

public class SplicingSource  {

    public List<ProcessingImage[,,]> mips = new List<ProcessingImage[,,]>();
    public int width { get; protected set; }
    public int depth { get; protected set; }
    public int height { get; protected set; }

    public SplicingSource(int mips, int width, int depth, int height)
    {
        this.width = width;
        this.depth = depth;
        this.height = height;

       
    } 
}
