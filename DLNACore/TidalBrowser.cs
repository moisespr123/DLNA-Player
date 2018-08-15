using System;
using System.Windows.Forms;

namespace DLNAPlayer
{
    public partial class TidalBrowser : Form
    {
        public static Tidl tidl = new Tidl();
        public TidalBrowser()
        {
            InitializeComponent();
            TidalLogin login = new TidalLogin();
            login.ShowDialog();
            if (tidl.isLoggedIn)
            {
                albumsListBox.Items.Clear();
                foreach (string item in tidl.AlbumNames)
                {
                    albumsListBox.Items.Add(item);
                    albumsListBox.Update();
                }
            }
            else
                this.Close();
        }
        private async void albumsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (albumsListBox.SelectedIndex > -1)
            {

                await tidl.getTracks(albumsListBox.SelectedIndex);
                if (tidl.TrackNames.Count > 0)
                {
                    tracksListBox.Items.Clear();
                    foreach (string track in tidl.TrackNames)
                    {
                        tracksListBox.Items.Add(track);
                    }
                }
            }
        }

        private void addTracks_Click(object sender, EventArgs e)
        {
            Form1 MainForm = (Form1)this.Owner;
            foreach (string item in tracksListBox.SelectedItems)
                MainForm.addToList(item, tidl.TrackIDs[tracksListBox.Items.IndexOf(item)].ToString(), 4);
        }

        private void tracksListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Form1 MainForm = (Form1)this.Owner;
                foreach (string item in tracksListBox.SelectedItems)
                    MainForm.addToList(item, tidl.TrackIDs[tracksListBox.Items.IndexOf(item)].ToString(), 4);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
                for (int i = 0; i < tracksListBox.Items.Count; i++)
                    tracksListBox.SetSelected(i, true);
            e.SuppressKeyPress = true;
        }

        private void tracksListBox_DoubleClick(object sender, MouseEventArgs e)
        {
            if (tracksListBox.SelectedIndex > -1)
            {
                Form1 MainForm = (Form1)this.Owner;
                MainForm.addToList(tracksListBox.SelectedItem.ToString(), tidl.TrackIDs[tracksListBox.Items.IndexOf(tracksListBox.SelectedItem)].ToString(), 4);
            }
        }

        private async void albumsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                await tidl.getAlbums();
                albumsListBox.Items.Clear();
                foreach (string item in tidl.AlbumNames)
                {
                    albumsListBox.Items.Add(item);
                    albumsListBox.Update();
                }
            }
        }
    }
}
