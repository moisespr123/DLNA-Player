using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLNAPlayer
{
    public partial class GDriveForm : Form
    {
        public GDrive drive;
        public GDriveForm()
        {
            InitializeComponent();
        }
          
        private void button1_Click(object sender, EventArgs e)
        {
            // Form1 test = (Form1)this.Owner;
            // test.addToList("test");
        }

        private void GDriveForm_Load(object sender, EventArgs e)
        {
            drive = new GDrive();
            PopulateListBoxes(drive);

        }
        private void PopulateListBoxes(GDrive drive, string location = "root")
        {
            drive.GetData(location);
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
                        FilesListBox .Items.Add(item);
                    }
                }
                if (location == "root")
                    GoBackButton.Enabled = false;
                else
                    GoBackButton.Enabled = true;
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
            PopulateListBoxes(drive, drive.previousFolder);
        }

        private void FoldersListBox_DoubleClick(object sender, EventArgs e)
        {
            if (FoldersListBox.SelectedIndex > -1)
                PopulateListBoxes(drive, drive.FolderListID[FoldersListBox.SelectedIndex]);
        }
    }
}
