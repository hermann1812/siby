using ExifLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace siby
{
    public partial class Form1 : Form
    {
        // Caption for MessageBox
        public static readonly string caption = "Siby - " + Assembly.GetEntryAssembly().GetName().Version;

        // Source and a destination folder
        string sourceFolder = string.Empty;
        string destRootFolder = string.Empty;

        // The last used directories will be saved in this text file
        string directories = Path.Combine(Path.GetTempPath(), "siby.txt");

        // Log File
        string logFile = string.Empty;
        string logText = string.Empty;

        // To decide whether the files should be copied or moved
        bool move = false;

        // List of JPG files in the source directory
        List<string> paths;

        public Form1()
        {
            InitializeComponent();

            // Caption for form
            Text = caption;

            // If available read the paths of last used directories
            if (File.Exists(directories))
            {
                string[] lines = File.ReadAllLines(directories);

                sourceFolder = lines[0];
                destRootFolder = lines[1];
            }
        }
        /// <summary>
        /// Method to select the source directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Method to select the target directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Actions that are carried out when the form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(sourceFolder))
            {
                label_counter.Text = Directory.GetFiles(sourceFolder, "*.jpg", SearchOption.AllDirectories).Count().ToString() + " JPG-files have been found"; 
                label_unsorted.Text = sourceFolder;
            }
            else
            {
                label_unsorted.Text = "not yet defined";
            }

            if (Directory.Exists(destRootFolder))
            {
                label_existing.Text = Directory.GetFiles(destRootFolder, "*.*", SearchOption.AllDirectories).Count().ToString() + " files have been found"; 
                label_sorted.Text = destRootFolder;
            }
            else
            {
                label_sorted.Text = "not yet defined";
            }

            if (Directory.Exists(sourceFolder) & Directory.Exists(destRootFolder))
            {
                if (!checkBox_copy.Checked & !checkBox_move.Checked)
                {
                    button_start.Enabled = false;
                }
                else
                {
                    button_start.Enabled = true;
                } 
            }
            else
            {
                button_start.Enabled = false;
            }
        }

        private void checkBox_copy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_copy.Checked)
            {
                checkBox_move.Checked = false;
                move = false;
            }
            Form1_Load(null, null);
        }

        private void checkBox_move_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_move.Checked)
            {
                checkBox_copy.Checked = false;
                move = true;
            }
            Form1_Load(null, null);
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            // Save the last used folder paths
            if (File.Exists(directories)) { File.Delete(directories); }
            File.WriteAllText(directories, sourceFolder + "\r\n" + destRootFolder);

            // Create new log file
            if (File.Exists(logFile)) { File.Delete(logFile); }
            logFile = Path.Combine(destRootFolder, "siby.log");
            logText = caption + " - Log File created " + DateTime.Now + Environment.NewLine;
            File.WriteAllText(logFile, logText);

            // List of JPG files in the source directory
            paths = new List<string>();
            paths = Directory.GetFiles(sourceFolder, "*.jpg", SearchOption.AllDirectories).ToList();

            // Start of file operations
            new Thread(StartCopyOrMove).Start();
        }

        private void StartCopyOrMove()
        {
            // Start ProgressBar
            Invoke((MethodInvoker)delegate () { progressBar1.Style = ProgressBarStyle.Marquee; });
            Invoke((MethodInvoker)delegate () { progressBar1.MarqueeAnimationSpeed = 30; });

            int z = 0;
            int end = paths.Count;

            foreach (string path in paths)
            {
                z = z + 1;
                
                Invoke((MethodInvoker)delegate () { label_progress.Text = z + "//" + end; });

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

                    if (move)
                    {
                        File.Move(path, destPath);
                    }
                    else
                    {
                        File.Copy(path, destPath);
                    }
                }
                catch (Exception ex)
                {
                    FileInfo info = new FileInfo(logFile);

                    while (IsFileLocked(info))
                    {
                        Thread.Sleep(1000);
                    }
                    
                    File.AppendAllText(logFile, ex.Message + " -> " + path + "\r\n");
                }
            }
            // Stop Progressbar
            Invoke((MethodInvoker)delegate () { progressBar1.Style = ProgressBarStyle.Blocks; });
            Invoke((MethodInvoker)delegate () { progressBar1.Value = 0; });

            logText = "Complete! " + paths.Count + " files were processed.";

            File.AppendAllText(logFile, logText);
            MessageBox.Show(logText + "\r\nPlease check log file " + logFile, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);

            //Invoke((MethodInvoker)delegate () { Close(); });
            Invoke((MethodInvoker)delegate () { Form1_Load(null, null); });
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
