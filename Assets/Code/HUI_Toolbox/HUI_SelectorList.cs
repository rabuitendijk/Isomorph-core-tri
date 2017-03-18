
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUI_SelectorList  {

	public string selected { get; protected set; }
    RectTransform source, root;
    public RectTransform scrollList { get; protected set; }
    HUI_TextNode textNode;
    Font font;

    public HUI_SelectorList(RectTransform source, SortedList<string, IsoObject> nodes, Vector2 min, Vector2 max, Font font)
    {
        selected = "VOID";
        this.source = source;
        this.font = font;

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

        HUI_TextNode n;
        for (int i = 0; i < nodes.Count; i++)
        {
            n = new HUI_TextNode(scrollList, AliasXMLLoader.main.objects.Values[i].name, font);
        }
    }

    private void changeSelected(HUI_TextNode node)
    {
        selected = node.name;

        if (textNode != null)
            textNode.image.color = HUI_TextNode.basicColor;

        textNode = node;
        node.image.color = HUI_TextNode.selectedColor;
    }

    public void destroy()
    {
        GameObject.Destroy(root.gameObject);
        HUI_NodeClick.removeOnClick(changeSelected);
    }


}
