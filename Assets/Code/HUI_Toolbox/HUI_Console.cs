
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUI_Console  {

    RectTransform source, root;
    HUI_TextBox textBox;
    HUI_InputField inputField;
    public string content { get { return inputField.content; } }
    public bool beingEdited { get { return inputField.beingEdited; } }


    public HUI_Console(RectTransform source, Vector2 min, Vector2 max, Font font)
    {
        this.source = source;
        root = HUI.buildUIObject("Console", source, min, max, new Vector2(.5f, .5f));

        textBox = new HUI_TextBox(root, new Vector2(0f, .06f), new Vector2(1f, 1f), font);
        inputField = new HUI_InputField(root, new Vector2(0f, 0f), new Vector2(1f, .06f), font, Color.black, true);
    }


    public void destroy()
    {
        GameObject.Destroy(root);
    }
}
