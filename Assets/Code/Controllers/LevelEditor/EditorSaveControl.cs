
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class EditorSaveControl {
    Dictionary<ulong, IsoObject> objects = new Dictionary<ulong, IsoObject>();

    public EditorSaveControl()
    {
        IsoObject.registerOnCreate(onObjectCreate);
        IsoObject.registerOnDestroy(onObjectDestroy);
        HUI_EditorSaveCommand.registerSave(save);
    }

    void onObjectCreate(IsoObject o){
        try {
            objects.Add(o.id, o);
        }catch(Exception e)
        {
            Debug.Log(e);
        }
    }
    void onObjectDestroy(IsoObject o) {
        try
        {
            objects.Remove(o.id);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void save(string filename)
    {
        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";

        xml += "<Level_XML width=\""+LogicControl.main.width+"\" height=\""+LogicControl.main.height+"\" length=\""+LogicControl.main.length+"\">\n";
        xml += "<objects>\n";

        foreach (KeyValuePair<ulong, IsoObject> entry in objects)
        {
            xml += "\t<IsoObject_XML direction=\"" + entry.Value.direction + "\" name=\"" + entry.Value.name + "\" x=\"" + entry.Value.origin.x + "\" y=\"" + entry.Value.origin.y + "\" z=\"" + entry.Value.origin.z + "\">\n";
            xml += "\t</IsoObject_XML>\n";
        }

        xml += "</objects>\n";
        xml += "</Level_XML>";
        File.WriteAllText(filename + ".xml", xml);
    }

    public void destroy()
    {
        IsoObject.removeOnCreate(onObjectCreate);
        IsoObject.removeOnDestroy(onObjectDestroy);
        HUI_EditorSaveCommand.removeSave(save);
    }
	
}
