using System;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;

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
        private static List<string> MediaFileLocation = new List<string> { };
        private static List<int> MediaFileLocationType = new List<int> { };
        private static MemoryStream NextTrack = new MemoryStream();
        private static int trackLoaded = -1;
        private static bool trackLoading = false;
        private static int status = 0;
        private static int renderer = -1;
        string[] mediainfo = { "Unknown", "Unknown" };
        string[] nextMediainfo = { "Unknown", "Unknown" };
        private System.Timers.Timer timer1 = new System.Timers.Timer(1000);

        private void ScanDLNARenderers()
        {
            Thread TH = new Thread(() =>
            {

                ScanRenderers.Invoke((MethodInvoker)delegate { ScanRenderers.Text = "Scanning... Press to stop"; });
                while (true)
                {
                    Thread.Sleep(1000);
                    DLNA.SSDP.Start();
                    for (int i = 0; i < DLNA.SSDP.Renderers.Count; i++)
                    {
                        string deviceInfo = "";
                        XmlDocument RendererXML = new XmlDocument();
                        try
                        {
                            RendererXML.Load(DLNA.SSDP.Renderers[i]);
                            XmlElement rootXML = RendererXML.DocumentElement;
                            deviceInfo = rootXML.GetElementsByTagName("friendlyName")[0].InnerText + " (" + DLNA.SSDP.Renderers[i] + ")";
                        }
                        catch
                        {
                            deviceInfo = DLNA.SSDP.Renderers[i];
                        }
                        if (!MediaRenderers.Items.Contains(deviceInfo))
                            MediaRenderers.Invoke((MethodInvoker)delegate { MediaRenderers.Items.Add(deviceInfo); });
                    }
                    if (!DLNA.SSDP.Run) break;
                }
            });
            TH.Start();
        }
        private void CmdSSDP_Click(object sender, EventArgs e)
        {
            if (DLNA.SSDP.Run)
            {
                DLNA.SSDP.Stop();
                ScanRenderers.Invoke((MethodInvoker)delegate { ScanRenderers.Text = "Scan Media Renderers"; });
            }
            else
            {
                DLNA.SSDP.Run = true;
                ScanDLNARenderers();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IPandPortTxt.Text = Extentions.Helper.GetMyIP() + ":9090";
            ApplyServerIPAndPort();
            timer1.Interval = 500;
            decodeOpusToWAVToolStripMenuItem.Checked = Properties.Settings.Default.DecodeOpus;
            decodeFLACToWAVToolStripMenuItem.Checked = Properties.Settings.Default.DecodeFLAC;
            decodeMP3ToWAVToolStripMenuItem.Checked = Properties.Settings.Default.DecodeMP3;
            decodeM4AToWAVToolStripMenuItem.Checked = Properties.Settings.Default.DecodeM4A;
            decodeWMAToWAVToolStripMenuItem.Checked = Properties.Settings.Default.DecodeWMA;
            useFfmpegForDecodingToolStripMenuItem.Checked = Properties.Settings.Default.UseFFMPEG;
            decodeToFLACInsteadOfWAVToolStripMenuItem.Checked = Properties.Settings.Default.DecodeToFLAC;

            bool foundFfmpeg = testSoftware("ffmpeg.exe");
            bool foundFlac = testSoftware("flac.exe");
            bool foundOpus = testSoftware("opusdec.exe");
            if (!foundFfmpeg)
            {
                decodeMP3ToWAVToolStripMenuItem.Checked = false;
                decodeM4AToWAVToolStripMenuItem.Checked = false;
                decodeWMAToWAVToolStripMenuItem.Checked = false;
                useFfmpegForDecodingToolStripMenuItem.Checked = false;
                decodeToFLACInsteadOfWAVToolStripMenuItem.Checked = false;
                decodeMP3ToWAVToolStripMenuItem.Enabled = false;
                decodeM4AToWAVToolStripMenuItem.Enabled = false;
                decodeWMAToWAVToolStripMenuItem.Enabled = false;
                useFfmpegForDecodingToolStripMenuItem.Enabled = false;
                decodeToFLACInsteadOfWAVToolStripMenuItem.Enabled = false;
                Properties.Settings.Default.UseFFMPEG = false;
                Properties.Settings.Default.DecodeToFLAC = false;
                Properties.Settings.Default.Save();
            }
            if (!foundFlac)
            {
                decodeFLACToWAVToolStripMenuItem.Checked = false;
                decodeFLACToWAVToolStripMenuItem.Enabled = false;
            }
            if (!foundOpus)
            {
                decodeOpusToWAVToolStripMenuItem.Checked = false;
                decodeOpusToWAVToolStripMenuItem.Enabled = false;
            }

            ScanDLNARenderers();
            timer1.Elapsed += TimerTick;
        }

        private void Play()
        {
            status = 1;
            if (MediaFiles.SelectedIndex != -1)
            {
                trackNum = MediaFiles.SelectedIndex;
                Thread thread = new Thread(() => LoadFile(trackNum));
                thread.Start();
                ;
            }
            else if (MediaFiles.Items.Count > 0)
            {
                trackNum = 0;
                MediaFiles.SelectedIndex = 0;
                Thread thread = new Thread(() => LoadFile(trackNum));
                thread.Start();
            }
        }
        private void CmdPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void ClearQueue_Click(object sender, EventArgs e)
        {
            MediaFiles.Items.Clear();
            MediaFileLocation.Clear();
            MediaFileLocationType.Clear();
            trackNum = -1;
            trackLoaded = -1;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] filepath = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string path in filepath)
                if (!Directory.Exists(path))
                {
                    addToList(Path.GetFileName(path), path, 1);
                }
                else
                {
                    DirectoryInfo DI = new DirectoryInfo(path);
                    foreach (FileInfo FI in DI.GetFiles())
                    {
                        addToList(Path.GetFileName(FI.FullName), FI.FullName, 1);
                    }
                }
            if (trackLoaded == -1 && !trackLoading)
            {
                Thread thread = new Thread(() => LoadNextTrack(trackNum + 1));
                thread.Start();
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
            if (MServer != null)
                MServer.Stop();
            string[] parseIPandPort = IPandPortTxt.Text.Split(':');
            ip = parseIPandPort[0];
            port = 9090;
            if (!string.IsNullOrEmpty(parseIPandPort[1]))
                port = Convert.ToInt32(parseIPandPort[1]);
            if (MServer != null) if (MServer.Running) MServer.Stop();
            MServer = new MediaServer(ip, port);
            MServer.Start();
        }

        private void MediaFiles_DoubleClick(object sender, EventArgs e)
        {
            Play();
        }
        private async void LoadNextTrack(int item)
        {
            string file_to_play = MediaFileLocation[item];
            int location_type = MediaFileLocationType[item];
            string filename = MediaFiles.Items[item].ToString();
            trackLoading = true;
            {
                try
                {
                    TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Loading"; });
                    NextTrack = new MemoryStream();
                    if (location_type == 1) //local file 
                    {
                        nextMediainfo = await Extentions.getMetadata(file_to_play);
                        int decodeMode = 0;
                        if (file_to_play.EndsWith(".opus") && decodeOpusToWAVToolStripMenuItem.Checked)
                        {
                            decodeMode = 1;
                        }
                        else if (file_to_play.EndsWith(".flac") && decodeFLACToWAVToolStripMenuItem.Checked)
                        {
                            decodeMode = 2;
                        }
                        else if ((file_to_play.EndsWith(".mp3") && decodeMP3ToWAVToolStripMenuItem.Checked) || (file_to_play.EndsWith(".m4a") && decodeM4AToWAVToolStripMenuItem.Checked) ||
                                 (filename.EndsWith(".wma") && decodeWMAToWAVToolStripMenuItem.Checked))
                        {
                            decodeMode = 3;
                        }
                        if (decodeMode != 0)
                        {
                            TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Decoding"; });
                            NextTrack = await Extentions.decodeAudio(file_to_play, decodeMode);
                        }
                        else
                        {
                            FileStream MediaFile = new FileStream(file_to_play, FileMode.Open, FileAccess.Read);
                            await MediaFile.CopyToAsync(NextTrack);
                            MediaFile.Close();
                        }
                    }
                    else if (location_type == 2) //Google Drive file
                    {
                        GDrive drive = GDriveForm.drive;
                        string tempFilename = Path.GetTempFileName();
                        TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Downloading"; });
                        await drive.DownloadFile(file_to_play, tempFilename);
                        TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Downloaded"; });
                        nextMediainfo = await Extentions.getMetadata(tempFilename);
                        int decodeMode = 0;
                        if (filename.EndsWith(".opus") && decodeOpusToWAVToolStripMenuItem.Checked)
                        {
                            decodeMode = 1;
                        }
                        else if (filename.EndsWith(".flac") && decodeFLACToWAVToolStripMenuItem.Checked)
                        {
                            decodeMode = 2;
                        }
                        else if ((filename.EndsWith(".mp3") && decodeMP3ToWAVToolStripMenuItem.Checked) || (filename.EndsWith(".m4a") && decodeM4AToWAVToolStripMenuItem.Checked) ||
                                 (filename.EndsWith(".wma") && decodeWMAToWAVToolStripMenuItem.Checked))
                        {
                            decodeMode = 3;
                        }
                        if (decodeMode != 0)
                        {
                            TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Decoding"; });
                            NextTrack = await Extentions.decodeAudio(tempFilename, decodeMode);
                        }
                        else
                        {
                            FileStream MediaFile = new FileStream(tempFilename, FileMode.Open, FileAccess.Read);
                            await MediaFile.CopyToAsync(NextTrack);
                            MediaFile.Close();
                        }
                        Extentions.deleteTempFile(tempFilename);
                    }
                    else if (location_type == 3) //CD Drive Audio Track
                    {
                        AudioCD drive = CDDriveChooser.drive;
                        NextTrack = drive.getTrack(file_to_play);
                        nextMediainfo[0] = "Track " + (item + 1).ToString();
                        nextMediainfo[1] = "Audio CD";
                        mediainfo[1] = string.Empty;
                    }
                    trackLoaded = item;
                    trackLoading = false;
                    TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Ready"; });
                }
                catch { trackLoading = false; }
            }
        }
        private async void LoadFile(int item = 0)
        {
            string file_to_play = MediaFileLocation[item];
            int location_type = MediaFileLocationType[item];
            string filename = MediaFiles.Items[item].ToString();
            if (renderer != -1)
            {
                DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[renderer]);
                if (Device.IsConnected())
                {
                    if (timer1.Enabled) timer1.Stop();
                    Device.StopPlay();
                    MServer.FS = new MemoryStream();
                    if ((filename.EndsWith(".opus") && decodeOpusToWAVToolStripMenuItem.Checked) || (filename.EndsWith(".flac") && decodeFLACToWAVToolStripMenuItem.Checked) ||
                        (filename.EndsWith(".mp3") && decodeMP3ToWAVToolStripMenuItem.Checked) || (filename.EndsWith(".m4a") && decodeM4AToWAVToolStripMenuItem.Checked) ||
                        (filename.EndsWith(".wma") && decodeWMAToWAVToolStripMenuItem.Checked))
                    {
                        MServer.Filename = Path.GetFileNameWithoutExtension(filename) + ".wav";
                        if (decodeToFLACInsteadOfWAVToolStripMenuItem.Checked)
                        {
                            MServer.Filename = Path.GetFileNameWithoutExtension(filename) + ".flac";
                        }
                    }
                    else
                        MServer.Filename = filename;
                    string url = null;
                    if (item != trackLoaded)
                    {
                        TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Loading"; });
                        if (location_type == 1) //local file 
                        {
                            mediainfo = await Extentions.getMetadata(file_to_play);
                            int decodeMode = 0;
                            if (file_to_play.EndsWith(".opus") && decodeOpusToWAVToolStripMenuItem.Checked)
                            {
                                decodeMode = 1;
                            }
                            else if (file_to_play.EndsWith(".flac") && decodeFLACToWAVToolStripMenuItem.Checked)
                            {
                                decodeMode = 2;
                            }
                            else if ((file_to_play.EndsWith(".mp3") && decodeMP3ToWAVToolStripMenuItem.Checked) ||
                                     (file_to_play.EndsWith(".m4a") && decodeM4AToWAVToolStripMenuItem.Checked) ||
                                     (filename.EndsWith(".wma") && decodeWMAToWAVToolStripMenuItem.Checked))
                            {
                                decodeMode = 3;
                            }
                            if (decodeMode != 0)
                            {
                                TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Decoding"; });
                                MServer.FS = await Extentions.decodeAudio(file_to_play, decodeMode);
                            }
                            else
                            {
                                FileStream MediaFile = new FileStream(file_to_play, FileMode.Open, FileAccess.Read);
                                await MediaFile.CopyToAsync(MServer.FS);
                                MediaFile.Close();
                            }
                        }
                        else if (location_type == 2) //Google Drive file (local download)
                        {
                            GDrive drive = GDriveForm.drive;
                            string tempFilename = Path.GetTempFileName();
                            TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Downloading"; });
                            await drive.DownloadFile(file_to_play, tempFilename);
                            TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Downloaded"; });
                            int decodeMode = 0;
                            mediainfo = await Extentions.getMetadata(tempFilename);
                            if (filename.EndsWith(".opus") && decodeOpusToWAVToolStripMenuItem.Checked)
                            {
                                decodeMode = 1;
                            }
                            else if (filename.EndsWith(".flac") && decodeFLACToWAVToolStripMenuItem.Checked)
                            {
                                decodeMode = 2;
                            }
                            else if ((filename.EndsWith(".mp3") && decodeMP3ToWAVToolStripMenuItem.Checked) || (filename.EndsWith(".m4a") && decodeM4AToWAVToolStripMenuItem.Checked) ||
                                     (filename.EndsWith(".wma") && decodeWMAToWAVToolStripMenuItem.Checked))
                            {
                                decodeMode = 3;
                            }
                            if (decodeMode != 0)
                            {
                                TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Decoding"; });
                                MServer.FS = await Extentions.decodeAudio(tempFilename, decodeMode);
                            }
                            else
                            {
                                FileStream MediaFile = new FileStream(tempFilename, FileMode.Open, FileAccess.Read);
                                await MediaFile.CopyToAsync(MServer.FS);
                                MediaFile.Close();
                            }
                            Extentions.deleteTempFile(tempFilename);
                        }
                        else if (location_type == 3) //CD Drive Audio Track
                        {
                            AudioCD drive = CDDriveChooser.drive;
                            MServer.FS = drive.getTrack(file_to_play);
                            mediainfo[0] = "Track " + (trackNum + 1).ToString();
                            mediainfo[1] = "Audio CD";
                        }
                        else if (location_type == 4) //Tidal Track
                        {
                            Tidl tidl = TidalBrowser.tidl;
                            url = await tidl.getStreamURL(Convert.ToInt32(file_to_play));
                            mediainfo = await tidl.getNameAndArtist(Convert.ToInt32(file_to_play));
                        }
                        else if (location_type == 5) //Google Drive file (stream)
                        {
                            GDrive drive = GDriveForm.drive;
                            try
                            {
                                url = await drive.GetUrl(file_to_play);
                                mediainfo[0] = "Unknown";
                                mediainfo[1] = "Unknown";
                            }
                            catch
                            {
                                MessageBox.Show("Cannot stream the file directly. Please change the Google Drive mode to \"download first\" and add them again to the queue.");
                            }
                        }
                        TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Ready"; });
                    }
                    else
                    {
                        nextMediainfo.CopyTo(mediainfo, 0);
                        MServer.FS = NextTrack;
                    }
                    Thread.Sleep(100);
                    if (location_type != 4 && location_type != 5)
                        url = "http://" + ip + ":" + port.ToString() + "/track" + Path.GetExtension(MServer.Filename);
                    if (status != 0)
                        SendFile(Device, item, url, mediainfo);

                }
                else
                {
                    MessageBox.Show("Could not connect to device.");
                }
            }
            else
                MessageBox.Show("No renderer selected");
        }

        private string SendFile(DLNA.DLNADevice Device, int item, string url, string[] mediainfo = null)
        {
            string returnedvalue = Device.TryToPlayFile(url, mediainfo);
            if (!timer1.Enabled) timer1.Start();
            if (MediaFiles.Items.Count - 1 > item)
                if (MediaFileLocationType[item + 1] != 4 && MediaFileLocationType[item + 1] != 5)
                {
                    Thread thread = new Thread(() => LoadNextTrack(trackNum + 1));
                    thread.Start();
                    return "OK";
                }
            return "OK";
        }
        private void Pause_Click(object sender, EventArgs e)
        {
            PauseTrack();
        }

        private void PauseTrack(bool updateTextOnly = false)
        {
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
                            UpdatePauseText();
                        }
                        else
                        {
                            Device.Pause();
                            paused = true;
                            UpdatePauseText();
                        }
                    }
                }
            }
        }

        private void UpdatePauseText()
        {
            if (paused)
                Pause.Text = "Resume";
            else
                Pause.Text = "Pause";
        }
        private void Stop_Function()
        {
            status = 0;
            DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
            if (Device.IsConnected())
            {
                Device.StopPlay();
                if (timer1.Enabled) timer1.Stop();
                TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "00:00:00"; });
                TrackDurationLabel.Invoke((MethodInvoker)delegate { TrackDurationLabel.Text = "00:00:00"; });
                trackProgress.Invoke((MethodInvoker)delegate { trackProgress.Maximum = 1; trackProgress.Value = 0; });
            }
        }
        private void Stop_Click(object sender, EventArgs e)
        {
            Stop_Function();
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (MediaFiles.Items.Count > 0 && trackNum > 0)
            {
                Thread thread = new Thread(() => LoadFile(trackNum));
                thread.Start();
                MediaFiles.ClearSelected();
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
            if (MediaFiles.Items.Count > 0 && trackNum < (MediaFiles.Items.Count - 1))
            {
                trackNum++;
                Thread thread = new Thread(() => LoadFile(trackNum));
                thread.Start();
                MediaFiles.ClearSelected();
                MediaFiles.SelectedIndex = trackNum;
            }
            else
            {
                trackNum = -1;
                Stop_Function();
            }
        }

        private void TimerTick(Object source, ElapsedEventArgs e)
        {
            MediaRenderers.Invoke((MethodInvoker)delegate
            {
                if (MediaRenderers.SelectedIndex != -1)
                {
                    DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                    if (Device.IsConnected())
                    {
                        string info = Device.GetPosition();
                        string status = Device.GetTransportInfo();
                        string trackDurationString = info.ChopOffBefore("<TrackDuration>").Trim().ChopOffAfter("</TrackDuration>");
                        string trackPositionString = info.ChopOffBefore("<RelTime>").Trim().ChopOffAfter("</RelTime>");
                        string currentStatus = status.ChopOffBefore("<CurrentTransportState>").Trim().ChopOffAfter("</CurrentTransportState>");
                        if (currentStatus != "TRANSITIONING")
                        {
                            if (!trackDurationString.Contains("HTTP"))
                                if (trackDurationString.Contains(":") && trackPositionString.Contains(":"))
                                    try
                                    {
                                        TimeSpan trackDurationTimeSpan = TimeSpan.Parse(trackDurationString);
                                        TimeSpan trackPositionTimeSpan = TimeSpan.Parse(trackPositionString);
                                        if (currentStatus == "PAUSED_PLAYBACK" && !paused)
                                        {
                                            paused = true;
                                            UpdatePauseText();
                                        }
                                        else if (currentStatus == "PLAYING")
                                        {
                                            paused = false;
                                            UpdatePauseText();
                                        }
                                        TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = trackPositionString; });
                                        if (Convert.ToInt32(trackDurationTimeSpan.TotalSeconds) != 0)
                                        {
                                            TrackDurationLabel.Invoke((MethodInvoker)delegate { TrackDurationLabel.Text = trackDurationString; });

                                            trackProgress.Invoke((MethodInvoker)delegate { trackProgress.Maximum = Convert.ToInt32(trackDurationTimeSpan.TotalSeconds); trackProgress.Value = Convert.ToInt32(trackPositionTimeSpan.TotalSeconds); });
                                            if (Convert.ToInt32(trackDurationTimeSpan.TotalSeconds) - Convert.ToInt32(trackPositionTimeSpan.TotalSeconds) <= 2)
                                            {
                                                Thread.Sleep(2000);
                                                timer1.Stop();
                                                PlayNextTrack();
                                            }
                                        }
                                        else
                                        {
                                            TrackDurationLabel.Invoke((MethodInvoker)delegate { TrackDurationLabel.Text = ""; });
                                        }
                                    }
                                    catch { }
                        }
                        else
                        {
                            TrackPositionLabel.Invoke((MethodInvoker)delegate { TrackPositionLabel.Text = "Loading"; });
                        }
                    }
                }
            });
        }

        private async void trackProgress_MouseUp(object sender, MouseEventArgs e)
        {
            if (MediaRenderers.SelectedIndex != -1)
            {
                await Task.Run(() =>
                {
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            TimeSpan positionToGo = positionToGo = TimeSpan.FromSeconds(trackProgress.Value);
                            DLNA.DLNADevice Device = new DLNA.DLNADevice(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                            if (Device.IsConnected())
                                Device.Seek(string.Format("{0:c}", positionToGo));
                        });
                    }
                });
            }
        }

        public void addToList(string item, string location, int type)
        {
            MediaFiles.Items.Add(item);
            MediaFileLocation.Add(location);
            MediaFileLocationType.Add(type);
            MediaFiles.Update();
        }

        private void googleDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GDriveForm DriveForm = new GDriveForm()
            {
                Owner = this
            };
            DriveForm.Show();
        }

        private void MediaFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (MediaFiles.SelectedIndex > -1)
                if (e.KeyCode == Keys.Delete)
                {
                    int firstItem = MediaFiles.SelectedIndices[0];
                    int lastItem = MediaFiles.SelectedIndices[MediaFiles.SelectedIndices.Count - 1];
                    if (trackNum >= firstItem)
                    {
                        if (trackLoaded == trackNum + 1)
                        {
                            trackLoaded = firstItem;
                        }
                        trackNum = firstItem - 1;
                    }
                    if (trackLoaded >= firstItem || trackLoaded <= lastItem)
                        trackLoaded = -1;

                    for (int i = lastItem; i >= firstItem; i--)
                    {
                        MediaFiles.Items.RemoveAt(i);
                        MediaFileLocation.RemoveAt(i);
                        MediaFileLocationType.RemoveAt(i);

                    }
                    if (MediaFiles.Items.Count == 0)
                        trackLoaded = -1;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    Play();
                }
                else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
                    for (int i = 0; i < MediaFiles.Items.Count; i++)
                        MediaFiles.SetSelected(i, true);
        }

        private void openFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Browse for media files",
                FileName = "",
                Filter = "Media files|*.*",
                Multiselect = true,
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (fileDialog.FileNames != null)
                {
                    foreach (string path in fileDialog.FileNames)
                        if (!Directory.Exists(path))
                            addToList(Path.GetFileName(path), path, 1);
                    if (trackLoaded == -1)
                    {
                        Thread thread = new Thread(() => LoadNextTrack(trackNum + 1));
                        thread.Start();
                    }
                }
            }

        }

        private void readmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/moisesmcardona/DLNA-Player/blob/master/README.md");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("GUI created by Moisés Cardona" + Environment.NewLine +
              "Version 1.0" + Environment.NewLine +
              "GitHub: https://github.com/moisespr123/DLNA-Player" + Environment.NewLine + Environment.NewLine +
              "This software contains code based on the following Open Source code from CodeProject:" + Environment.NewLine +
              "DLNAMediaServer: https://www.codeproject.com/Articles/1079847/DLNA-Media-Server-to-feed-Smart-TVs" + Environment.NewLine +
              "DLNACore: https://www.codeproject.com/articles/893791/dlna-made-easy-with-play-to-from-any-device" + Environment.NewLine +
              "C Sharp Ripper: https://www.codeproject.com/articles/5458/c-sharp-ripper");
        }

        private void openAudioCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CDDriveChooser driveChooser = new CDDriveChooser()
            {
                Owner = this
            };
            driveChooser.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("DLNAPlayer"))
            {
                process.Kill();
            }
        }

        private void tidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TidalBrowser tidal = new TidalBrowser()
            {
                Owner = this
            };
            try
            {
                tidal.Show();
            }
            catch { }
        }

        private void decodeOpusToWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!decodeOpusToWAVToolStripMenuItem.Checked)
                decodeOpusToWAVToolStripMenuItem.Checked = true;
            else
                decodeOpusToWAVToolStripMenuItem.Checked = false;
            Properties.Settings.Default.DecodeOpus = decodeOpusToWAVToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }
        private bool testSoftware(string software)
        {
            try
            {
                ProcessStartInfo ProcessInfo = new ProcessStartInfo()
                {
                    FileName = software,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                Process process = new Process
                {
                    StartInfo = ProcessInfo
                };
                process.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void decodeFLACToWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!decodeFLACToWAVToolStripMenuItem.Checked)
                decodeFLACToWAVToolStripMenuItem.Checked = true;
            else
                decodeFLACToWAVToolStripMenuItem.Checked = false;
            Properties.Settings.Default.DecodeFLAC = decodeFLACToWAVToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void DecodeMP3ToWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!decodeMP3ToWAVToolStripMenuItem.Checked)
                decodeMP3ToWAVToolStripMenuItem.Checked = true;
            else
                decodeMP3ToWAVToolStripMenuItem.Checked = false;
            Properties.Settings.Default.DecodeMP3 = decodeMP3ToWAVToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void UseFfmpegForDecodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!useFfmpegForDecodingToolStripMenuItem.Checked)
                useFfmpegForDecodingToolStripMenuItem.Checked = true;
            else
                useFfmpegForDecodingToolStripMenuItem.Checked = false;
            Properties.Settings.Default.UseFFMPEG = useFfmpegForDecodingToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void DecodeM4AToWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!decodeM4AToWAVToolStripMenuItem.Checked)
                decodeM4AToWAVToolStripMenuItem.Checked = true;
            else
                decodeM4AToWAVToolStripMenuItem.Checked = false;
            Properties.Settings.Default.DecodeM4A = decodeM4AToWAVToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MediaRenderers.SelectedIndex >= -1)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Title = "Browse to save renderer information", Filter = "*.txt|*.txt" };
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false);
                    writer.Write(DLNA.SSDP.Renderers[MediaRenderers.SelectedIndex]);
                    writer.Close();
                    MessageBox.Show("Renderer information saved");

                }
            }

        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Title = "Browse for a renderer information file", Filter = "*.txt|*.txt" };
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFileDialog.FileName);
                DLNA.SSDP.Renderers.Add(reader.ReadToEnd());
                reader.Close();
            }
        }

        private void DecodeWMAToWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!decodeWMAToWAVToolStripMenuItem.Checked)
                decodeWMAToWAVToolStripMenuItem.Checked = true;
            else
                decodeWMAToWAVToolStripMenuItem.Checked = false;
            Properties.Settings.Default.DecodeWMA = decodeWMAToWAVToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void decodeToFLACInsteadOfWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!decodeToFLACInsteadOfWAVToolStripMenuItem.Checked)
                decodeToFLACInsteadOfWAVToolStripMenuItem.Checked = true;
            else
                decodeToFLACInsteadOfWAVToolStripMenuItem.Checked = false;
            Properties.Settings.Default.DecodeToFLAC = decodeToFLACInsteadOfWAVToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void MediaRenderers_SelectedIndexChanged(object sender, EventArgs e)
        {
            renderer = MediaRenderers.SelectedIndex;
        }
    }
}
