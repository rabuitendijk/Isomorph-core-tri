
using System.Collections.Generic;
using UnityEngine;

public class Alias_Loader {
    public static string alias_folder { get; protected set; }
    public static string object_folder { get; protected set; }

    public static Alias_Loader main;

    Dictionary<string, Sprite> sprites;
    Dictionary<string, IsoObjectBody> prototypes;
    public SortedList<string, IsoObjectBody> objectsList { get; protected set; }

    public Alias_Loader()
    {
        main = this;
        alias_folder = Application.streamingAssetsPath + "/Export_Images";
        object_folder = Application.streamingAssetsPath + "/Export_Objects";

        objectsList = new SortedList<string, IsoObjectBody>();

        sprites = Alias_SpriteLoader.load(alias_folder);
        prototypes = Alias_ObjectLoader.load(object_folder, objectsList);

        Resources.UnloadUnusedAssets();
    }


    /// <summary>
    /// Trys to get Sprite or returns null
    /// </summary>
    /// <param name="name">name of target Sprite</param>
    /// <returns>the Sprite</returns>
    public Sprite getSprite(string name)
    {
        Sprite sp;
        if (sprites.TryGetValue(name, out sp))
        {
            return sp;
        }

        Debug.Log("Sprite: " + name + ", not found");
        return null;
    }

    /// <summary>
    /// Trys to get IsoObject or returns null
    /// </summary>
    public IsoObjectBody getObject(string name)
    {
        IsoObjectBody ob;
        if (prototypes.TryGetValue(name, out ob))
        {
            return ob;
        }

        Debug.Log("Object: " + name + ", not found");
        return null;
    }
}
