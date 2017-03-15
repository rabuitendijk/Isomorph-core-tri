
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

    public GameObject levelEditor, leftBox, scrollList;
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
        levelEditor = new GameObject() { name = "LevelEditor"};
        RectTransform rectLE = levelEditor.AddComponent<RectTransform>() as RectTransform;
        rectLE.SetParent(canvas.transform);
        setRectFull(rectLE);

        leftBox = new GameObject() { name = "LeftBox" };
        RectTransform rectLB = leftBox.AddComponent<RectTransform>() as RectTransform;
        rectLB.SetParent(levelEditor.transform);
        setRectFull(rectLB);

        scrollList = new GameObject() { name = "ScrollList" };
        RectTransform rectSL = scrollList.AddComponent<RectTransform>() as RectTransform;
        rectSL.SetParent(leftBox.transform);
        setRectFull(rectSL);

        //Level Editor
        Image i = levelEditor.AddComponent<Image>();
        i.color = new Color(1f, 1f, 1f, 0f);
        mouse = levelEditor.AddComponent<MonoEditorMouseTrap>();

        //LeftBox
        rectLB.anchorMax = new Vector2(.2f, 1f);
        i = leftBox.AddComponent<Image>() as Image;
        i.color = new Color(.2f, .2f, .2f, .6f);
        leftBox.AddComponent<Mask>();
        ScrollRect sr = leftBox.AddComponent<ScrollRect>() as ScrollRect;
        sr.horizontal = false;
        sr.content = scrollList.GetComponent<RectTransform>();
        sr.scrollSensitivity = 15f;

        //ScrollList
        rectSL.pivot = new Vector2(0f, 1f);
        VerticalLayoutGroup vlg = scrollList.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(2, 2, 2, 2);
        vlg.childControlHeight = false;
        vlg.childForceExpandHeight = false;
        vlg.childAlignment = TextAnchor.UpperLeft;
        vlg.spacing = 2;
        ContentSizeFitter csf = scrollList.AddComponent<ContentSizeFitter>();
        csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

    }

    void setRectFull(RectTransform rect)
    {
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(.5f, .5f);
        rect.sizeDelta = new Vector2(0f, 0f);
        rect.position = new Vector3(0f, 0f, 0f);
        rect.offsetMax = new Vector2(0f, 0f);
        rect.offsetMin = new Vector2(0f, 0f);
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

        foreach (EditorComponentUIListNode n in nodes)
        {
            n.graphic = null;
        }


        GameObject.Destroy(scrollList);
        GameObject.Destroy(leftBox);
        GameObject.Destroy(levelEditor);

        MonoUIListNodeClick.removeOnClick(onClick);

    }
}
