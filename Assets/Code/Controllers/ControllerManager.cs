﻿using AssetHandeling_LevelLoader;
using UnityEngine;
using Graphics_C;
using Input_C;
using Logic_C;
using Lighting_C;
using Save_C;
using UI_C;

/// <summary>
/// Manages all controllers and the transitions between them
/// </summary>
public class ControllerManager {
    public static ControllerManager main;
    public enum Mode { editor, movement_test}

    Mode nextmode;
    bool swap_on_update = false;
    string next_level;

    InputControl inputControl;
    GraphicsControl graphicsControl;
    LogicControl logicControl;
    SaveControl saveControl;
    UIControl uiControl;
    LightingControl lightingControl;

    /// <summary>
    /// Common constructor
    /// </summary>
    public ControllerManager()
    {
        main = this;
        editor_mode("VOID");
    }

    /// <summary>
    /// Swaps to given mode next update
    /// </summary>
    public void setNextSwap(Mode mode, string level = "VOID")
    {

        swap_on_update = true;
        nextmode = mode;
        next_level = level;
    }

    /// <summary>
    /// Runs once per frame
    /// </summary>
    public void update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            setNextSwap(Mode.editor);

        if (Input.GetKeyDown(KeyCode.F5))
            setNextSwap(Mode.movement_test);

        if (swap_on_update)
            swap();

        if (inputControl != null)
            inputControl.update();
        if (lightingControl != null)
            lightingControl.update();

    }

    /// <summary>
    /// Switch case that controlls the target swap.
    /// </summary>
    void swap()
    {
        flush();

        switch (nextmode)
        {
            case Mode.editor:
                editor_mode(next_level);
            break;

            case Mode.movement_test:
                movement_test_mode() ;
            break;

            default:
                Debug.Log("ControllerManager.swap: default, "+nextmode);
            break;
        }

        swap_on_update = false;
    }

    /// <summary>
    /// Destroys current controllers
    /// </summary>
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

    /// <summary>
    /// Setup for movement test
    /// </summary>
    void movement_test_mode()
    {
        XMLO_LL_Level xml;

        if (!LevelLoader.loadFile("mv_t", out xml))
        {
            Debug.Log("Runner, Somthing went wrong while loading a level.");
            return;
        }


        logicControl = new TestingLogicControl(xml, "mv_t");
        saveControl = new NoSaveControl();
        graphicsControl = new TestingGraphicsControl();
        inputControl = new TestingInputControl();
        uiControl = new NoUIControl();
        lightingControl = new NoLightingControl();

        saveControl.delayedConstruction();
        inputControl.delayedConstruction();
        graphicsControl.delayedConstruction();
        logicControl.delayedConstruction();
        uiControl.delayedConstruction();
        lightingControl.delayedConstruction();
    }

    /// <summary>
    /// Setup for level editor mode
    /// </summary>
    void editor_mode()
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

    /// <summary>
    /// Setup for level editor mode from loaded level
    /// </summary>
    void editor_mode(string filename)
    {
        if (filename == "VOID")
        {
            editor_mode();
            return;
        }

        XMLO_LL_Level xml;

        if (!LevelLoader.loadFile(filename, out xml))
        {
            Debug.Log("Runner, Somthing went wrong while loading a level.");
            return;
        }
        //Debug.Log(xml.ToString());

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
