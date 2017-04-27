using LogicControl = Logic_C.LogicControl;

using System.Collections.Generic;
using System.IO;
using System;
using Debug = UnityEngine.Debug;

namespace Save_C
{

    /// <summary>
    /// Save controller implimentation for the level editor
    /// </summary>
    public class EditorSaveControl : SaveControl
    {
        Dictionary<ulong, IsoObject> objects = new Dictionary<ulong, IsoObject>();

        /// <summary>
        /// Common constructor
        /// </summary>
        public EditorSaveControl() : base()
        {
            IsoObject.registerOnCreate(onObjectCreate);
            IsoObject.registerOnDestroy(onObjectDestroy);
            UI_C.HUI_EditorSaveCommand.registerSave(save);
        }

        /// <summary>
        /// Delayed constructor
        /// </summary>
        public override void delayedConstruction()
        {
            //Empty
        }

        /// <summary>
        /// Respond to object creation
        /// </summary>
        void onObjectCreate(IsoObject o)
        {
            try
            {
                objects.Add(o.id, o);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        /// <summary>
        /// Respond to object destruction
        /// </summary>
        void onObjectDestroy(IsoObject o)
        {
            try
            {
                objects.Remove(o.id);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        /// <summary>
        /// Save current level
        /// </summary>
        protected override void save(string filename)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";

            xml += "<XMLO_LL_Level width=\"" + LogicControl.main.width + "\" height=\"" + LogicControl.main.height + "\" length=\"" + LogicControl.main.length + "\">\n";
            xml += "<objects>\n";

            foreach (KeyValuePair<ulong, IsoObject> entry in objects)
            {
                xml += "\t<XMLO_LL_IsoObject direction=\"" + entry.Value.direction + "\" name=\"" + entry.Value.name + "\" x=\"" + entry.Value.origin.x + "\" y=\"" + entry.Value.origin.y + "\" z=\"" + entry.Value.origin.z + "\">\n";
                xml += "\t</XMLO_LL_IsoObject>\n";
            }

            xml += "</objects>\n";
            xml += "</XMLO_LL_Level>";
            File.WriteAllText(filename + ".xml", xml);
        }

        /// <summary>
        /// Destroy this obejct
        /// </summary>
        protected override void destructor()
        {
            IsoObject.removeOnCreate(onObjectCreate);
            IsoObject.removeOnDestroy(onObjectDestroy);
            UI_C.HUI_EditorSaveCommand.removeSave(save);
        }

    }
}