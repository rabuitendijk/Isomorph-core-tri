using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This runs the game
/// </summary>
public class Runner : MonoBehaviour {
    public static Runner main;

    public Font ariel; 
    public bool rebuildAtlasses = false;

    ControllerManager manager;

    // Use this for initialization
    void Start()
    {


        main = this;
        if (rebuildAtlasses)
            AssetHandeling_AtlasBuilder.Atlas_Builder.build(128, 1, 8);

        new AssetHandeling_AtlasLoader.Atlas_Loader(128, 1);
        manager = new ControllerManager();


        
    }


    // Update is called once per frame
    void Update()
    {
        manager.update();

    }



}
