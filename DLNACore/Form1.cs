using System;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;

namespace DLNAPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CmdSSDP_Click(object sender, EventArgs e)
        {
            DLNA.SSDP.Start();//Start a service as this will take a long time
            Thread.Sleep(5000);//Wait for each TV/Device to reply to the broadcast
            DLNA.SSDP.Stop();//Stop the service if it has not stopped already
            for (int i = 0; i < DLNA.SSDP.Renderers.Count; i++)
            {
                XmlDocument RendererXML = new XmlDocument();
                RendererXML.Load(DLNA.SSDP.Renderers[i]);
                XmlElement rootXML = RendererXML.DocumentElement;
                String deviceInfo = rootXML.GetElementsByTagName("friendlyName")[0].InnerText;
                MediaRenderers.Items.Add(deviceInfo);
            }
            //this.textBox1.Text = DLNA.SSDP.Servers;//Best to save this string to a file or windows registry as we don't want to keep looking for devices on the network
            //if (this.textBox1.Text.Length < 10)
            //    this.textBox1.Text = "Are you sure that your smart TV and devices are turned on !";

        }
        private MediaServer MServer = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            //filestream = new MemoryStream();
            //FileStream temp = new FileStream((@"C:\Users\Moises Cardona\Music\Re-Encoded\2NE1 - Crush\1 - 2 - Come Back Home.flac"), FileMode.Open);

            //MServer = new MediaServer("192.168.11.41", 9090);
            //MServer.FS = new MemoryStream();
            //temp.CopyTo(MServer.FS);
            //temp.Close();
            //MServer.Start();
        }
        public static MemoryStream filestream;
        private void CmdPlay_Click(object sender, EventArgs e)
        {
            DLNA.DLNADevice Device = new DLNA.DLNADevice("http://192.168.11.80:49152/description.xml");//You will need to Keep a referance to each device so that you can stop it playing or what ever and don't need to keep calling "IsConnected();"
            if (Device.IsConnected())//Will make sure that the device is switched on and runs a avtransport:1 service protocol
            {//Best to use an IP-Address and not a machine name because the TV/Device might not be able to resolve a machine/domain name
                //string test = Device.GetPosition();
                string Reply = Device.TryToPlayFile("http://192.168.11.41:9090/file.flac");
                if (Reply == "OK")
                { }
                //    this.textBox1.Text += Environment.NewLine + "Playing to " + Device.FriendlyName;
                else
                    MessageBox.Show("Error playing file");

            }
        }

        private void ClearQueue_Click(object sender, EventArgs e)
        {
            MediaFiles.Items.Clear();
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] filepath = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string path in filepath)
                if (!System.IO.Directory.Exists(path))
                    MediaFiles.Items.Add(path);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }
    }
}
