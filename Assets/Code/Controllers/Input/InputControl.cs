
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version alpha-1
/// 
/// Accesor functions for input;
/// 
/// Robin Apollo Buitendijk
/// Early March 2017
/// </summary>
public abstract class InputControl : Controller {

    public static InputControl main;

    protected ComponentCamera componentCamera;
    protected ComponentMouse componentMouse;
    /// <summary>
    /// Sets common variables and registers callbacks
    /// </summary>
	public InputControl()
    {
        main = this;
        UIControl.registerOnMouseClick(onClick);
    }

    public abstract void delayedConstruction();

    /// <summary>
    /// Respond to the mouse clicks passing the ui
    /// </summary>
    protected abstract void onClick(string mode);

    /// <summary>
    /// Processes all mouse, keyboard and controller input
    /// </summary>
    public abstract void update();

    /// <summary>
    /// Destroys inherentring object, automatically called by base.destroy();
    /// </summary>
    protected abstract void destructor();

    /// <summary>
    /// Deregisters callbacks and cleans up so that the input controller can be overwritten in the Runner.
    /// </summary>
    public void destroy()
    {
        UIControl.removeOnMouseClick(onClick);
        destructor();
    }
}
