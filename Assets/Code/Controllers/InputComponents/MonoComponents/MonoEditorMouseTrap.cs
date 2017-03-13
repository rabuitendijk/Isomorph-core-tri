
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MonoEditorMouseTrap : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick();
    }

    Action onClick;

    public void registerOnClick(Action funct)
    {
        onClick += funct;
    }

    public void removeOnClick(Action funct)
    {
        onClick -= funct;
    }
}
