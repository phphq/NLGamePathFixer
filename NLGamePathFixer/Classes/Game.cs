using System.Windows.Forms;

namespace NLGamePathFixer.Classes
{
    public class Game
    {
        public string Name;
        public string Exe;
        public TextBox TextBox;
        public Button Button;

        public Game()
        {
            Name = "";
            Exe = "";
            TextBox = null;
            Button = null;
        }

    }

}