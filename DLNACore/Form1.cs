using System;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Threading.Tasks;

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
            Thread TH = new Thread(() =>
            {
                ScanRenderers.Invoke((MethodInvoker)delegate { ScanRenderers.Text = "Scanning..."; });
                DLNA.SSDP.Start();//Start a service as this will take a long time
                Thread.Sleep(5000);//Wait for each TV/Device to reply to the broadcast
                DLNA.SSDP.Stop();//Stop the service if it has not stopped already
                MediaRenderers.Invoke((MethodInvoker)delegate { MediaRenderers.Items.Clear(); });
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
                    MediaRenderers.Invoke((MethodInvoker)delegate { MediaRenderers.Items.Add(deviceInfo); });
                }
                ScanRenderers.Invoke((MethodInvoker)delegate { ScanRenderers.Text = "Scan Media Renderers"; });
            });
            TH.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IPandPortTxt.Text = Extentions.Helper.GetMyIP() + ":9090";
            ApplyServerIPAndPort();
            timer1.Interval = 500;
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
            MediaFileLocation.Clear();
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
            Thread TH = new Thread(() =>
            {
                Invoke((MethodInvoker)delegate
                {
                    if (MediaRenderers.SelectedIndex != -1)
                    {
                        DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                        if (Device.IsConnected())
                        {
                            if (timer1.Enabled) timer1.Stop();
                            Device.StopPlay();
                            FileStream MediaFile = new FileStream(file_to_play, FileMode.Open);
                            MServer.FS = new MemoryStream();
                            MediaFile.CopyTo(MServer.FS);
                            MediaFile.Close();
                            string Reply = Device.TryToPlayFile("http://" + ip + ":" + port.ToString() + "/file");
                            if (Reply == "OK")
                            {
                                if (!timer1.Enabled) timer1.Start();
                            }
                            else
                                MessageBox.Show("Error playing file");
                        }
                    }
                    else
                        MessageBox.Show("No renderer selected");
                });
            });
            TH.Start();
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            Thread TH = new Thread(() =>
            {
                Invoke((MethodInvoker)delegate
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
                                if (!timer1.Enabled) timer1.Start();
                            }
                            else
                            {
                                Device.Pause();
                                paused = true;
                                Pause.Text = "Resume";
                                if (timer1.Enabled) timer1.Stop();
                            }
                        }
                    }
                });
            });
            TH.Start();
        }
        private void Stop_Click(object sender, EventArgs e)
        {
            Thread TH = new Thread(() =>
            {
                Invoke((MethodInvoker)delegate
                {
                    if (MediaRenderers.SelectedIndex != -1)
                    {
                        DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                        if (Device.IsConnected())
                        {
                            Device.StopPlay();
                            if (timer1.Enabled) timer1.Stop();
                        }
                    }
                });
            });
            TH.Start();
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
            PlayNextTrack();
        }

        private void PlayNextTrack()
        {
            if (MediaFiles.Items.Count > 0 && trackNum < MediaFiles.Items.Count - 1)
            {
                LoadFile(MediaFileLocation[trackNum + 1]);
                MediaFiles.SelectedIndex = trackNum + 1;
                trackNum++;
            }
        }
        private void AboutLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("GUI created by Moisés Cardona" + Environment.NewLine +
                "Version 0.1" + Environment.NewLine +
                "GitHub: https://github.com/moisesmcardona/DLNA-Player" + Environment.NewLine + Environment.NewLine +
                "This software cointains code based on the following Open Source code from CodeProject:" + Environment.NewLine +
                "DLNAMediaServer: https://www.codeproject.com/Articles/1079847/DLNA-Media-Server-to-feed-Smart-TVs" + Environment.NewLine +
                "DLNACore: https://www.codeproject.com/articles/893791/dlna-made-easy-with-play-to-from-any-device");
        }

        private void trackProgress_Scroll(object sender, EventArgs e)
        {
            Thread TH = new Thread(() =>
            {
                Invoke((MethodInvoker)delegate
                {
                    TimeSpan positionToGo = TimeSpan.FromSeconds(trackProgress.Value);
                    DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                    if (Device.IsConnected())
                        Device.Seek(String.Format("{0:c}", positionToGo));
                });
            });
            TH.Start();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                MediaRenderers.Invoke((MethodInvoker)delegate
                {
                if  (MediaRenderers.SelectedIndex != -1)
                    {
                        DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                        if (Device.IsConnected())
                        {
                            string info = Device.GetPosition();
                            string trackDurationString = info.ChopOffBefore("<TrackDuration>").Trim().ChopOffAfter("</TrackDuration>");
                            string trackPositionString = info.ChopOffBefore("<RelTime>").Trim().ChopOffAfter("</RelTime>");
                            try
                            {
                                TimeSpan trackDurationTimeSpan = TimeSpan.Parse(trackDurationString);
                                TimeSpan trackPositionTimeStan = TimeSpan.Parse(trackPositionString);
                                TrackDurationLabel.Invoke((MethodInvoker)delegate { TrackDurationLabel.Text = trackDurationString; });
                                TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = trackPositionString; });
                                trackProgress.Invoke((MethodInvoker)delegate { trackProgress.Maximum = Convert.ToInt32(trackDurationTimeSpan.TotalSeconds); trackProgress.Value = Convert.ToInt32(trackPositionTimeStan.TotalSeconds); });
                                if (Convert.ToInt32(trackDurationTimeSpan.TotalSeconds) - Convert.ToInt32(trackPositionTimeStan.TotalSeconds) <= 1) //(trackDurationString == trackPositionString)
                                {
                                    timer1.Stop();
                                    PlayNextTrack();
                                }
                            }
                            catch { }
                        }
                    }
                });
            });
        }
    }
}
