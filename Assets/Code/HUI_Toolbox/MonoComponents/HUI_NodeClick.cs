
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUI_NodeClick : MonoBehaviour, IPointerClickHandler{

    public HUI_TextNode node;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (node != null)
            onClick(node);
    }

    static Action<HUI_TextNode> onClick;

    public static void registerOnClick(Action<HUI_TextNode> funct) { onClick += funct; }
    public static void removeOnClick(Action<HUI_TextNode> funct) { onClick -= funct; }
}
