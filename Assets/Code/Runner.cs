using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {
    public static Runner main;

    public Font ariel; 
    public bool rebuildAliasses = false;
    public bool levelEditor = false;

    InputControl inputControl;
    GraphicsControl graphicsControl;
    LogicControl logicControl;
    // Use this for initialization
    void Start()
    {


        main = this;
        HUI_EditorLoadCommand.registerLoad(loadLevelInEditor);


        if (rebuildAliasses)
            Alias_Builder.build();

        new Alias_Loader();


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
        if (inputControl != null)
            inputControl.update();
    }

    void flush()
    {
        if (logicControl != null)
            logicControl.destroy();
        if (graphicsControl != null)
            graphicsControl.destroy();
        if (inputControl != null)
            inputControl.destroy();

        logicControl = null;
        inputControl = null;
        graphicsControl = null;
    }

    void loadLevelInEditor(string filename)
    {
        
        Level_XML xml;

        if (!LevelLoader.loadFile(filename, out xml))
        {
            Debug.Log("Runner, Somthing went wrong while loading a level.");
            return;
        }
        //Debug.Log(xml.ToString());

        flush();

        graphicsControl = new EditorGraphicsControl();
        inputControl = new EditorInputControl();
        logicControl = new EditorLogicControl(xml, filename);

    }


}
