using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {


    public bool rebuildAliasses = false;

    // Use this for initialization
    void Start()
    {
        if (rebuildAliasses)
            new BuildAliasTextures();

        
        new AliasXMLLoader();

        Map map = new Map(12, 12, 24);
        new GraphicsControl();

        map.makeLevel();
       
    }


    // Update is called once per frame
    void Update()
    {

    }
}
