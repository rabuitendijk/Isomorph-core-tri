using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUI_Command_FilePath : HUI_ConsoleCommand
{
    HUI_ConsoleProcessor processor;

    public HUI_Command_FilePath(HUI_ConsoleProcessor processor) : base("filepath")
    {
        this.processor = processor;
    }

    public override void flush()
    {
        //Not needed
    }

    public override string help()
    {
        return processor.filepath;
    }

    public override void process(string[] args)
    {
        processor.console.textBox.append(processor.filepath+"\n");
    }
}
