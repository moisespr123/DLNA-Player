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
            try
            {
                if (drive.FolderList.Count > 0)
                {
                    FoldersListBox.DataSource = drive.FolderList;
                }
                if (drive.FileList.Count > 0)
                {
                    FilesListBox.DataSource = drive.FileList;
                }
            }
            catch
            {
                MessageBox.Show("Error loading Google Drive contents");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PopulateListBoxes(drive);
        }
    }
}
