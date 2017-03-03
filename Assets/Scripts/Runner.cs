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

        Map map = new Map(8, 8, 8);
        new GraphicsControl();

        map.makeLevel();

        /*
        for (int i = 0; i < 8; i++)
        {
            Debug.Log("Mip "+i+": " + (512 >> i));
        }
        */
       
    }


    // Update is called once per frame
    void Update()
    {

    }
}
