using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {


    public bool rebuildAliasses = false;

    InputController inputController;
    // Use this for initialization
    void Start()
    {
        if (rebuildAliasses)
            new BuildAliasTextures();

        
        new AliasXMLLoader();

        Map map = new Map(16, 16, 16);
        new GraphicsControl();

        map.makeLevel();
        inputController = new InputController();
    }


    // Update is called once per frame
    void Update()
    {
        inputController.update();
    }



}
