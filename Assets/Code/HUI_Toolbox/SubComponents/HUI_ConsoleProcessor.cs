
using System.Collections.Generic;

namespace H_UI
{
    /// <summary>
    /// Processes console commands
    /// </summary>
    public abstract class HUI_ConsoleProcessor
    {

        public List<HUI_ConsoleCommand> commands;

        /// <summary>
        /// Constructor, runs loadCommands.
        /// Adds help and clear
        /// </summary>
        public HUI_ConsoleProcessor()
        {
            commands = loadCommands();

            commands.Add(new HUI_Command_Clear());
            commands.Add(new HUI_Command_Help());
            commands.Add(new HUI_Command_FilePath());
        }

        /// <summary>
        /// Impelment the load commands to add new Console commands
        /// </summary>
        protected abstract List<HUI_ConsoleCommand> loadCommands();

        /// <summary>
        /// Processes the commands
        /// </summary>
        public void process()
        {
            string[] args = HUI_Console.main.content.Split(' ');

            if (args.Length == 0 || args[0] == "")
                return;

            foreach (HUI_ConsoleCommand c in commands)
            {
                if (args[0] == c.name)
                {
                    c.process(args);
                    return;
                }
            }

            HUI_Console.main.textBox.append("[" + args[0] + "], not fonud.");
        }

        /// <summary>
        /// Destroy this object, flushes all registeries
        /// </summary>
        public void destroy()
        {
            //Empty
        }
    }
}