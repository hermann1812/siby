namespace siby
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_sorted = new System.Windows.Forms.Label();
            this.button_sorted = new System.Windows.Forms.Button();
            this.label_unsorted = new System.Windows.Forms.Label();
            this.button_unsorted = new System.Windows.Forms.Button();
            this.checkBox_copy = new System.Windows.Forms.CheckBox();
            this.checkBox_move = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_sorted
            // 
            this.label_sorted.AutoSize = true;
            this.label_sorted.Location = new System.Drawing.Point(133, 108);
            this.label_sorted.Name = "label_sorted";
            this.label_sorted.Size = new System.Drawing.Size(16, 13);
            this.label_sorted.TabIndex = 7;
            this.label_sorted.Text = "-1";
            // 
            // button_sorted
            // 
            this.button_sorted.Location = new System.Drawing.Point(15, 94);
            this.button_sorted.Name = "button_sorted";
            this.button_sorted.Size = new System.Drawing.Size(112, 41);
            this.button_sorted.TabIndex = 6;
            this.button_sorted.Text = "Folder with\r\nsorted JPG files";
            this.button_sorted.UseVisualStyleBackColor = true;
            this.button_sorted.Click += new System.EventHandler(this.button_sorted_Click);
            // 
            // label_unsorted
            // 
            this.label_unsorted.AutoSize = true;
            this.label_unsorted.Location = new System.Drawing.Point(133, 52);
            this.label_unsorted.Name = "label_unsorted";
            this.label_unsorted.Size = new System.Drawing.Size(16, 13);
            this.label_unsorted.TabIndex = 5;
            this.label_unsorted.Text = "-1";
            // 
            // button_unsorted
            // 
            this.button_unsorted.Location = new System.Drawing.Point(15, 38);
            this.button_unsorted.Name = "button_unsorted";
            this.button_unsorted.Size = new System.Drawing.Size(112, 41);
            this.button_unsorted.TabIndex = 4;
            this.button_unsorted.Text = "Folder with\r\nunsorted JPG files";
            this.button_unsorted.UseVisualStyleBackColor = true;
            this.button_unsorted.Click += new System.EventHandler(this.button_unsorted_Click);
            // 
            // checkBox_copy
            // 
            this.checkBox_copy.AutoSize = true;
            this.checkBox_copy.Location = new System.Drawing.Point(15, 152);
            this.checkBox_copy.Name = "checkBox_copy";
            this.checkBox_copy.Size = new System.Drawing.Size(50, 17);
            this.checkBox_copy.TabIndex = 8;
            this.checkBox_copy.Text = "Copy";
            this.checkBox_copy.UseVisualStyleBackColor = true;
            this.checkBox_copy.CheckedChanged += new System.EventHandler(this.checkBox_copy_CheckedChanged);
            // 
            // checkBox_move
            // 
            this.checkBox_move.AutoSize = true;
            this.checkBox_move.Location = new System.Drawing.Point(15, 176);
            this.checkBox_move.Name = "checkBox_move";
            this.checkBox_move.Size = new System.Drawing.Size(53, 17);
            this.checkBox_move.TabIndex = 9;
            this.checkBox_move.Text = "Move";
            this.checkBox_move.UseVisualStyleBackColor = true;
            this.checkBox_move.CheckedChanged += new System.EventHandler(this.checkBox_move_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Sort Images By Year";
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(15, 199);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(112, 41);
            this.button_start.TabIndex = 11;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 270);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_move);
            this.Controls.Add(this.checkBox_copy);
            this.Controls.Add(this.label_sorted);
            this.Controls.Add(this.button_sorted);
            this.Controls.Add(this.label_unsorted);
            this.Controls.Add(this.button_unsorted);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_sorted;
        private System.Windows.Forms.Button button_sorted;
        private System.Windows.Forms.Label label_unsorted;
        private System.Windows.Forms.Button button_unsorted;
        private System.Windows.Forms.CheckBox checkBox_copy;
        private System.Windows.Forms.CheckBox checkBox_move;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_start;
    }
}

