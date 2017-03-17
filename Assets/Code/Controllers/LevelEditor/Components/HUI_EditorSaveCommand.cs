using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class HUI_EditorSaveCommand : HUI_ConsoleCommand
{
    static Action<string> save;
    public static void registerSave(Action<string> funct) { save += funct; }
    HUI_ConsoleProcessor processor;

    public HUI_EditorSaveCommand(HUI_ConsoleProcessor processor) : base("save")
    {
        this.processor = processor;
    }

    public override void flush()
    {
        save = null;
    }

    public override string help()
    {
        return "<color=grey>[filename]</color>, saves file to filename.xml.";
    }

    public override void process(string[] args)
    {
        if (save == null)
        {
            processor.console.textBox.append("<color=red><ERROR> no callback to save function.</color>\n");
            return;
        }

        if (args.Length != 2 || args[1] == "")
        {
            processor.console.textBox.append("<color=red><ERROR> incorect number of arguments, expects: filename.</color>\n");
            return;
        }

        if (!Directory.Exists(processor.filepath + "/Levels"))
            Directory.CreateDirectory(processor.filepath + "/Levels");

        save(processor.filepath+"/Levels/"+args[1]);
        processor.console.textBox.append("File saved to "+ processor.filepath + "/Levels/" + args[1] + ".xml\n");
    }
}
