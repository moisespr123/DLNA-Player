using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class Extentions
{

    public static string ChopOffBefore(this string s, string Before)
    {//Usefull function for chopping up strings
        int End = s.ToUpper().IndexOf(Before.ToUpper());
        if (End > -1)
        {
            return s.Substring(End + Before.Length);
        }
        return s;
    }

    public static string ChopOffAfter(this string s, string After)
    {//Usefull function for chopping up strings
        int End = s.ToUpper().IndexOf(After.ToUpper());
        if (End > -1)
        {
            return s.Substring(0, End);
        }
        return s;
    }

    public static string ReplaceIgnoreCase(this string Source, string Pattern, string Replacement)
    {// using \\$ in the pattern will screw this regex up
        //return Regex.Replace(Source, Pattern, Replacement, RegexOptions.IgnoreCase);

        if (Regex.IsMatch(Source, Pattern, RegexOptions.IgnoreCase))
            Source = Regex.Replace(Source, Pattern, Replacement, RegexOptions.IgnoreCase);
        return Source;
    }

    private static void deleteTempFile(string path)
    {
        try
        {
            FileAttributes attrs = File.GetAttributes(path);
            if (attrs.HasFlag(FileAttributes.ReadOnly))
                File.SetAttributes(path, attrs & ~FileAttributes.ReadOnly);
            File.Delete(path);
        }
        catch
        {
            ProcessStartInfo decProcessInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/C del \"" + path +"\"",
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                UseShellExecute = false
            };
            Process.Start(decProcessInfo).WaitForExit();
        }
    }
    public static Task<MemoryStream> decodeAudio(string file, int format)
    {
        string dec = string.Empty;
        string args = string.Empty;
        string tempFilename = Path.GetTempFileName();
        File.Delete(tempFilename);
        if (DLNAPlayer.Properties.Settings.Default.UseFFMPEG)
        {
            dec = "ffmpeg.exe";
            if (DLNAPlayer.Properties.Settings.Default.DecodeToFLAC)
                tempFilename = tempFilename + ".flac";
            else
                tempFilename = tempFilename + ".wav";
            args = "-i \"" + file + "\" \"" + tempFilename + "\" - y";
        }
        else
        {
            tempFilename = tempFilename + ".wav";
            switch (format)
            {
                case 1:
                    dec = "opusdec.exe";
                    args = "--rate 48000 --no-dither --float \"" + file + "\" \"" + tempFilename + "\"";
                    break;
                case 2:
                    dec = "flac.exe";
                    args = "-d \"" + file + "\" -o \"" + tempFilename + "\"";
                    break;
                case 3:
                    dec = "ffmpeg.exe";
                    args = "-i \"" + file + "\" \"" + tempFilename + "\" - y";
                    break;
            }
        }
        ProcessStartInfo decProcessInfo = new ProcessStartInfo()
        {
            FileName = dec,
            Arguments = args,
            CreateNoWindow = true,
            RedirectStandardOutput = false,
            UseShellExecute = false
        };
        Process.Start(decProcessInfo).WaitForExit();
        MemoryStream decodedWav = new MemoryStream();
        bool decoded = false;
        while (!decoded)
        {
            try
            {
                FileStream temp = new FileStream(tempFilename, FileMode.Open, FileAccess.Read);
                temp.CopyTo(decodedWav);
                temp.Close();
                decoded = true;
            }
            catch
            {
                decoded = false;
            }
        }
        deleteTempFile(tempFilename);
        return Task.FromResult<MemoryStream>(decodedWav);
    }
    public static Task<string[]> getMetadata(string file)
    {
        string track = "Unknown";
        string artist = "Unknown";
        string performer = "Unknown";
        ProcessStartInfo ProcessInfo = new ProcessStartInfo()
        {
            FileName = "mediainfo.exe",
            Arguments = "\"" + file + "\"",
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            UseShellExecute = false
        };
        Process process = new Process
        {
            StartInfo = ProcessInfo
        };
        process.Start();
        string line = string.Empty;
        while (true)
        {
            if (!process.StandardOutput.EndOfStream)
            {
                string[] splitted_line = process.StandardOutput.ReadLine().Split(':');
                if (splitted_line[0].Contains("Track name") && !splitted_line[0].Contains("/"))
                    track = splitted_line[1].Trim();
                else if (splitted_line[0].Contains("Artist") && !splitted_line[0].Contains("/"))
                    artist = splitted_line[1].Trim();
                else if (splitted_line[0].Contains("Performer") && !splitted_line[0].Contains("/"))
                    performer = splitted_line[1].Trim();
            }
            else
                break;
        }
        if (artist == "Unknown" && performer != "Unknown")
            artist = performer;
        string[] returnString = { track, artist };
        return Task.FromResult<string[]>(returnString);
    }
    public static class Helper
    {
        public static string GetMyIP()
        {//Might return the wrong NC ip but you need the one connected to the router
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            string ipAddress = "";
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipAddress = ip.ToString();
                }
            }
            return ipAddress;

        }
    }
}


