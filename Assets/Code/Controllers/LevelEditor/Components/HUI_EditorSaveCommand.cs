using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class HUI_EditorSaveCommand : HUI_ConsoleCommand
{
    static Action<string> save;
    public static void registerSave(Action<string> funct) { save += funct; }
    public static void removeSave(Action<string> funct) { save -= funct; }

    public HUI_EditorSaveCommand() : base("save")
    {
        //Empty
    }

    public override string help()
    {
        return "<color=grey>[filename]</color>, saves file to filename.xml.";
    }

    public override void process(string[] args)
    {
        if (save == null)
        {
            HUI_Console.main.textBox.append("<color=red><ERROR> no callback to save function.</color>\n");
            return;
        }

        if (args.Length != 2 || args[1] == "")
        {
            HUI_Console.main.textBox.append("<color=red><ERROR> incorect number of arguments, expects: filename.</color>\n");
            return;
        }

        if (!Directory.Exists(HUI_Console.main.filepath + "/Levels"))
            Directory.CreateDirectory(HUI_Console.main.filepath + "/Levels");

        save(HUI_Console.main.filepath+"/Levels/"+args[1]);
        HUI_Console.main.textBox.append("File saved to "+ HUI_Console.main.filepath + "/Levels/" + args[1] + ".xml\n");
    }
}
