
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorSaveControl {
    List<IsoObject> objects = new List<IsoObject>();

    public EditorSaveControl()
    {
        IsoObject.registerOnCreate(onObjectCreate);
        IsoObject.registerOnDestroy(onObjectDestroy);
        HUI_EditorSaveCommand.registerSave(save);
    }

    void onObjectCreate(IsoObject o){objects.Add(o);}
    void onObjectDestroy(IsoObject o) { objects.Remove(o); }

    public void save(string filename)
    {
        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";

        xml += "<Level_XML width=\""+LogicControl.main.width+"\" height=\""+LogicControl.main.height+"\" depth=\""+LogicControl.main.depth+"\">\n";
        xml += "<objects>\n";

        foreach (IsoObject o in objects)
        {
            xml += "\t<IsoObject_XML name=\"" + o.name + "\" x=\""+o.origin.x+"\" y=\""+o.origin.y+"\" z=\""+o.origin.z+"\">\n";
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
