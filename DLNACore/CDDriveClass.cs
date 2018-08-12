using Ripper;
using System;
using System.Collections.Generic;
using System.IO;
using WaveLib;
using Yeti.MMedia;

namespace DLNAPlayer
{
    public class AudioCD
    {
        private Ripper.CDDrive drive;
        public List<string> DriveList = new List<string> { };
        public List<int> AudioTracks = new List<int> { };
        public AudioCD()
        {
            drive = new CDDrive();
            char[] Drives = CDDrive.GetCDDriveLetters();
            foreach (char drive in Drives)
            {
                DriveList.Add(drive.ToString());
            }
        }
        public bool ready(int index)
        {
            drive.Open(DriveList[index][0]);
            if (drive.IsCDReady())
            {
                LoadTracks();
            }
            return drive.IsCDReady();
        }
        public void LoadTracks()
        {
            if (drive.IsCDReady())
            {
                drive.Refresh();
                int Tracks = drive.GetNumTracks();
                for (int i = 1; i <= Tracks; i++)
                {
                    if (drive.IsAudioTrack(i))
                        AudioTracks.Add(i);
                }
            }
        }
        WaveWriter writer = null;
        public MemoryStream getTrack(string track)
        {
            WaveFormat Format = new WaveFormat(44100, 16, 2);
            MemoryStream WaveFile = new MemoryStream();
            writer = new WaveWriter(WaveFile, Format);
            drive.ReadTrack(Convert.ToInt32(track), new CdDataReadEventHandler(WriteWaveData), null);
            writer.Close();
            //We recreate the MemoryStream because the above line closes the previous MemoryStream as well as the WaveWriter.
            WaveFile = new MemoryStream(WaveFile.ToArray());
            return WaveFile;
        }
        public void WriteWaveData(object sender, DataReadEventArgs ea)
        {
            if (writer != null)
            {
                writer.Write(ea.Data, 0, (int)ea.DataSize);
            }
        }
    }
}