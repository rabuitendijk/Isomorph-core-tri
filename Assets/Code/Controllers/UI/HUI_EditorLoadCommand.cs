
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class HUI_EditorLoadCommand : HUI_ConsoleCommand
{
    static Action<string> load;
    public static void registerLoad(Action<string> funct) { load += funct; }
    public static void removeLoad(Action<string> funct) { load -= funct; }

    public HUI_EditorLoadCommand() : base("load")
    {
        //Empty
    }

    public override string help()
    {
        return "<color=grey>filename</color>, loads level from filename.";
    }

    public override void process(string[] args)
    {
        if (load == null)
        {
            HUI_Console.main.textBox.append("<color=red><ERROR> no callback to load function.</color>\n");
            return;
        }

        if (args.Length != 2 || args[1] == "")
        {
            HUI_Console.main.textBox.append("<color=red><ERROR> incorect number of arguments, expects: filename.</color>\n");
            return;
        }

        if (!File.Exists(HUI_Console.main.filepath + "/Levels/"+args[1]+".xml"))
        {
            HUI_Console.main.textBox.append("<color=red><ERROR> file does not exist: "+ HUI_Console.main.filepath + "/Levels/" + args[1] + ".xml" + ".</color>\n");
            return;
        }

        load(args[1]);
    }
}
