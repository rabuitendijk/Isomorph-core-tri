
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// alpha-1
/// 
/// Holds object data for splicing script loaded images
/// To be processed by the alias builder.
/// 
/// Robin Apollo Buitendijk
/// Early March 2017
/// </summary>
public class ProcessingObject {

	public List<ProcessingImage> images { get; protected set; }
    public List<ProcessingImage> mip1 { get; protected set; }
    public List<ProcessingImage> mip2 { get; protected set; }

    public List<Iso> coords { get; protected set; }
    public string name { get; protected set; }

    public ProcessingObject(string name, List<ProcessingImage> images, List<Iso> coords, List<ProcessingImage> mip1 =null, List<ProcessingImage> mip2=null)
    {
        this.name = name;
        this.images = images;
        this.coords = coords;
        this.mip1 = mip1;
        this.mip2 = mip2;

        if (images.Count > 49)
            Debug.Log("ProcessingObject: Image has to many pieces (>49), horrible crash imminent.");
    }
}
