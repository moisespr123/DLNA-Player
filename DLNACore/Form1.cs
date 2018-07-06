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
        private static int trackNum = -1;
        private static bool paused = false;
        private static List<String> MediaFileLocation = new List<string> { };

        private void CmdSSDP_Click(object sender, EventArgs e)
        {
            DLNA.SSDP.Start();//Start a service as this will take a long time
            Thread.Sleep(5000);//Wait for each TV/Device to reply to the broadcast
            DLNA.SSDP.Stop();//Stop the service if it has not stopped already
            MediaRenderers.Items.Clear();
            for (int i = 0; i < DLNA.SSDP.Renderers.Count; i++)
            {
                String deviceInfo = "";
                XmlDocument RendererXML = new XmlDocument();
                try
                {
                    RendererXML.Load(DLNA.SSDP.Renderers[i]);
                    XmlElement rootXML = RendererXML.DocumentElement;
                    deviceInfo = rootXML.GetElementsByTagName("friendlyName")[0].InnerText;
                }
                catch
                {
                    deviceInfo = DLNA.SSDP.Renderers[i];
                }
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
            {
                LoadFile(MediaFileLocation[MediaFiles.SelectedIndex]);
                trackNum = MediaFiles.SelectedIndex;
            }
            else if (MediaFiles.Items.Count > 0)
            {
                LoadFile(MediaFileLocation[0]);
                trackNum = 0;
                MediaFiles.SelectedIndex = 0;
            }

        }

        private void ClearQueue_Click(object sender, EventArgs e)
        {
            MediaFiles.Items.Clear();
            trackNum = -1;
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
            string[] parseIPandPort = IPandPortTxt.Text.Split(':');
            ip = parseIPandPort[0];
            port = 9090;
            if (!String.IsNullOrEmpty(parseIPandPort[1]))
                port = Convert.ToInt32(parseIPandPort[1]);
            if (MServer != null) if (MServer.Running) MServer.Stop();
            MServer = new MediaServer(ip, port);
            MServer.Start();
        }

        private void MediaFiles_DoubleClick(object sender, EventArgs e)
        {
            LoadFile(MediaFileLocation[MediaFiles.SelectedIndex]);
            trackNum = MediaFiles.SelectedIndex;
        }
        private void LoadFile(string file_to_play)
        {
            if (MediaRenderers.SelectedIndex != -1)
            {
                DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                if (Device.IsConnected())
                {
                    Device.StopPlay();
                    FileStream MediaFile = new FileStream(file_to_play, FileMode.Open);
                    MServer.FS = new MemoryStream();
                    MediaFile.CopyTo(MServer.FS);
                    MediaFile.Close();
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

        private void Pause_Click(object sender, EventArgs e)
        {
            if (MediaRenderers.SelectedIndex != -1)
            {
                DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                if (Device.IsConnected())
                {
                    if (paused)
                    {
                        Device.StartPlay(0);
                        paused = false;
                        Pause.Text = "Pause";
                    }
                    else
                    {
                        Device.Pause();
                        paused = true;
                        Pause.Text = "Resume";
                    }
                }
            }
        }
        private void Stop_Click(object sender, EventArgs e)
        {
            if (MediaRenderers.SelectedIndex != -1)
            {
                DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                if (Device.IsConnected())
                {
                    Device.StopPlay();
                }
            }
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (MediaFiles.Items.Count > 0 && trackNum > 0)
            {
                LoadFile(MediaFileLocation[trackNum - 1]);
                MediaFiles.SelectedIndex = trackNum - 1;
                trackNum--;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (MediaFiles.Items.Count > 0 && trackNum < MediaFiles.Items.Count - 1)
            {
                LoadFile(MediaFileLocation[trackNum + 1]);
                MediaFiles.SelectedIndex = trackNum + 1;
                trackNum++;
            }
        }
    }
}
