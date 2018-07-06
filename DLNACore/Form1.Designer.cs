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
            this.label4 = new System.Windows.Forms.Label();
            this.ClearQueue = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScanRenderers
            // 
            this.ScanRenderers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanRenderers.Location = new System.Drawing.Point(3, 454);
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
            this.PlayBtn.Location = new System.Drawing.Point(138, 504);
            this.PlayBtn.Name = "PlayBtn";
            this.PlayBtn.Size = new System.Drawing.Size(75, 23);
            this.PlayBtn.TabIndex = 2;
            this.PlayBtn.Text = "Play";
            this.PlayBtn.UseVisualStyleBackColor = true;
            this.PlayBtn.Click += new System.EventHandler(this.CmdPlay_Click);
            // 
            // Pause
            // 
            this.Pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Pause.Location = new System.Drawing.Point(219, 504);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(75, 23);
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
            this.MediaRenderers.Size = new System.Drawing.Size(239, 420);
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
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Select  a renderer:";
            // 
            // MediaFiles
            // 
            this.MediaFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MediaFiles.FormattingEnabled = true;
            this.MediaFiles.Location = new System.Drawing.Point(6, 28);
            this.MediaFiles.Name = "MediaFiles";
            this.MediaFiles.Size = new System.Drawing.Size(999, 472);
            this.MediaFiles.TabIndex = 7;
            this.MediaFiles.DoubleClick += new System.EventHandler(this.MediaFiles_DoubleClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.82691F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.1731F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1271, 542);
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
            this.panel1.Size = new System.Drawing.Size(245, 536);
            this.panel1.TabIndex = 0;
            // 
            // ApplyServerIP
            // 
            this.ApplyServerIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyServerIP.Location = new System.Drawing.Point(160, 504);
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
            this.IPandPortTxt.Location = new System.Drawing.Point(3, 506);
            this.IPandPortTxt.Name = "IPandPortTxt";
            this.IPandPortTxt.Size = new System.Drawing.Size(151, 20);
            this.IPandPortTxt.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 490);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "IP and Port number for server:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.NextButton);
            this.panel2.Controls.Add(this.PreviousButton);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.ClearQueue);
            this.panel2.Controls.Add(this.Stop);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.MediaFiles);
            this.panel2.Controls.Add(this.PlayBtn);
            this.panel2.Controls.Add(this.Pause);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(254, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1014, 536);
            this.panel2.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(955, 513);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "label4";
            // 
            // ClearQueue
            // 
            this.ClearQueue.Location = new System.Drawing.Point(7, 504);
            this.ClearQueue.Name = "ClearQueue";
            this.ClearQueue.Size = new System.Drawing.Size(75, 23);
            this.ClearQueue.TabIndex = 9;
            this.ClearQueue.Text = "Clear Queue";
            this.ClearQueue.UseVisualStyleBackColor = true;
            this.ClearQueue.Click += new System.EventHandler(this.ClearQueue_Click);
            // 
            // Stop
            // 
            this.Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Stop.Location = new System.Drawing.Point(300, 504);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 23);
            this.Stop.TabIndex = 8;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PreviousButton.Location = new System.Drawing.Point(381, 504);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(75, 23);
            this.PreviousButton.TabIndex = 11;
            this.PreviousButton.Text = "Previous";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NextButton.Location = new System.Drawing.Point(462, 504);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 12;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 542);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "DLNA Player";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ScanRenderers;
        private System.Windows.Forms.Button PlayBtn;
        private System.Windows.Forms.Button Pause;
        private System.Windows.Forms.ListBox MediaRenderers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox MediaFiles;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button ClearQueue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ApplyServerIP;
        private System.Windows.Forms.TextBox IPandPortTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button PreviousButton;
    }
}

