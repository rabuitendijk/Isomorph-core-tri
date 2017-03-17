using System;
using System.Collections.Generic;
using UnityEngine;

public class HUI_Command_Clear : HUI_ConsoleCommand
{
    HUI_ConsoleProcessor processor;

    public HUI_Command_Clear(HUI_ConsoleProcessor processor):base("clear")
    {
        this.processor = processor;
    }

    /// <summary>
    /// No flush needed
    /// </summary>
    public override void flush()
    {
        return;
    }

    public override string help()
    {
        return "clears the console.";
    }

    public override void process(string[] args)
    {
        processor.console.textBox.setText("");
    }
}
