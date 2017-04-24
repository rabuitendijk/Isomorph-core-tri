
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level editor console command processor
/// </summary>
public class HUI_EditorProcessor : HUI_ConsoleProcessor
{
    /// <summary>
    /// Constructor does nothing
    /// </summary>
    public HUI_EditorProcessor() 
    {
        //Empty
    } 

    /// <summary>
    /// Loads all needed commands
    /// </summary>
    protected override List<HUI_ConsoleCommand> loadCommands()
    {
        List<HUI_ConsoleCommand> ret = new List<HUI_ConsoleCommand>();
        ret.Add(new HUI_EditorSaveCommand());
        ret.Add(new HUI_EditorLoadCommand());
        return ret;
    }
}
