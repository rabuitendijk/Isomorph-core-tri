
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUI_TextNode  {
    public static Color basicColor = new Color(1f, 1f, 1f, .5f), selectedColor = new Color(.5f, .5f, 1f, .8f);

    int height = 16;
    public Image image { get; protected set; }
    Text text;
    public string name { get; protected set; }
    RectTransform source, root;

    public HUI_TextNode(RectTransform source, string name, Font font)
    {
        this.source = source;
        this.name = name;

        root = HUI.buildUIObject("ListNode", source);
        root.sizeDelta = new Vector2(0, height);

        image = HUI.addImage(root, basicColor);

        HUI_NodeClick click = root.gameObject.AddComponent<HUI_NodeClick>();
        click.node = this;

        text = HUI.addTextChild(root, Color.black, font, 32, new Vector2(.05f, 0f), new Vector2(1f, 1f), name, true);
    }


}
