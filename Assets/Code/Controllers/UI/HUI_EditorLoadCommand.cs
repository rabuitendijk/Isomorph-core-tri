

using System.IO;
using H_UI;

namespace UI_C
{
    /// <summary>
    /// Load command for console in level editor
    /// </summary>
    public class HUI_EditorLoadCommand : HUI_ConsoleCommand
    {
        /// <summary>
        /// Constructor does nothing
        /// </summary>
        public HUI_EditorLoadCommand() : base("load")
        {
            //Empty
        }

        /// <summary>
        /// Prints help
        /// </summary>
        public override string help()
        {
            return "<color=grey>filename</color>, loads level from filename.";
        }

        /// <summary>
        /// Pushes next swapstate to controller manager
        /// </summary>
        public override void process(string[] args)
        {
            /* if (load == null)
             {
                 HUI_Console.main.textBox.append("<color=red><ERROR> no callback to load function.</color>\n");
                 return;
             }*/

            if (args.Length != 2 || args[1] == "")
            {
                HUI_Console.main.textBox.append("<color=red><ERROR> incorect number of arguments, expects: filename.</color>\n");
                return;
            }

            if (!File.Exists(HUI_Console.main.filepath + "/Levels/" + args[1] + ".xml"))
            {
                HUI_Console.main.textBox.append("<color=red><ERROR> file does not exist: " + HUI_Console.main.filepath + "/Levels/" + args[1] + ".xml" + ".</color>\n");
                return;
            }

            ControllerManager.main.setNextSwap(ControllerManager.Mode.editor, args[1]);
        }
    }
}