
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// An editable text field
/// </summary>
public class HUI_InputField {

    public string content { get { return inputField.text; }  }
    public bool beingEdited { get { return inputField.isFocused; } }
    RectTransform root;
    InputField inputField;

    /// <summary>
    /// Costructor that build inputfield
    /// </summary>
    public HUI_InputField(RectTransform source, Vector2 min, Vector2 max, Font font, bool bestFit = false)
    {
        //this.source = source;

        root = HUI.buildUIObject("InputField", source, min, max, new Vector2(.5f, .5f));
        inputField = HUI.addInputField(root, Color.white, font, "Enter filename here...", bestFit);
    }

    /// <summary>
    /// Costructor that build inputfield
    /// </summary>
    public HUI_InputField(RectTransform source, Vector2 min, Vector2 max, Font font, Color textColor, bool bestFit=false)
    {
        //this.source = source;

        root = HUI.buildUIObject("InputField", source, min, max, new Vector2(.5f, .5f));
        inputField = HUI.addInputField(root, textColor, font, "Enter filename here...", bestFit);
    }

    /// <summary>
    /// Register a function to be called when a command has been input
    /// </summary>
    public void registerOnEndEdit(UnityAction<string> call)
    {
        inputField.onEndEdit.AddListener(call);
    }

    /// <summary>
    /// Clear this fields text
    /// </summary>
    public void clear()
    {
        inputField.text = "";
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    public void destroy()
    {
        GameObject.Destroy(root.gameObject);
    }
}
