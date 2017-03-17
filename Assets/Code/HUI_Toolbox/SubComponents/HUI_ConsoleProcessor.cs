
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Processes console commands
/// </summary>
public abstract class HUI_ConsoleProcessor  {

    public List<HUI_ConsoleCommand> commands;
    public HUI_Console console;
    public string filepath;

    /// <summary>
    /// Constructor, runs loadCommands.
    /// Adds help and clear
    /// </summary>
    public HUI_ConsoleProcessor(HUI_Console console)
    {
        filepath = Application.streamingAssetsPath;
        commands = loadCommands();
        this.console = console;

        commands.Add(new HUI_Command_Clear(this));
        commands.Add(new HUI_Command_Help(this));
        commands.Add(new HUI_Command_FilePath(this));
    }

    /// <summary>
    /// Impelment the load commands to add new Console commands
    /// </summary>
    protected abstract List<HUI_ConsoleCommand> loadCommands();

    /// <summary>
    /// Processes the commands
    /// </summary>
    public void process()
    {
        string[] args = console.content.Split(' ');

        if (args.Length == 0 || args[0] == "")
            return;

        foreach (HUI_ConsoleCommand c in commands)
        {
            if (args[0] == c.name)
            {
                c.process(args);
                return;
            }
        }

        console.textBox.append("["+args[0]+"], not fonud.");
    }

    /// <summary>
    /// Destroy this object, flushes all registeries
    /// </summary>
    public void destroy()
    {
        foreach (HUI_ConsoleCommand c in commands)
        {
            c.flush();
        }
    }
}
