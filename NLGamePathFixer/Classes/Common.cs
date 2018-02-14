using System.Diagnostics;

namespace NLGamePathFixer.Classes
{
    public class Common
    {
        public static void GoToNovaHq()
        {
            Process.Start(Config.NoavHqUrlApp);
        }
    }

}