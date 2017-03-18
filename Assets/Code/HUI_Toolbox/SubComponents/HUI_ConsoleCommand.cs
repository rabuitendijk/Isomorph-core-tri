
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Processes a command with args
/// </summary>
public abstract class HUI_ConsoleCommand {
    public string name { get; protected set; }

    public HUI_ConsoleCommand(string name)
    {
        this.name = name;
    }

    /// <summary>
    /// Execute argument
    /// </summary>
    public abstract void process(string[] args);

    /// <summary>
    /// Get help
    /// </summary>
    public abstract string help();
}
