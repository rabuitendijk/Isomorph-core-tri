
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
public class EditorUIControl : UIControl{

    RectTransform levelEditor;
    HUI_SelectorList selectorList;
    HUI_Console console;
    MonoEditorMouseTrap mouse;

    /// <summary>
    /// Detects canvas and desired UI structure
    /// </summary>
    public EditorUIControl() : base()
    {
        construct();
        selectorList = new HUI_SelectorList(levelEditor, Atlas_Loader.main.objectsList, new Vector2(0f, .0f), new Vector2(.12f, 1f), Runner.main.ariel);
        selectorList.registerOnChangeSelected(changeSelected);
        console = new HUI_Console(levelEditor, new Vector2(.5f, 0f), new Vector2(1f, .3f), Runner.main.ariel, new HUI_EditorProcessor());

        mouse.registerOnClick(onMouseClick);
    }

    public override void delayedConstruction()
    {
        //Empty
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

    static Action<string> onChangeSelected;
    public static void registerOnChangeSelected(Action<string> funct) { onChangeSelected += funct; }
    public static void removeOnChangeSelected(Action<string> funct) { onChangeSelected -= funct; }

    void changeSelected(string name) { if (onChangeSelected != null) { onChangeSelected(name); } }//Push outward

    /// <summary>
    /// Destroy this object
    /// </summary>
    protected override void destructor()
    {
        mouse.removeOnClick(onMouseClick);
        selectorList.removeOnChangeSelected(changeSelected);
        selectorList.destroy();
        console.destroy();
        GameObject.Destroy(levelEditor.gameObject);

    }

    public override bool usesKeys()
    {
        return console.beingEdited;
    }
}
