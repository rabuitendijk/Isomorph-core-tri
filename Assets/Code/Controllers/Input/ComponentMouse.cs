
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// version alpha-1
/// 
/// Controls actions for the mouse
/// 
/// Likely to be alterd to function fully on callbacks
/// 
/// Robin Apollo Butiendijk
/// Early March 2017
/// </summary>
public abstract class ComponentMouse {

    /// <summary>
    /// returns true if cell a tile is currently hovered over and then set t to seleced tile.
    /// </summary>
    public abstract void update();

    /// <summary>
    /// For UI Based callback with onClick
    /// </summary>
    public abstract void onClick(string mode);
}
