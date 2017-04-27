

namespace H_UI
{
    /// <summary>
    /// Processes a command with args
    /// </summary>
    public abstract class HUI_ConsoleCommand
    {
        public string name { get; protected set; }

        /// <summary>
        /// This constructor must be ran with the name of the inhereting command
        /// </summary>
        public HUI_ConsoleCommand(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Execute argument
        /// </summary>
        public abstract void process(string[] args);

        /// <summary>
        /// Get help
        /// </summary>
        public abstract string help();
    }
}
