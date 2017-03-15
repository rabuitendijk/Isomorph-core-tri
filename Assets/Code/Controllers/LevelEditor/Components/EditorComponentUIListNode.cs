
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A UI list node
/// </summary>
public class EditorComponentUIListNode {

    int height = 16;
    EditorComponentUI control;
    public Text textComponent { get; protected set; }
    public Image image { get; protected set; }
    GameObject Graphic;
    public string name { get; protected set; }
    public static Color basicColor = new Color(1f, 1f, 1f, .5f), selectedColor = new Color(.5f, .5f, 1f, .8f);

    public GameObject graphic
    {
        get
        {
            return Graphic;
        }
        set
        {
            if (graphic != null)
                GameObject.Destroy(Graphic);
            Graphic = value;
        }
    }

    /// <summary>
    /// Set text
    /// </summary>
	public EditorComponentUIListNode(EditorComponentUI control, string text)
    {
        this.control = control;
        this.name = text;
    }

    /// <summary>
    /// Construct grahpic repesentation of this node
    /// </summary>
    public void construct()
    {
        RectTransform rect = UIHelp.buildUIObject("ListNode", control.scrollList);
        graphic = rect.gameObject;
        rect.sizeDelta = new Vector2(0, height);

        image = UIHelp.addImage(rect, basicColor);

        MonoUIListNodeClick click = graphic.AddComponent<MonoUIListNodeClick>();
        click.node = this;

        textComponent = UIHelp.addTextChild(rect, Color.black, Runner.main.ariel, 32, new Vector2(.05f, 0f), new Vector2(1f, 1f), name, true);
        
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
}

