
using System.Collections.Generic;
using UnityEngine;

public class Thread_Job {
    public Iso coord { get; protected set; }
    public Color color { get; protected set; }
	
    public Thread_Job(Iso coord, Color color)
    {
        this.coord = coord;
        this.color = color;
    } 
}
