
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version alpha-1
/// 
/// Editor component that manages its UI
/// 
/// Robin Apollo Butiendijk
/// Early March 2017
/// </summary>
public class EditorComponentUI {

    public GameObject levelEditor, scrollList;
    List<EditorComponentUIListNode> nodes = new List<EditorComponentUIListNode>();

    /// <summary>
    /// Detects canvas and desired UI structure
    /// </summary>
    public EditorComponentUI()
    {
        GameObject canvas = GameObject.Find("/Canvas");
        levelEditor = getObjectFromObject(canvas, "LevelEditor");
        levelEditor.SetActive(true);

        scrollList = getObjectFromObject(getObjectFromObject(levelEditor, "LeftBox"), "ScrollList");
        nodes.Add(new EditorComponentUIListNode(this, "Bullshit"));
        nodes[0].construct();
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
    /// Destroy this object
    /// </summary>
    public void destructor()
    {
        foreach(EditorComponentUIListNode n in nodes)
        {
            n.graphic = null;
        }
        levelEditor.SetActive(false);
    }
}
