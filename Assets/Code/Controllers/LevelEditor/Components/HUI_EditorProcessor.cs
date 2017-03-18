
using System;
using System.Collections.Generic;
using UnityEngine;

public class HUI_EditorProcessor : HUI_ConsoleProcessor
{
    public HUI_EditorProcessor() 
    {
        //Empty
    } 

    protected override List<HUI_ConsoleCommand> loadCommands()
    {
        List<HUI_ConsoleCommand> ret = new List<HUI_ConsoleCommand>();
        ret.Add(new HUI_EditorSaveCommand());
        ret.Add(new HUI_EditorLoadCommand());
        return ret;
    }
}
