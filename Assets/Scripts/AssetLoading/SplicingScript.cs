using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// aplha-1
/// 
/// Splices cource files into unit blocks using data from an xml file.
/// Needs a reference unit cell sprite.
/// 
/// 
/// Robin Apollo Butiendijk
/// Late February 2017
/// </summary>
public class SplicingScript{

    public static SplicingScript main;
    int count = 0;

    public SplicingScript()
    {
        main = this;
        Splice();
    }

    void Splice()
    {
        string targetDir = System.IO.Path.Combine(Application.streamingAssetsPath, "Images\\Spliced");
        string sourceDir = System.IO.Path.Combine(Application.streamingAssetsPath, "UnsplicedSource");

        if (!Directory.Exists(targetDir))
        {
            try {
                Directory.CreateDirectory(targetDir);
            }catch(Exception e)
            {
                Debug.Log(e);
            }
        }


        RecersiveDirectoryCrawler(sourceDir);

        Debug.Log("Splicing sprites: " + count + ", sprites spliced");
    }

    /// <summary>
    /// Recersive first load later
    /// </summary>
    void RecersiveDirectoryCrawler(string path)
    {

        Debug.Log("RecersiveDirectoryCrawler: "+path);

        //Recieve
        string[] subDirs = Directory.GetDirectories(path);

        foreach (string s in subDirs)
        {
            RecersiveDirectoryCrawler(s);
        }

        //Load
        string[] subFiles = Directory.GetFiles(path);

        foreach (string f in subFiles)
        {
            if (f.ToLower().EndsWith(".png"))
            {
                //LoadSprite(f.Substring(f.LastIndexOf('\\') + 1, f.Length - f.LastIndexOf('\\') - 5), f); 
                Debug.Log("RecersiveDirectoryCrawler: " + f);
            }

            }
    }

}
