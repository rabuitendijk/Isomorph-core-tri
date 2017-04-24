﻿
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A console prosessor with no added fuctionallity
/// </summary>
public class HUI_Prosessor_Basic : HUI_ConsoleProcessor
{
    public HUI_Prosessor_Basic()
    {
        //empty on purpose
    }

    protected override List<HUI_ConsoleCommand> loadCommands()
    {
        return new List<HUI_ConsoleCommand>();
    }
}
