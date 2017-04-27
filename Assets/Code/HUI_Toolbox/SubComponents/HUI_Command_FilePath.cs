

namespace H_UI
{
    /// <summary>
    /// console command that shows currect file path
    /// </summary>
    public class HUI_Command_FilePath : HUI_ConsoleCommand
    {

        public HUI_Command_FilePath() : base("filepath")
        {
            //Empty
        }

        public override string help()
        {
            return HUI_Console.main.filepath;
        }

        public override void process(string[] args)
        {
            HUI_Console.main.textBox.append(HUI_Console.main.filepath + "\n");
        }
    }

}