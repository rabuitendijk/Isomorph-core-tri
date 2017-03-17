﻿
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// version alpha-1
/// 
/// Editor component that manages its UI
/// 
/// Robin Apollo Butiendijk
/// Early March 2017
/// </summary>
public class EditorComponentUI {

    RectTransform levelEditor;
    HUI_SelectorList selectorList;
    HUI_InputField inputField;
    HUI_Console console;
    MonoEditorMouseTrap mouse;
    public bool beingEdited { get { return console.beingEdited; } }

    /// <summary>
    /// Detects canvas and desired UI structure
    /// </summary>
    public EditorComponentUI()
    {

        construct();
        selectorList = new HUI_SelectorList(levelEditor, AliasXMLLoader.main.objects, new Vector2(0f, .0f), new Vector2(.12f, 1f), Runner.main.ariel);
        console = new HUI_Console(levelEditor, new Vector2(.5f, 0f), new Vector2(1f, .3f), Runner.main.ariel);
    }

    /// <summary>
    /// Get an inactive gameobject
    /// </summary>
    GameObject getObjectFromObject(GameObject parent, string name)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name == name)
                return parent.transform.GetChild(i).gameObject;
        }

        Debug.Log("getObjectFromObject ob not found: " + name);
        return null;
    }

    /// <summary>
    /// Construct UI
    /// </summary>
    public void construct()
    {
        GameObject canvas = GameObject.Find("/Canvas");

        //Build rects
        levelEditor = HUI.buildUIObject("LevelEditor", canvas.transform);

        //Level Editor
        HUI.addImage(levelEditor, new Color(1f, 1f, 1f, 0f));
        mouse = levelEditor.gameObject.AddComponent<MonoEditorMouseTrap>();

    }

    public void registerOnClick(Action<string> funct){ mouse.registerOnClick(funct); }
    public void removeOnClick(Action<string> funct) { mouse.removeOnClick(funct); }
    public string getSelectedObject()
    {
        return selectorList.selected;
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    public void destructor()
    {
        selectorList.destroy();
        console.destroy();
        inputField.destroy();
        GameObject.Destroy(levelEditor);

    }
}