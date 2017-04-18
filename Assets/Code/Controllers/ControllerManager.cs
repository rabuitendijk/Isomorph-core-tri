using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public ControllerManager()
    {
        main = this;
        editor_mode("VOID");
    }

    public void setNextSwap(Mode mode, string level = "VOID")
    {

        swap_on_update = true;
        nextmode = mode;
        next_level = level;
    }

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

    void movement_test_mode()
    {
        Level_XML xml;

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

    void editor_mode(string filename)
    {
        if (filename == "VOID")
        {
            editor_mode();
            return;
        }

        Level_XML xml;

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
