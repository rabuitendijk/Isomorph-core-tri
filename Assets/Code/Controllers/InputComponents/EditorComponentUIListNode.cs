
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A UI list node
/// </summary>
public class EditorComponentUIListNode {

    int height = 16;
    EditorComponentUI control;
    Text textComponent;
    GameObject Graphic;

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

        Image i = graphic.AddComponent<Image>();
        i.color = new Color(1f, 1f, 1f, .5f);

        GameObject textOb = new GameObject() { name = "text" };
        RectTransform textRect = textOb.AddComponent<RectTransform>() as RectTransform;
        textComponent = textOb.AddComponent<Text>() as Text;
        textComponent.text = text;
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.fontSize = 32;
        textComponent.font = Runner.main.ariel;
        textComponent.color = new Color(0f,0f,0f);
        textComponent.resizeTextForBestFit = true;
        textOb.transform.SetParent(graphic.transform);
        setRectFull(textRect);
        
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

