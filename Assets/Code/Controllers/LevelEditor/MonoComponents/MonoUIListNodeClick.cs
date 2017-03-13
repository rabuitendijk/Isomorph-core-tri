
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonoUIListNodeClick : MonoBehaviour, IPointerClickHandler
{
    public EditorComponentUIListNode node;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (node != null)
            onClick(node);
    }

    static Action<EditorComponentUIListNode> onClick;

    public static void registerOnClick(Action<EditorComponentUIListNode> funct) { onClick += funct; }
    public static void removeOnClick(Action<EditorComponentUIListNode> funct) { onClick -= funct; }

}
