
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

/// <summary>
/// version aplha-1
/// 
/// A generic ui console
/// 
/// Robin Apollo Buitendijk
/// Early March 2017
/// </summary>
public class HUI_Console  {

    public static HUI_Console main;

    public string filepath;

    RectTransform root;
    HUI_InputField inputField;

    public string content { get { return inputField.content; } }
    public bool beingEdited { get { return inputField.beingEdited; } }

    public HUI_ConsoleProcessor processor { get; protected set; }
    public HUI_TextBox textBox { get; protected set; }

    /// <summary>
    /// Constructor imidiatly builds the console
    /// </summary>
    public HUI_Console(RectTransform source, Vector2 min, Vector2 max, Font font, HUI_ConsoleProcessor processor=null)
    {
        main = this;
        filepath = Application.streamingAssetsPath;

        //this.source = source;
        root = HUI.buildUIObject("Console", source, min, max, new Vector2(.5f, .5f));

        textBox = new HUI_TextBox(root, new Vector2(0f, .06f), new Vector2(1f, 1f), font);
        inputField = new HUI_InputField(root, new Vector2(0f, 0f), new Vector2(1f, .06f), font, Color.black, true);

        if (processor == null)
            processor = new HUI_Prosessor_Basic();
        else
            this.processor = processor;


        textBox.setText("Console version <i>aplha-1</i>.\nDate: <i>Middle March 2017</i>\n");
        inputField.registerOnEndEdit(delegate { process(processor, inputField); });
    }

    /// <summary>
    /// Process commands on entry [of commands]
    /// </summary>
    void process(HUI_ConsoleProcessor processor, HUI_InputField inputField)
    {
        processor.process();
        inputField.clear();
    }

    /// <summary>
    /// Destroy the object
    /// </summary>
    public void destroy()
    {
        GameObject.Destroy(root.gameObject);
        processor.destroy();
        main = null;
    }
}
