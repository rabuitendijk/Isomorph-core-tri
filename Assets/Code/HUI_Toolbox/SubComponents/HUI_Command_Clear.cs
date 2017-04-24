using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Console command that clear the console
/// </summary>
public class HUI_Command_Clear : HUI_ConsoleCommand
{

    public HUI_Command_Clear():base("clear")
    {
        //Empty
    }

    public override string help()
    {
        return "clears the console.";
    }

    /// <summary>
    /// Emptys texbox
    /// </summary>
    public override void process(string[] args)
    {
        HUI_Console.main.textBox.setText("");
    }
}
