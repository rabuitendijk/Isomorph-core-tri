
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUI_InputField {

    public string content { get { return inputField.text; }  }
    public bool beingEdited { get { return inputField.isFocused; } }
    RectTransform source, root;
    InputField inputField;

    public HUI_InputField(RectTransform source, Vector2 min, Vector2 max, Font font, bool bestFit = false)
    {
        this.source = source;

        root = HUI.buildUIObject("InputField", source, min, max, new Vector2(.5f, .5f));
        inputField = HUI.addInputField(root, Color.white, font, "Enter filename here...", bestFit);
    }

    public HUI_InputField(RectTransform source, Vector2 min, Vector2 max, Font font, Color textColor, bool bestFit=false)
    {
        this.source = source;

        root = HUI.buildUIObject("InputField", source, min, max, new Vector2(.5f, .5f));
        inputField = HUI.addInputField(root, textColor, font, "Enter filename here...", bestFit);
    }

    public void registerOnEndEdit(UnityAction<string> call)
    {
        inputField.onEndEdit.AddListener(call);
    }

    public void clear()
    {
        inputField.text = "";
    }

    public void destroy()
    {
        GameObject.Destroy(root);
    }
}
