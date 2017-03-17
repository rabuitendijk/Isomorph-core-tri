
using System;
using System.Collections.Generic;
using UnityEngine;

public class HUI_Prosessor_Basic : HUI_ConsoleProcessor
{
    public HUI_Prosessor_Basic(HUI_Console console): base(console)
    {
        //empty on purpose
    }

    protected override List<HUI_ConsoleCommand> loadCommands()
    {
        return new List<HUI_ConsoleCommand>();
    }
}
