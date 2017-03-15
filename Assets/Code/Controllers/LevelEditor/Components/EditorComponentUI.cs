
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

    public RectTransform levelEditor, scrollList;
    List<EditorComponentUIListNode> nodes = new List<EditorComponentUIListNode>();
    EditorComponentUIListNode selected = null;
    MonoEditorMouseTrap mouse;

    /// <summary>
    /// Detects canvas and desired UI structure
    /// </summary>
    public EditorComponentUI()
    {
        MonoUIListNodeClick.registerOnClick(onClick);

        construct();
        EditorComponentUIListNode n;
        for (int i = 0; i < AliasXMLLoader.main.objects.Count; i++)
        {
            n = new EditorComponentUIListNode(this, AliasXMLLoader.main.objects.Values[i].name);
            nodes.Add(n);
            n.construct();
        }

    }

    void onClick(EditorComponentUIListNode node)
    {
        if (selected != null)
            selected.image.color = EditorComponentUIListNode.basicColor;

        selected = node;
        node.image.color = EditorComponentUIListNode.selectedColor;
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
        levelEditor = UIHelp.buildUIObject("LevelEditor", canvas.transform);
        RectTransform leftBox = UIHelp.buildUIObject("LeftBox", levelEditor, new Vector2(0f, .05f), new Vector2(.2f, 1f), new Vector2(.5f, .5f));
        RectTransform inputField = UIHelp.buildUIObject("InputField", levelEditor, new Vector2(0f, .0f), new Vector2(.2f, 0.05f), new Vector2(.5f, .5f));
        scrollList = UIHelp.buildUIObject("ScrollList", leftBox, new Vector2(0f, .0f), new Vector2(1f, 1f), new Vector2(0f, 1f));

        //Level Editor
        UIHelp.addImage(levelEditor, new Color(1f, 1f, 1f, 0f));
        mouse = levelEditor.gameObject.AddComponent<MonoEditorMouseTrap>();

        //LeftBox
        UIHelp.addImage(leftBox, new Color(.2f, .2f, .2f, .6f));
        leftBox.gameObject.AddComponent<Mask>();
        UIHelp.addScrollRect(leftBox, scrollList, false);

        //InputField
        UIHelp.addInputField(inputField, Color.white, Runner.main.ariel, "Enter filename here...");

        //ScrollList
        UIHelp.addVerticalLayoutGroup(scrollList, new RectOffset(2, 2, 2, 2), 2);
        UIHelp.addContentSizeFitter(scrollList);

    }

    public void registerOnClick(Action<string> funct){ mouse.registerOnClick(funct); }
    public void removeOnClick(Action<string> funct) { mouse.removeOnClick(funct); }
    public string getSelectedObject()
    {
        if (selected == null)
            return "VOID";
        return selected.name;
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    public void destructor()
    {

        GameObject.Destroy(levelEditor);

        MonoUIListNodeClick.removeOnClick(onClick);

    }
}
