
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
    string text;

    /// <summary>
    /// Set text
    /// </summary>
	public EditorComponentUIListNode(EditorComponentUI control, string text)
    {
        this.control = control;
        this.text = text;
    }

    /// <summary>
    /// Construct grahpic repesentation of this node
    /// </summary>
    public void construct()
    {
        graphic = new GameObject() { name = "ListNode" };
        RectTransform rect = graphic.AddComponent<RectTransform>() as RectTransform;
        rect.sizeDelta = new Vector2(0, height);
        rect.SetParent(control.scrollList.transform);

        image = graphic.AddComponent<Image>();
        image.color = basicColor;

        MonoUIListNodeClick click = graphic.AddComponent<MonoUIListNodeClick>();
        click.node = this;

        GameObject textOb = new GameObject() { name = "text" };
        RectTransform textRect = textOb.AddComponent<RectTransform>() as RectTransform;
        textComponent = textOb.AddComponent<Text>() as Text;
        textComponent.text = text;
        textComponent.raycastTarget = false;
        textComponent.alignment = TextAnchor.MiddleLeft;
        textComponent.fontSize = 32;
        textComponent.font = Runner.main.ariel;
        textComponent.color = Color.black;
        textComponent.resizeTextForBestFit = true;
        textOb.transform.SetParent(graphic.transform);
        setRectFull(textRect);
        textRect.anchorMin = new Vector2(.05f, 0f);
        
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

