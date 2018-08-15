namespace DLNAPlayer
{
    partial class GDriveForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.GoBackButton = new System.Windows.Forms.Button();
            this.GoToRootButton = new System.Windows.Forms.Button();
            this.FoldersListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AddSelected = new System.Windows.Forms.Button();
            this.AddAll = new System.Windows.Forms.Button();
            this.FilesListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(807, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.GoBackButton);
            this.panel1.Controls.Add(this.GoToRootButton);
            this.panel1.Controls.Add(this.FoldersListBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(397, 444);
            this.panel1.TabIndex = 0;
            // 
            // GoBackButton
            // 
            this.GoBackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GoBackButton.Location = new System.Drawing.Point(209, 412);
            this.GoBackButton.Name = "GoBackButton";
            this.GoBackButton.Size = new System.Drawing.Size(182, 23);
            this.GoBackButton.TabIndex = 2;
            this.GoBackButton.Text = "Go Back";
            this.GoBackButton.UseVisualStyleBackColor = true;
            this.GoBackButton.Click += new System.EventHandler(this.GoBack_Click);
            // 
            // GoToRootButton
            // 
            this.GoToRootButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GoToRootButton.Location = new System.Drawing.Point(9, 412);
            this.GoToRootButton.Name = "GoToRootButton";
            this.GoToRootButton.Size = new System.Drawing.Size(194, 23);
            this.GoToRootButton.TabIndex = 3;
            this.GoToRootButton.Text = "Go to Root";
            this.GoToRootButton.UseVisualStyleBackColor = true;
            this.GoToRootButton.Click += new System.EventHandler(this.GoToRoot_Click);
            // 
            // FoldersListBox
            // 
            this.FoldersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FoldersListBox.FormattingEnabled = true;
            this.FoldersListBox.Location = new System.Drawing.Point(9, 22);
            this.FoldersListBox.Name = "FoldersListBox";
            this.FoldersListBox.Size = new System.Drawing.Size(382, 381);
            this.FoldersListBox.TabIndex = 1;
            this.FoldersListBox.DoubleClick += new System.EventHandler(this.FoldersListBox_DoubleClick);
            this.FoldersListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FoldersListBox_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Folders:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.AddSelected);
            this.panel2.Controls.Add(this.AddAll);
            this.panel2.Controls.Add(this.FilesListBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(406, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(398, 444);
            this.panel2.TabIndex = 1;
            // 
            // AddSelected
            // 
            this.AddSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddSelected.Enabled = false;
            this.AddSelected.Location = new System.Drawing.Point(6, 412);
            this.AddSelected.Name = "AddSelected";
            this.AddSelected.Size = new System.Drawing.Size(182, 23);
            this.AddSelected.TabIndex = 6;
            this.AddSelected.Text = "Add Selected Files";
            this.AddSelected.UseVisualStyleBackColor = true;
            this.AddSelected.Click += new System.EventHandler(this.AddSelected_Click);
            // 
            // AddAll
            // 
            this.AddAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddAll.Location = new System.Drawing.Point(206, 412);
            this.AddAll.Name = "AddAll";
            this.AddAll.Size = new System.Drawing.Size(182, 23);
            this.AddAll.TabIndex = 4;
            this.AddAll.Text = "Add All Files";
            this.AddAll.UseVisualStyleBackColor = true;
            this.AddAll.Click += new System.EventHandler(this.AddAll_Click);
            // 
            // FilesListBox
            // 
            this.FilesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesListBox.FormattingEnabled = true;
            this.FilesListBox.Location = new System.Drawing.Point(6, 22);
            this.FilesListBox.Name = "FilesListBox";
            this.FilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.FilesListBox.Size = new System.Drawing.Size(382, 381);
            this.FilesListBox.TabIndex = 5;
            this.FilesListBox.SelectedIndexChanged += new System.EventHandler(this.FilesListBox_SelectedIndexChanged);
            this.FilesListBox.DoubleClick += new System.EventHandler(this.FilesListBox_DoubleClick);
            this.FilesListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FilesListBox_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Files:";
            // 
            // GDriveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GDriveForm";
            this.Text = "Google Drive Browser";
            this.Load += new System.EventHandler(this.GDriveForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button GoToRootButton;
        private System.Windows.Forms.Button GoBackButton;
        private System.Windows.Forms.ListBox FoldersListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox FilesListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddSelected;
        private System.Windows.Forms.Button AddAll;
    }
}