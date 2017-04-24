
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A mouse click blocking script with no other functionallity
/// </summary>
public class HUI_MouseTrap : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        return;
    }
}
