using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SteamManifestGrabber
{
    public partial class SteamManifestGrabber : Form
    {
        public SteamManifestGrabber()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChooseFolder();
        }


        public void ChooseFolder()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChooseFolder2();
        }

        public void ChooseFolder2()
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Error. Both paths must be specified.", "Path not specified error");
                    return;
                }

                var sourcePath = textBox1.Text;
                var destinationPath = textBox2.Text;

                var gameFolder = Path.Combine(destinationPath, @"steamapps\common");
                var manifestFolder = Path.Combine(sourcePath, "steamapps");
                var destinationDirectories = Directory.GetDirectories(gameFolder);

                string[] myFiles = Directory.GetFiles(manifestFolder);

                foreach (var d in destinationDirectories)
                {
                    foreach (var f in myFiles)
                    {
                        if (File.ReadLines(f).Any(line => line.Contains("\"installdir\"		\"" + Path.GetFileName(d) + "\"")))
                        {
                            File.Copy(f, Path.Combine(destinationPath, Path.GetFileName(f)), true);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Exception: {exception}", "Exception encountered");
            }
        }
    }
}
