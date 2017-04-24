
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Console command that prints all help for a processors list of commands.
/// </summary>
public class HUI_Command_Help : HUI_ConsoleCommand
{


    public HUI_Command_Help(): base("help")
    {
        //Empty
    }

    public override string help()
    {
        return "<b>is help.</b>";
    }

    public override void process(string[] args)
    {
        string ret = "\n<color=cyan>All help</color>\n";
        foreach (HUI_ConsoleCommand c in HUI_Console.main.processor.commands)
        {
            ret += "\t-\t["+c.name+"], "+c.help() + "\n";
        }

        HUI_Console.main.textBox.append(ret);
    }
}
