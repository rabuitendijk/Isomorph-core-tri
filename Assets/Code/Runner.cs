using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {
    public static Runner main;

    public bool rebuildAliasses = false;
    public bool levelEditor = false;

    InputControl inputControl;
    GraphicsControl graphicsControl;
    LogicControl logicControl;
    // Use this for initialization
    void Start()
    {
        main = this;

        if (rebuildAliasses)
            new BuildAliasTextures();

        new AliasXMLLoader();


        if (levelEditor)
        {
            Debug.Log("Editor mode loaded.");

            logicControl = new EditorLogicControl(16, 16, 16);
            graphicsControl = new EditorGraphicsControl();
            inputControl = new EditorInputControl();
        }
        else
        {
            logicControl = new BasicLogicControl(16, 16, 16);
            graphicsControl = new BasicGraphicsControl();
            inputControl = new BasicInputControl();

            logicControl.makeLevel("Simple");
        }

        
    }


    // Update is called once per frame
    void Update()
    {
        inputControl.update();
    }

}
