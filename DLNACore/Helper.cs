using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Diagnostics;

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
    public static MemoryStream decodeOpus(string file)
    {
        ProcessStartInfo opusProcessInfo = new ProcessStartInfo()
        {
            FileName = "opusdec.exe",
            Arguments = "--rate 48000 --no-dither --float \"" + file + "\" temp.wav",
            CreateNoWindow = true,
            RedirectStandardOutput = false,
            UseShellExecute = false
        };
        Process opusProcess = Process.Start(opusProcessInfo);
        opusProcess.WaitForExit();
        FileStream tempFile = new FileStream("temp.wav", FileMode.Open);
        MemoryStream decodedWav = new MemoryStream();
        tempFile.CopyTo(decodedWav);
        tempFile.Close();
        File.Delete("temp.wav");
        return decodedWav;
    }
    public static MemoryStream decodeFlac(string file)
    {
        ProcessStartInfo flacProcessInfo = new ProcessStartInfo()
        {
            FileName = "flac.exe",
            Arguments = "-d \"" + file + "\" -o temp.wav",
            CreateNoWindow = true,
            RedirectStandardOutput = false,
            UseShellExecute = false
        };
        Process flacProcess = Process.Start(flacProcessInfo);
        flacProcess.WaitForExit();
        FileStream tempFile = new FileStream("temp.wav", FileMode.Open);
        MemoryStream decodedWav = new MemoryStream();
        tempFile.CopyTo(decodedWav);
        tempFile.Close();
        File.Delete("temp.wav");
        return decodedWav;
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


