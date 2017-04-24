
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A list of nodes where one or no node can be selected
/// </summary>
public class HUI_SelectorList  {

	public string selected { get; protected set; }
    RectTransform root;
    public RectTransform scrollList { get; protected set; }
    HUI_TextNode textNode;
    //Font font;

        /// <summary>
        /// Constructor that build list imidiatly
        /// </summary>
    public HUI_SelectorList(RectTransform source, SortedList<string, IsoObjectBody> nodes, Vector2 min, Vector2 max, Font font)
    {
        selected = "VOID";
        //this.source = source;
        //this.font = font;

        HUI_NodeClick.registerOnClick(changeSelected);

        //Build rects
        root = HUI.buildUIObject("LeftBox", source, min, max, new Vector2(.5f, .5f));
        scrollList = HUI.buildUIObject("ScrollList", root, new Vector2(0f, .0f), new Vector2(1f, 1f), new Vector2(0f, 1f));

        //Root
        HUI.addImage(root, new Color(.2f, .2f, .2f, .6f));
        root.gameObject.AddComponent<Mask>();
        HUI.addScrollRect(root, scrollList, false);
        root.gameObject.AddComponent<HUI_MouseTrap>();

        //ScrollList
        HUI.addVerticalLayoutGroup(scrollList, new RectOffset(2, 2, 2, 2), 2);
        HUI.addContentSizeFitter(scrollList);

        //HUI_TextNode n;
        for (int i = 0; i < nodes.Count; i++)
        {
            new HUI_TextNode(scrollList, Atlas_Loader.main.objectsList.Values[i].name, font);
        }
    }

    /// <summary>
    /// Change the appearance of the newly selceded node and cleas up the old selection.
    /// </summary>
    /// <param name="node"></param>
    private void changeSelected(HUI_TextNode node)
    {
        selected = node.name;

        if (textNode != null)
            textNode.image.color = HUI_TextNode.basicColor;

        textNode = node;
        node.image.color = HUI_TextNode.selectedColor;

        if (onChangeSelected != null)   //If needed notify outside world
            onChangeSelected(node.name);
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    public void destroy()
    {
        GameObject.Destroy(root.gameObject);
        HUI_NodeClick.removeOnClick(changeSelected);
    }

    Action<string> onChangeSelected;
    public void registerOnChangeSelected(Action<string> funct) { onChangeSelected += funct; }
    public void removeOnChangeSelected(Action<string> funct) { onChangeSelected -= funct; }

}
