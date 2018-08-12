using System;
using System.Windows.Forms;

namespace DLNAPlayer
{
    public partial class CDDriveChooser : Form
    {

        public CDDriveChooser()
        {
            InitializeComponent();
        }
        public static AudioCD drive = new AudioCD();
        private void CDDriveChooser_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < drive.DriveList.Count; i++)
                driveComboBox.Items.Add(drive.DriveList[i].ToString());
            if (driveComboBox.Items.Count > 0)
            {
                driveComboBox.SelectedItem = driveComboBox.Items[0];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (driveComboBox.SelectedIndex > -1)
                drive.AudioTracks.Clear();
            if (drive.ready(driveComboBox.SelectedIndex))
            {
                foreach (int item in drive.AudioTracks)
                {
                    Form1 MainForm = (Form1)this.Owner;
                    MainForm.addToList("Track " + item.ToString() + ".wav", item.ToString(), 3);
                }
                this.Close();
            }
            else
                MessageBox.Show("The selected drive is not ready. Check if there's an Audio CD inserted");
        }
    }
}
