namespace DLNAPlayer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.ScanRenderers = new System.Windows.Forms.Button();
            this.PlayBtn = new System.Windows.Forms.Button();
            this.Pause = new System.Windows.Forms.Button();
            this.MediaRenderers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MediaFiles = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ApplyServerIP = new System.Windows.Forms.Button();
            this.IPandPortTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TrackPositionLabel = new System.Windows.Forms.Label();
            this.trackProgress = new System.Windows.Forms.TrackBar();
            this.NextButton = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.TrackDurationLabel = new System.Windows.Forms.Label();
            this.ClearQueue = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAudioCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloudToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleDriveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readmeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tidalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackProgress)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScanRenderers
            // 
            this.ScanRenderers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanRenderers.Location = new System.Drawing.Point(3, 430);
            this.ScanRenderers.Name = "ScanRenderers";
            this.ScanRenderers.Size = new System.Drawing.Size(239, 23);
            this.ScanRenderers.TabIndex = 0;
            this.ScanRenderers.Text = "Scan Media Renderers";
            this.ScanRenderers.UseVisualStyleBackColor = true;
            this.ScanRenderers.Click += new System.EventHandler(this.CmdSSDP_Click);
            // 
            // PlayBtn
            // 
            this.PlayBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PlayBtn.Location = new System.Drawing.Point(138, 456);
            this.PlayBtn.Name = "PlayBtn";
            this.PlayBtn.Size = new System.Drawing.Size(75, 49);
            this.PlayBtn.TabIndex = 2;
            this.PlayBtn.Text = "Play";
            this.PlayBtn.UseVisualStyleBackColor = true;
            this.PlayBtn.Click += new System.EventHandler(this.CmdPlay_Click);
            // 
            // Pause
            // 
            this.Pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Pause.Location = new System.Drawing.Point(219, 456);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(75, 49);
            this.Pause.TabIndex = 3;
            this.Pause.Text = "Pause";
            this.Pause.UseVisualStyleBackColor = true;
            this.Pause.Click += new System.EventHandler(this.Pause_Click);
            // 
            // MediaRenderers
            // 
            this.MediaRenderers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MediaRenderers.FormattingEnabled = true;
            this.MediaRenderers.Location = new System.Drawing.Point(3, 28);
            this.MediaRenderers.Name = "MediaRenderers";
            this.MediaRenderers.Size = new System.Drawing.Size(239, 394);
            this.MediaRenderers.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Drag and Drop Media Files:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Select a renderer:";
            // 
            // MediaFiles
            // 
            this.MediaFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MediaFiles.FormattingEnabled = true;
            this.MediaFiles.Location = new System.Drawing.Point(6, 28);
            this.MediaFiles.Name = "MediaFiles";
            this.MediaFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.MediaFiles.Size = new System.Drawing.Size(999, 420);
            this.MediaFiles.TabIndex = 7;
            this.MediaFiles.DoubleClick += new System.EventHandler(this.MediaFiles_DoubleClick);
            this.MediaFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MediaFiles_KeyUp);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.82691F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.1731F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1271, 518);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ApplyServerIP);
            this.panel1.Controls.Add(this.IPandPortTxt);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.MediaRenderers);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ScanRenderers);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 512);
            this.panel1.TabIndex = 0;
            // 
            // ApplyServerIP
            // 
            this.ApplyServerIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyServerIP.Location = new System.Drawing.Point(167, 480);
            this.ApplyServerIP.Name = "ApplyServerIP";
            this.ApplyServerIP.Size = new System.Drawing.Size(75, 23);
            this.ApplyServerIP.TabIndex = 9;
            this.ApplyServerIP.Text = "Apply";
            this.ApplyServerIP.UseVisualStyleBackColor = true;
            this.ApplyServerIP.Click += new System.EventHandler(this.ApplyServerIP_Click);
            // 
            // IPandPortTxt
            // 
            this.IPandPortTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IPandPortTxt.Location = new System.Drawing.Point(3, 482);
            this.IPandPortTxt.Name = "IPandPortTxt";
            this.IPandPortTxt.Size = new System.Drawing.Size(158, 20);
            this.IPandPortTxt.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 466);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "IP and Port number for server:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.TrackPositionLabel);
            this.panel2.Controls.Add(this.trackProgress);
            this.panel2.Controls.Add(this.NextButton);
            this.panel2.Controls.Add(this.PreviousButton);
            this.panel2.Controls.Add(this.TrackDurationLabel);
            this.panel2.Controls.Add(this.ClearQueue);
            this.panel2.Controls.Add(this.Stop);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.MediaFiles);
            this.panel2.Controls.Add(this.PlayBtn);
            this.panel2.Controls.Add(this.Pause);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(254, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1014, 512);
            this.panel2.TabIndex = 1;
            // 
            // TrackPositionLabel
            // 
            this.TrackPositionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TrackPositionLabel.AutoSize = true;
            this.TrackPositionLabel.Location = new System.Drawing.Point(627, 466);
            this.TrackPositionLabel.Name = "TrackPositionLabel";
            this.TrackPositionLabel.Size = new System.Drawing.Size(49, 13);
            this.TrackPositionLabel.TabIndex = 15;
            this.TrackPositionLabel.Text = "00:00:00";
            // 
            // trackProgress
            // 
            this.trackProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackProgress.Location = new System.Drawing.Point(682, 460);
            this.trackProgress.Name = "trackProgress";
            this.trackProgress.Size = new System.Drawing.Size(220, 45);
            this.trackProgress.TabIndex = 14;
            this.trackProgress.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackProgress_MouseUp);
            // 
            // NextButton
            // 
            this.NextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NextButton.Location = new System.Drawing.Point(462, 456);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 49);
            this.NextButton.TabIndex = 12;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PreviousButton.Location = new System.Drawing.Point(381, 456);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(75, 49);
            this.PreviousButton.TabIndex = 11;
            this.PreviousButton.Text = "Previous";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // TrackDurationLabel
            // 
            this.TrackDurationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackDurationLabel.AutoSize = true;
            this.TrackDurationLabel.Location = new System.Drawing.Point(908, 466);
            this.TrackDurationLabel.Name = "TrackDurationLabel";
            this.TrackDurationLabel.Size = new System.Drawing.Size(49, 13);
            this.TrackDurationLabel.TabIndex = 10;
            this.TrackDurationLabel.Text = "00:00:00";
            // 
            // ClearQueue
            // 
            this.ClearQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ClearQueue.Location = new System.Drawing.Point(7, 456);
            this.ClearQueue.Name = "ClearQueue";
            this.ClearQueue.Size = new System.Drawing.Size(88, 49);
            this.ClearQueue.TabIndex = 9;
            this.ClearQueue.Text = "Clear Queue";
            this.ClearQueue.UseVisualStyleBackColor = true;
            this.ClearQueue.Click += new System.EventHandler(this.ClearQueue_Click);
            // 
            // Stop
            // 
            this.Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Stop.Location = new System.Drawing.Point(300, 456);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 49);
            this.Stop.TabIndex = 8;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.cloudToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1271, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFilesToolStripMenuItem,
            this.openAudioCDToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFilesToolStripMenuItem
            // 
            this.openFilesToolStripMenuItem.Name = "openFilesToolStripMenuItem";
            this.openFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
            this.openFilesToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.openFilesToolStripMenuItem.Text = "Open Files";
            this.openFilesToolStripMenuItem.Click += new System.EventHandler(this.openFilesToolStripMenuItem_Click);
            // 
            // openAudioCDToolStripMenuItem
            // 
            this.openAudioCDToolStripMenuItem.Name = "openAudioCDToolStripMenuItem";
            this.openAudioCDToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.openAudioCDToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.openAudioCDToolStripMenuItem.Text = "Open Audio CD";
            this.openAudioCDToolStripMenuItem.Click += new System.EventHandler(this.openAudioCDToolStripMenuItem_Click);
            // 
            // cloudToolStripMenuItem
            // 
            this.cloudToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.googleDriveToolStripMenuItem,
            this.tidalToolStripMenuItem});
            this.cloudToolStripMenuItem.Name = "cloudToolStripMenuItem";
            this.cloudToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.cloudToolStripMenuItem.Text = "Cloud";
            // 
            // googleDriveToolStripMenuItem
            // 
            this.googleDriveToolStripMenuItem.Name = "googleDriveToolStripMenuItem";
            this.googleDriveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.G)));
            this.googleDriveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.googleDriveToolStripMenuItem.Text = "Google Drive";
            this.googleDriveToolStripMenuItem.Click += new System.EventHandler(this.googleDriveToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readmeToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // readmeToolStripMenuItem
            // 
            this.readmeToolStripMenuItem.Name = "readmeToolStripMenuItem";
            this.readmeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H)));
            this.readmeToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.readmeToolStripMenuItem.Text = "Readme";
            this.readmeToolStripMenuItem.Click += new System.EventHandler(this.readmeToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tidalToolStripMenuItem
            // 
            this.tidalToolStripMenuItem.Name = "tidalToolStripMenuItem";
            this.tidalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.tidalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.tidalToolStripMenuItem.Text = "Tidal";
            this.tidalToolStripMenuItem.Click += new System.EventHandler(this.tidalToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 542);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "DLNA Player - v0.5";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackProgress)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ScanRenderers;
        private System.Windows.Forms.Button PlayBtn;
        private System.Windows.Forms.Button Pause;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button ClearQueue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ApplyServerIP;
        private System.Windows.Forms.TextBox IPandPortTxt;
        private System.Windows.Forms.Label TrackDurationLabel;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.TrackBar trackProgress;
        private System.Windows.Forms.Label TrackPositionLabel;
        public System.Windows.Forms.ListBox MediaRenderers;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.ListBox MediaFiles;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cloudToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem googleDriveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readmeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openAudioCDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tidalToolStripMenuItem;
    }
}

