
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controllers of a given type aways exist and control an aspect of the games behaviour
/// </summary>
public interface Controller  {

    /// <summary>
    /// Destroy this object
    /// </summary>
    void destroy();

    /// <summary>
    /// Runs after all regular controller constructors have been ran.
    /// </summary>
    void delayedConstruction();
}
