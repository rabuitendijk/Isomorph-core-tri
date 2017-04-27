
using System;

using GraphicsControl = Graphics_C.GraphicsControl;
using LogicControl = Logic_C.LogicControl;

using Debug = UnityEngine.Debug;

namespace Input_C
{
    /// <summary>
    /// Layer mode, collides mouse with current height layer only
    /// </summary>
    public class EditorComponentMouseLayer : ComponentMouse
    {
        public int layer;

        public static Action<bool> moveLayer;
        public static Action<bool, int> enableLayer;
        public static void registerMoveLayer(Action<bool> funct) { moveLayer += funct; }
        public static void registerEnableLayer(Action<bool, int> funct) { enableLayer += funct; }
        public static void removeMoveLayer(Action<bool> funct) { moveLayer -= funct; }
        public static void removeEnableLayer(Action<bool, int> funct) { enableLayer -= funct; }

        Iso selected;
        EditorInputControl input;

        /// <summary>
        /// Common constructor
        /// </summary>
        public EditorComponentMouseLayer(EditorInputControl input, int layer = 0)
        {
            this.layer = layer;
            this.input = input;

            if (enableLayer != null)
                enableLayer(true, layer);
        }

        /// <summary>
        /// Run once per frame
        /// </summary>
        public override void update()
        {


            if (catchHitFloor(out selected))
            {   // Valid coordinate
                GraphicsControl.main.hover.unhide();
                GraphicsControl.main.hover.translate(selected);
            }
            else
                GraphicsControl.main.hover.hide();

        }


        /// <summary>
        /// Racasthit plus hitting empty floor functionallity
        /// </summary>
        bool catchHitFloor(out Iso i)
        {
            i = Directions.mouseToFloor(layer);

            if (i == null || !LogicControl.main.inGrid(i))
                return false;
            return true;
        }

        /// <summary>
        /// Handles on click event
        /// </summary>
        public override void onClick(string mode)
        {
            //Remove at righth mouse click
            if (mode == "right")
            {

                if (selected != null && LogicControl.main.exists(selected))
                    LogicControl.main.get(selected).destroy();
                else
                    Debug.Log("No tile selected for removal.");

                return;
            }

            string name = input.selected;
            if (name == "VOID")
                return;

            //Add at left mouse click
            if (mode == "left")
            {
                if (selected != null)
                {
                    new IsoObject(name, selected, GraphicsControl.main.hover.getDirection());
                }
                else
                    Debug.Log("No tile selected.");
                return;
            }

            Debug.Log("You clicked:" + mode);
        }
    }
}