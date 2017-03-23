
using System.Collections.Generic;
using UnityEngine;

public class Atlas_Loader {
    public static string Atlas_folder { get; protected set; }
    public static string object_folder { get; protected set; }

    public static Atlas_Loader main;

    Dictionary<string, Sprite> sprites;
    Dictionary<string, IsoObjectBody> prototypes;
    public SortedList<string, IsoObjectBody> objectsList { get; protected set; }

    public Atlas_Loader(int resolution, int miplevels)
    {
        main = this;
        Atlas_folder = Application.streamingAssetsPath + "/Export_Images";
        object_folder = Application.streamingAssetsPath + "/Export_Objects";

        objectsList = new SortedList<string, IsoObjectBody>();

        sprites = Atlas_SpriteLoader.load(Atlas_folder);
        prototypes = Atlas_ObjectLoader.load(object_folder, objectsList);

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
