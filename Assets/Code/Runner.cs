using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {
    public static Runner main;

    public Font ariel; 
    public bool rebuildAtlasses = false;
    public bool levelEditor = false;

    InputControl inputControl;
    GraphicsControl graphicsControl;
    LogicControl logicControl;
    SaveControl saveControl;
    UIControl uiControl;
    LightingControl lightingControl;

    // Use this for initialization
    void Start()
    {


        main = this;
        HUI_EditorLoadCommand.registerLoad(loadLevelInEditor);

        

        if (rebuildAtlasses)
            Atlas_Builder.build(128, 1, 8);

        new Atlas_Loader(128, 1);


        if (levelEditor)
        {
            Debug.Log("Editor mode loaded.");

            logicControl = new EditorLogicControl(24, 24, 16);
            saveControl = new EditorSaveControl();
            inputControl = new EditorInputControl();
            graphicsControl = new EditorGraphicsControl();
            uiControl = new EditorUIControl();
            lightingControl = new EditorLightingControl();

            saveControl.delayedConstruction();
            inputControl.delayedConstruction();
            graphicsControl.delayedConstruction();
            logicControl.delayedConstruction();
            uiControl.delayedConstruction();
            lightingControl.delayedConstruction();

        }
        else
        {
            Debug.Log("TODO");
        }

        
    }


    // Update is called once per frame
    void Update()
    {
        if (inputControl != null)
            inputControl.update();
        if (lightingControl != null)
            lightingControl.update();
    }

    void flush()
    {
        if (logicControl != null)
            logicControl.destroy();
        if (graphicsControl != null)
            graphicsControl.destroy();
        if (inputControl != null)
            inputControl.destroy();
        if (saveControl != null)
            saveControl.destroy();
        if (uiControl != null)
            uiControl.destroy();
        if (lightingControl != null)
            lightingControl.destroy();

        logicControl = null;
        inputControl = null;
        graphicsControl = null;
        saveControl = null;
        uiControl = null;
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

        logicControl = new EditorLogicControl(xml, filename);
        saveControl = new EditorSaveControl();
        graphicsControl = new EditorGraphicsControl();
        inputControl = new EditorInputControl();
        uiControl = new EditorUIControl();
        lightingControl = new EditorLightingControl();

        saveControl.delayedConstruction();
        inputControl.delayedConstruction();
        graphicsControl.delayedConstruction();
        logicControl.delayedConstruction();
        uiControl.delayedConstruction();
        lightingControl.delayedConstruction();
    }


}
