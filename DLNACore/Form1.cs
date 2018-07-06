using System;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace DLNAPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private MediaServer MServer = null;
        private static string ip = "";
        private static int port = 9090;
        private static List<String> MediaFileLocation = new List<string> { };
        private string path;

        private void CmdSSDP_Click(object sender, EventArgs e)
        {
            DLNA.SSDP.Start();//Start a service as this will take a long time
            Thread.Sleep(5000);//Wait for each TV/Device to reply to the broadcast
            DLNA.SSDP.Stop();//Stop the service if it has not stopped already
            MediaRenderers.Items.Clear();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            IPandPortTxt.Text = Extentions.Helper.GetMyIP() + ":9090";
            ApplyServerIPAndPort();

            //temp.Close();
            //MServer.Start();
        }

        private void CmdPlay_Click(object sender, EventArgs e)
        {
            if (MediaFiles.SelectedIndex != -1)
                LoadFile(MediaFileLocation[MediaFiles.SelectedIndex]);
        }

        private void ClearQueue_Click(object sender, EventArgs e)
        {
            MediaFiles.Items.Clear();
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] filepath = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string path in filepath)
                if (!Directory.Exists(path))
                {
                    MediaFileLocation.Add(path);
                    MediaFiles.Items.Add(Path.GetFileName(path));
                }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void ApplyServerIP_Click(object sender, EventArgs e)
        {
            ApplyServerIPAndPort();
        }
        private void ApplyServerIPAndPort()
        {
            if (MServer != null && MServer.Running)
            {
                MServer.Stop();
                string[] parseIPandPort = IPandPortTxt.Text.Split(':');
                ip = parseIPandPort[0];
                port = 9090;
                if (!String.IsNullOrEmpty(parseIPandPort[1]))
                    port = Convert.ToInt32(parseIPandPort[1]);
                else
                    MServer = new MediaServer(ip, port);
                MServer.Start();
            }
        }

        private void MediaFiles_DoubleClick(object sender, EventArgs e)
        {
            LoadFile(MediaFileLocation[MediaFiles.SelectedIndex]);
        }
        private void LoadFile(string file_to_play)
        {
            if (MediaRenderers.SelectedIndex != -1)
            {
                DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                if (Device.IsConnected())
                {
                    Device.StopPlay();
                    FileStream MediaFile = new FileStream(MediaFileLocation[MediaFiles.SelectedIndex], FileMode.Open);
                    MediaFile.CopyTo(MServer.FS);
                    string Reply = Device.TryToPlayFile("http://" + ip + ":" + port.ToString() + "/file.flac");
                    if (Reply == "OK")
                    { }
                    //    this.textBox1.Text += Environment.NewLine + "Playing to " + Device.FriendlyName;
                    else
                        MessageBox.Show("Error playing file");
                }
            }
            else
                MessageBox.Show("No renderer selected");
        }
    }
}
