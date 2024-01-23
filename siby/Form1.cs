using ExifLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace siby
{
    public partial class Form1 : Form
    {
        // Caption for MessageBox
        public static readonly string caption = "Siby - " + Assembly.GetEntryAssembly().GetName().Version;

        string sourceFolder = "D:\\Temp\\unsorted";
        string destRootFolder = "D:\\Temp\\sorted";

        bool move = false;

        public Form1()
        {
            InitializeComponent();

            // Caption for form
            Text = caption;
        }

        private void button_unsorted_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    sourceFolder = fbd.SelectedPath;
                }
            }

            Form1_Load(null, null);
        }

        private void button_sorted_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    destRootFolder = fbd.SelectedPath;
                }
            }

            Form1_Load(null, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label_unsorted.Text = sourceFolder;
            label_sorted.Text = destRootFolder;
        }

        private void checkBox_copy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_copy.Checked)
            {
                checkBox_move.Checked = false;
                move = false;
            }
        }

        private void checkBox_move_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_move.Checked)
            {
                checkBox_copy.Checked = false;
                move = true;
            }
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            string logFile = Path.Combine(destRootFolder, "siby.log");

            if (File.Exists(logFile)) { File.Delete(logFile); }

            string fileName = string.Empty;

            int z = 0;

            List<string> paths = new List<string>();

            paths = Directory.GetFiles(sourceFolder, "*.jpg", SearchOption.AllDirectories).ToList();

            int number = paths.Count;

            foreach (string path in paths)
            {
                z = z + 1;

                //string make, model;
                DateTime dateTimeOriginal;

                try
                {
                    ExifReader reader = new ExifReader(path);

                    //reader.GetTagValue<string>(ExifTags.Make, out make);
                    //reader.GetTagValue<string>(ExifTags.Model, out model);
                    reader.GetTagValue<DateTime>(ExifTags.DateTimeOriginal, out dateTimeOriginal);
                    reader.Dispose();

                    string destFolder = Path.Combine(destRootFolder, dateTimeOriginal.Year.ToString());
                    string destPath = Path.Combine(destFolder, Path.GetFileName(path));

                    if (!Directory.Exists(destFolder)) { Directory.CreateDirectory(destFolder); }

                    //File.Move(path, destPath);
                    File.Copy(path, destPath);
                }
                catch (Exception ex)
                {
                    File.AppendAllText(logFile, ex.Message + " -> " + path + "\r\n");
                }
            }
            MessageBox.Show("Done!");

            Close();
        }
    }
}
