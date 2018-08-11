namespace DLNAPlayer
{
    partial class CDDriveChooser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.driveComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // driveComboBox
            // 
            this.driveComboBox.FormattingEnabled = true;
            this.driveComboBox.Location = new System.Drawing.Point(12, 12);
            this.driveComboBox.Name = "driveComboBox";
            this.driveComboBox.Size = new System.Drawing.Size(121, 21);
            this.driveComboBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(139, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CDDriveChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 55);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.driveComboBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CDDriveChooser";
            this.Text = "Open which drive?";
            this.Load += new System.EventHandler(this.CDDriveChooser_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox driveComboBox;
        private System.Windows.Forms.Button button1;
    }
}