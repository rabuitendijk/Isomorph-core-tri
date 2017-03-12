
using System;
using System.Collections.Generic;
using UnityEngine;

public class EditorInputControl : InputControl {
    GameObject levelEditor, listPrototype;

    /// <summary>
    /// Create the common level input controller
    /// </summary>
    public EditorInputControl() : base()
    {
        componentCamera = new BasicComponentCamera();
        componentMouse = new BasicComponentMouse();
        GameObject canvas = GameObject.Find("/Canvas");
        levelEditor = getObjectFromObject(canvas, "LevelEditor");
        levelEditor.SetActive(true);

        listPrototype = getObjectFromObject(getObjectFromObject(getObjectFromObject(levelEditor, "LeftBox"), "ScrollList"), "Prototype");
    }

    /// <summary>
    /// Input processing, run this once per frame
    /// </summary>
    public override void update()
    {
        componentCamera.update();
        Tile t;
        componentMouse.update(out t);
    }

    GameObject getObjectFromObject(GameObject parent, string name)
    {
        for (int i=0; i< parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name == name)
                return parent.transform.GetChild(i).gameObject;
        }

        Debug.Log("getObjectFromObject ob not found: "+name);
        return null;
    }

    protected override void destructor()
    {
        levelEditor.SetActive(false);
        return;
    }
}
