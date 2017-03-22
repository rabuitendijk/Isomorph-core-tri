using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Atlas_Builder  {
    static string xml_folder = Application.streamingAssetsPath+"/Source_XML";
    static string image_folder = Application.streamingAssetsPath + "/Source_Images";
    static string export_folder = Application.streamingAssetsPath + "/Export_Images";
    static string object_folder = Application.streamingAssetsPath + "/Export_Objects";

    public static void build()
    {
        List<SplicingObject_XML> objects = Atlas_Requests.getRequests(xml_folder);
        Dictionary<string, SplicingSource> dir = Atlas_SourceLoader.loadSource(image_folder, objects);
        Atlas_WriteAtlas.write(export_folder, Atlas_SourceLoader.entries);
        Atlas_WriteObjectXML.write(object_folder, objects, dir);
    }
}
