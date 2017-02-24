using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// version alpha-1
/// A hash loader saves Sprites under names in Directory
/// 
/// Notes:
/// Outdated only use for quick tests, use asset steaming instead.
///
/// Robin Apollo Buitendijk
/// Late Febuary 2017
/// </summary>
public class SpriteHashLoader : MonoBehaviour {
    public Sprite[] sprites;
    public string[] names;

    public Dictionary<int, Sprite> table = new Dictionary<int, Sprite>();

	/// <summary>
    /// Manual run before use because of loading issues (something with loading on theads not allowed).
    /// </summary>
	public void onStartup () {
        if (sprites.Length != names.Length) {
            Debug.Log("SpriteHashLoader.Start(), diffrent number of sprites than names");
            return;
        }


        for (int i= 0; i< sprites.Length; i++) {
            try
            {
                table.Add(names[i].GetHashCode(), sprites[i]);
            }
            catch (Exception e){
                Debug.Log(e);
            }
        }
	}

    /// <summary>
    /// Trys to get Sprite or returns null
    /// </summary>
    /// <param name="name">name of target Sprite</param>
    /// <returns>the Sprite</returns>
    public Sprite getSprite(string name) {
        Sprite sp;
        if (table.TryGetValue(name.GetHashCode(), out sp)) {
            return sp;
        }

        Debug.Log("Sprite: "+name+", not found");
        return null;
    }
	
}
