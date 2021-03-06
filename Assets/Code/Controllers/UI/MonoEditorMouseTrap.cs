﻿
using UnityEngine.EventSystems;
using System;


namespace UI_C
{
    /// <summary>
    /// This absorbs all click not stopped by the UI
    /// </summary>
    public class MonoEditorMouseTrap : UnityEngine.MonoBehaviour, IPointerClickHandler
    {

        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick == null)
                return;

            if (eventData.pointerId == -1)
                onClick("left");
            else if (eventData.pointerId == -3)
                onClick("middle");
            else if (eventData.pointerId == -2)
                onClick("right");

        }

        Action<string> onClick;

        public void registerOnClick(Action<string> funct)
        {
            onClick += funct;
        }

        public void removeOnClick(Action<string> funct)
        {
            onClick -= funct;
        }
    }
}