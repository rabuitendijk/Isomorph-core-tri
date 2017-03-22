
using System.Collections.Generic;
using UnityEngine;

public class SplicingSource  {

    public List<ProcessingImage[,,]> mips = new List<ProcessingImage[,,]>();
    public int width { get; protected set; }
    public int length { get; protected set; }
    public int height { get; protected set; }

    public SplicingSource(int mips, int width, int length, int height)
    {
        this.width = width;
        this.length = length;
        this.height = height;

       
    } 
}
