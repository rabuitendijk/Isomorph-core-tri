using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {

    public SpriteHashLoader temp_loader;


    // Use this for initialization
    void Start()
    {
        temp_loader.onStartup();
        Map map = new Map(8, 8, 8);
        new GraphicsControl(temp_loader);

        map.makeLevel();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
