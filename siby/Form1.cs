﻿using ExifLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Windows.Forms;

namespace siby
{
    public partial class Form1 : Form
    {
        // Caption for MessageBox
        public static readonly string caption = "Siby - " + Assembly.GetEntryAssembly().GetName().Version;

        string sourceFolder = "D:\\Temp\\unsorted";
        static string destRootFolder = "D:\\Temp\\sorted";

        string logFile = Path.Combine(destRootFolder, "siby.log");

        string logText = string.Empty;

        bool move = false;

        List<string> paths;

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

            label_counter.Text = Directory.GetFiles(sourceFolder, "*.jpg", SearchOption.AllDirectories).Count().ToString() + " JPG-files have been found";
            label_existing.Text = Directory.GetFiles(destRootFolder, "*.*", SearchOption.AllDirectories).Count().ToString() + " files have been found";
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
            if (File.Exists(logFile)) { File.Delete(logFile); }

            logText = caption + " - Log File created " + DateTime.Now + Environment.NewLine;
            File.WriteAllText(logFile, logText);

            paths = new List<string>();

            paths = Directory.GetFiles(sourceFolder, "*.jpg", SearchOption.AllDirectories).ToList();

            new Thread(StartCopyOrMove).Start();


       
        }

        private void StartCopyOrMove()
        {
            // Start ProgressBar
            Invoke((MethodInvoker)delegate () { progressBar1.Style = ProgressBarStyle.Marquee; });
            Invoke((MethodInvoker)delegate () { progressBar1.MarqueeAnimationSpeed = 30; });



            foreach (string path in paths)
            {
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
            // Stop Progressbar
            Invoke((MethodInvoker)delegate () { progressBar1.Style = ProgressBarStyle.Blocks; });
            Invoke((MethodInvoker)delegate () { progressBar1.Value = 0; });

            logText = "Complete! " + paths.Count + " files have been copied.";

            File.AppendAllText(logFile, logText);
            MessageBox.Show(logText + "\r\nPlease check log file " + logFile, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);

            Invoke((MethodInvoker)delegate () { Close(); });
        }
    }
}
