using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLNAPlayer
{
    public partial class GDriveForm : Form
    {
        private string formName = "Google Drive Browser";
        public static GDrive drive;
        public GDriveForm()
        {
            InitializeComponent();
        }

        private void GDriveForm_Load(object sender, EventArgs e)
        {
            drive = new GDrive();
            if (drive.connected)
                PopulateListBoxes(drive);
            else
            {
                MessageBox.Show("client_secret.json file not found. Please follow Step 1 in this guide: https://developers.google.com/drive/v3/web/quickstart/dotnet" + Environment.NewLine + Environment.NewLine + "This file should be located in the folder where this software is located.");
                Process.Start("https://developers.google.com/drive/v3/web/quickstart/dotnet");
                Close();
            }
        }
        private void PopulateListBoxes(GDrive drive, string location = "root", bool refreshing = false)
        {
            if (location == "back")
                drive.GoBack();
            else
            {
                if (!refreshing)
                    drive.GetData(location);
                else
                    drive.GetData(location, false, true);
            }
            FoldersListBox.Items.Clear();
            FilesListBox.Items.Clear();
            try
            {
                if (drive.FolderList.Count > 0)
                {
                    foreach (string item in drive.FolderList)
                    {
                        FoldersListBox.Items.Add(item);
                    }
                }
                if (drive.FileList.Count > 0)
                {
                    foreach (string item in drive.FileList)
                    {
                        FilesListBox.Items.Add(item);
                    }
                    AddAll.Enabled = true;
                }
                else
                    AddAll.Enabled = false;
                if (drive.previousFolder.Count < 1)
                    GoBackButton.Enabled = false;
                else
                    GoBackButton.Enabled = true;
                this.Text = formName + " - " + drive.currentFolderName;
            }
            catch
            {
                MessageBox.Show("Error loading Google Drive contents");
            }
        }

        private void GoToRoot_Click(object sender, EventArgs e)
        {
            PopulateListBoxes(drive);
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            PopulateListBoxes(drive, "back");
        }

        private void FoldersListBox_DoubleClick(object sender, EventArgs e)
        {
            if (FoldersListBox.SelectedIndex > -1)
                PopulateListBoxes(drive, drive.FolderListID[FoldersListBox.SelectedIndex]);
        }

        private void FilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilesListBox.SelectedIndex > -1)
                AddSelected.Enabled = true;
            else
                AddSelected.Enabled = false;
        }

        private void AddAll_Click(object sender, EventArgs e)
        {
            if (FilesListBox.Items.Count > 0)
                foreach (string item in FilesListBox.Items)
                {
                    Form1 MainForm = (Form1)this.Owner;
                    MainForm.addToList(item, drive.FileListID[FilesListBox.Items.IndexOf(item)], 2);
                }
        }

        private void AddSelected_Click(object sender, EventArgs e)
        {
            Form1 MainForm = (Form1)this.Owner;
            foreach (string item in FilesListBox.SelectedItems)
                MainForm.addToList(item, drive.FileListID[FilesListBox.Items.IndexOf(item)], 2);
        }

        private void FilesListBox_DoubleClick(object sender, EventArgs e)
        {
            if (FilesListBox.SelectedIndex > -1)
            {
                Form1 MainForm = (Form1)this.Owner;
                MainForm.addToList(FilesListBox.SelectedItem.ToString(), drive.FileListID[FilesListBox.SelectedIndex], 2);
            }
        }

        private void FoldersListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (FoldersListBox.SelectedIndex > -1)
                {
                    PopulateListBoxes(drive, drive.FolderListID[FoldersListBox.SelectedIndex]);
                }
            }
            else if (e.KeyCode == Keys.Back)
            {
                if (GoBackButton.Enabled)
                {
                    PopulateListBoxes(drive, "back");
                }
            }
            else if (e.KeyCode == Keys.F5)
                PopulateListBoxes(drive, drive.currentFolder, true);
        }

        private void FilesListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Form1 MainForm = (Form1)this.Owner;
                foreach (string item in FilesListBox.SelectedItems)
                    MainForm.addToList(item, drive.FileListID[FilesListBox.Items.IndexOf(item)], 2);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
                for (int i = 0; i < FilesListBox.Items.Count; i++)
                    FilesListBox.SetSelected(i, true);
            else if (e.KeyCode == Keys.F5)
                PopulateListBoxes(drive, drive.currentFolder, true);
            e.SuppressKeyPress = true;
        }
    }
}
