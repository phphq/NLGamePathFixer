using System;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Collections.Generic;

using NLGamePathFixer.Classes;
using NLGamePathFixer.Properties;

namespace NLGamePathFixer.Forms
{
    public partial class Main : Form
    {
        private const string GameNotDetected = "Game not detected, please select the game folder.";
        private const string InvalidPath = "Invalid game folder detected, please select a new folder.";
        private const string RegAppPaths = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\"; //+Df.exe

        private readonly List<Game> GameList = new List<Game>();

        public Main()
        {
            InitializeComponent();

            GameList.Add(new Game
            {
                Name = "DF1",
                Exe = "df.exe",
                Button = btndf1,
                TextBox = tbdf1,
            });

            GameList.Add(new Game
            {
                Name = "DF2",
                Exe = "df2.exe",
                Button = btndf2,
                TextBox = tbdf2,
            });

            GameList.Add(new Game
            {
                Name = "DFLW",
                Exe = "dflw.exe",
                Button = btndf3,
                TextBox = tbdf3,
            });

            GameList.Add(new Game
            {
                Name = "DFTFD",
                Exe = "dftfd.exe",
                Button = btndf4,
                TextBox = tbdf4,
            });

            GameList.Add(new Game
            {
                Name = "DFBHD",
                Exe = "dfbhd.exe",
                Button = btndf5,
                TextBox = tbdf5,
            });

        }

        private void Main_Load(object sender, EventArgs e)
        {

            Text = Config.ProgramName + " v" + Config.ProgramVersion;

            pbNovaHQ.Image = Resources.NovaHQ;
            new ToolTip().SetToolTip(pbNovaHQ, "Visit NovaHQ!");

            btnSave.Enabled = false;

            foreach (var game in GameList)
            {

                using (var key = Registry.LocalMachine.OpenSubKey(RegAppPaths + game.Exe))
                {

                    if (key == null)
                    {
                        game.TextBox.Text = GameNotDetected;
                        continue;
                    }

                    var path = key.GetValue("Path");

                    if (path == null)
                    {
                        game.TextBox.Text = GameNotDetected;
                        continue;
                    }

                    game.TextBox.Text = File.Exists(Path.Combine(path.ToString(), game.Exe)) ? path + game.Exe : InvalidPath;

                }

            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            foreach (var game in GameList)
            {
                if (ofd.SafeFileName != null && !string.Equals(game.Exe, ofd.SafeFileName, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                game.TextBox.Text = ofd.FileName;

                btnSave.Enabled = true;

                return;
            }

            MessageBox.Show("File does not match / was not found.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            foreach (var game in GameList)
            {

                if (File.Exists(game.TextBox.Text))
                {
                    using (var key = Registry.LocalMachine.CreateSubKey(RegAppPaths + game.Exe))
                    {

                        if (key == null)
                            continue;

                        key.SetValue("Path", Path.GetDirectoryName(game.TextBox.Text) + "\\",
                            RegistryValueKind.String);
                        key.Close();
                    }

                }
                else if (game.TextBox.Text != InvalidPath && game.TextBox.Text != GameNotDetected && game.TextBox.Text != "")
                {
                    MessageBox.Show("Invalid " + game.Name + " Path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            btnSave.Enabled = false;

            MessageBox.Show("You should now be able to use programs that relied on these paths.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pbNoavHQ_Click(object sender, EventArgs e)
        {
            Common.GoToNovaHq();
        }

    }

}