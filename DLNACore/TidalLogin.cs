using System;
using System.Windows.Forms;

namespace DLNAPlayer
{
    public partial class TidalLogin : Form
    {
        public TidalLogin()
        {
            InitializeComponent();
        }
        public static Tidl tidl = TidalBrowser.tidl;
        private async void button1_Click(object sender, EventArgs e)
        {
            if (await tidl.login(userNameTxtBx.Text, passTxtBx.Text))
            {
                this.Close();
            }
            else
                MessageBox.Show("Username or password is incorrect");
        }
    }
}
