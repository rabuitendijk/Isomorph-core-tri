
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls actions for the mouse
/// </summary>
public abstract class ComponentMouse {

    /// <summary>
    /// returns true if cell a tile is currently hovered over and then set t to seleced tile.
    /// </summary>
    public abstract bool update(out Tile t);


}
