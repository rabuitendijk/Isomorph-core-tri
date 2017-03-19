using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Alias_Builder  {
    static string xml_folder = Application.streamingAssetsPath+"/Source_XML";
    static string image_folder = Application.streamingAssetsPath + "/Source_Images";
    static string export_folder = Application.streamingAssetsPath + "/Export_Images";
    static string object_folder = Application.streamingAssetsPath + "/Export_Objects";

    public static void build()
    {
        List<SplicingObject_XML> objects = Alias_Requests.getRequests(xml_folder);
        Dictionary<string, SplicingSource> dir = Alias_SourceLoader.loadSource(image_folder, objects);
        Alias_WriteAlias.write(export_folder, Alias_SourceLoader.entries);
        Alias_WriteObjectXML.write(object_folder, objects, dir);
    }
}
