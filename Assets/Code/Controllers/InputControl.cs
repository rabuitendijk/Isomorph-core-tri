
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
public abstract class InputControl {

    public static InputControl main;


	public InputControl()
    {
        main = this;
    }

    public abstract void update();
}
