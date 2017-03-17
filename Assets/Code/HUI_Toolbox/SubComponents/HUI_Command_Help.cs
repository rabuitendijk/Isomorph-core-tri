
using System;
using System.Collections.Generic;
using UnityEngine;

public class HUI_Command_Help : HUI_ConsoleCommand
{
    HUI_ConsoleProcessor processor;
    
    /// <summary>
    /// No flush needed
    /// </summary>
    public override void flush()
    {
        return;
    }

    public HUI_Command_Help(HUI_ConsoleProcessor processor): base("help")
    {
        this.processor = processor;
    }

    public override string help()
    {
        return "<b>is help.</b>";
    }

    public override void process(string[] args)
    {
        string ret = "\n<color=cyan>All help</color>\n";
        foreach (HUI_ConsoleCommand c in processor.commands)
        {
            ret += "\t-\t["+c.name+"], "+c.help() + "\n";
        }

        processor.console.textBox.append(ret);
    }
}
