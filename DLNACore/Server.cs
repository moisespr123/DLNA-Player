using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
//Dr Gadgit from the code project http://www.codeproject.com/Articles/1079847/DLNA-Media-Server-to-feed-Smart-TVs
//This little gem will server up your media collection to DLNA devices like your TV and has been tested and woks with
//VLC-Media player, Samsung, Sony ,LG smart TV's and will ever service requests for album-artwork .png/jpg images
//Just call Start() to start the service and Stop() to stop it, jobs a good one
namespace DLNAPlayer
{
    public class MediaServer
    {
        public bool Running = false;//Flag set to true when running and false to kill the service
        public string IP = null;//The ip of this service we will listen on for DLNA requests
        public int Port = 9090;//The post we will listen on for incoming DLNA requests 
        public MemoryStream FS = null;
        private Socket SocServer = null;
        private Thread TH = null;
        private long TempStartRange = 0; //Past to the client thread ready to service the request
        private long TempEndRange = 0; //Past to the client thread ready to service the request
        private Socket TempClient = null;  //Past to the client thread ready to service the request
        public string Filename = "";  //Past to the client thread ready to service the request

        public MediaServer(string ip, int port)
        {//Our Contructor
            this.IP = ip;
            this.Port = port;
        }

        public void Start()
        {//Starts our DLNA service
            Running = true;
            TH = new Thread(Listen);
            TH.Start();
        }

        public void Stop()
        {//Stops our DLNA service
            this.Running = false;
            if (this.FS != null)
            { try { FS.Close(); } catch {; } }
            if (SocServer != null && SocServer.Connected) SocServer.Shutdown(SocketShutdown.Both);
            TH.Abort();
        }

       

        private bool IsMusicOrImage(string FileName)
        {//We don't want to use byte-ranges for music or image data so we test the filename here
            if (FileName.ToLower().EndsWith(".jpg") || FileName.ToLower().EndsWith(".png") || FileName.ToLower().EndsWith(".gif") || FileName.ToLower().EndsWith(".mp3"))
                return true;
            return false;
        }

        private string GMTTime(DateTime Time)
        {//Covert date to GMT time/date
            string GMT = Time.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'");
            return GMT;//Example "Sat, 25 Jan 2014 12:03:19 GMT";
        }

        public string MakeBaseUrl(string DirectoryName)
        {//Helper function to make the base url thats past to the DNLA device so that it can talk to this media service
            string Url = "http://" + this.IP + ":" + this.Port + "/";
            if (Url.EndsWith("//")) return Url.Substring(0, Url.Length - 1);
            return Url;
        }//Returns something like http://192.168.0.10:9090/Action%20Films/

        private string EncodeUrl(string Value)
        {//Encode requests sent to the DLNA device
            if (Value == null) return null;
            return Value.Replace(" ", "%20").Replace("&", "%26").Replace("'", "%27").Replace("\\", "/");
        }

        private string DecodeUrl(string Value)
        {//Decode request from the DLNA device
            if (Value == null) return null;
            return Value.Replace("%20", " ").Replace("%26", "&").Replace("%27", "'").Replace("/", "\\");
        }

        private string ContentString(long StartRange, long EndRange, string ContentType, long FileLength)
        {//Builds up our HTTP reply string for byte-range requests
            string Reply = "";
            Reply = "HTTP/1.1 206 Partial Content" + Environment.NewLine + "Server: DLNAPlayer" + Environment.NewLine + "Content-Type: " + ContentType + Environment.NewLine;
            Reply += "Accept-Ranges: bytes" + Environment.NewLine;
            Reply += "Date: " + GMTTime(DateTime.Now) + Environment.NewLine;
            if (StartRange == 0)
            {
                Reply += "Content-Length: " + FileLength + Environment.NewLine;
                Reply += "Content-Range: bytes 0-" + (FileLength - 1) + "/" + FileLength + Environment.NewLine;
            }
            else
            {
                Reply += "Content-Length: " + (EndRange - StartRange) + Environment.NewLine;
                Reply += "Content-Range: bytes " + StartRange + "-" + EndRange + "/" + FileLength + Environment.NewLine;
            }
            return Reply + Environment.NewLine;
        }

        private string SendHeadData()
        {//This runs in the same thread as the service since it should be nice and fast
            string ContentType = GetContentType("");
            string Reply = "HTTP/1.1 200 OK" + Environment.NewLine + "Server: DLNAPlayer" + Environment.NewLine + "Content-Type: " + ContentType + Environment.NewLine;
            Reply += "Date: " + GMTTime(DateTime.Now) + Environment.NewLine;
            Reply += "Accept-Ranges: bytes" + Environment.NewLine;//We only do ranges for movies
            Reply += "Content-Length: " + FS.Length + Environment.NewLine;
            //Soc.Send(UTF8Encoding.UTF8.GetBytes(Reply), SocketFlags.None);
            //Soc.Close();
            //this.TempClient = null;
            return Reply + Environment.NewLine;
        }
        private void Listen()
        {//This is the main service that waits for bew incoming request and then service the requests on another thread in most cases
            try
            {
                SocServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint IPE = new IPEndPoint(IPAddress.Parse(this.IP), this.Port);
                SocServer.Bind(IPE);
                while (this.Running)
                {
                    SocServer.Listen(0);
                    TempClient = SocServer.Accept();
                    if (TempClient == null)
                        return;
                    byte[] Buf = new byte[16384];
                    bool succeeded = false;
                    int Size = 0;
                    if (FS != null)
                    {
                        while (!succeeded)
                        {
                            try
                            {
                                if (TempClient == null)
                                    return;
                                Size = TempClient.Receive(Buf, SocketFlags.None);
                                succeeded = true;
                            }
                            catch
                            {
                                succeeded = false;
                            }
                        }
                        MemoryStream MS = new MemoryStream();
                        MS.Write(Buf, 0, Size);
                        string Request = UTF8Encoding.UTF8.GetString(MS.ToArray());
                        if (Request.ToUpper().StartsWith("GET /") && Request.ToUpper().IndexOf("HTTP/1.") > -1)
                        {
                            try
                            {
                                if (Request.ToLower().IndexOf("range: ") > -1)
                                {
                                    string[] Range = Request.ToLower().ChopOffBefore("range: ").ChopOffAfter(Environment.NewLine).Replace("bytes=", "").Split('-');
                                    if (!String.IsNullOrEmpty(Range[0]))
                                        long.TryParse(Range[0], out TempStartRange);
                                    else
                                        TempStartRange = 0;
                                    if (!String.IsNullOrEmpty(Range[1]))
                                        long.TryParse(Range[1], out TempEndRange);
                                    else
                                        TempEndRange = FS.Length;
                                }
                                else
                                {
                                    TempStartRange = 0;

                                    TempEndRange = FS.Length;
                                }
                                Thread THStream = new Thread(StreamFile);
                                THStream.Start();
                            }
                            catch { }
                        }
                        else
                            TempClient.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The DLNA Server could not start. This usually means the port is in use. Try changing ports and press the \"Apply\" button." + Environment.NewLine + Environment.NewLine + "The error is: " + ex.ToString());
            }
        }
        private string GetContentType(string FileName)
        {//Based on the file type we create our content type for the reply to the TV/DLNA device
            string ContentType = "audio/flac";
            if (FileName.ToLower().EndsWith(".jpg")) ContentType = "image/jpg";
            else if (FileName.ToLower().EndsWith(".png")) ContentType = "image/png";
            else if (FileName.ToLower().EndsWith(".gif")) ContentType = "image/gif";
            else if (FileName.ToLower().EndsWith(".avi")) ContentType = "video/avi";
            else if (FileName.ToLower().EndsWith(".mp4")) ContentType = "video/mp4";
            else if (FileName.ToLower().EndsWith(".m4a")) ContentType = "audio/mp4";
            else if (FileName.ToLower().EndsWith(".flac")) ContentType = "audio/flac";
            else if (FileName.ToLower().EndsWith(".wav")) ContentType = "audio/wav";
            else if (FileName.ToLower().EndsWith(".ogg") || FileName.ToLower().EndsWith(".opus")) ContentType = "audio/ogg";
            else if (FileName.ToLower().EndsWith(".mp3")) ContentType = "audio/mpeg";
            return ContentType;
        }

        private void StreamFile()
        {//Streams using ranges and runs on it's own thread
            try
            {
                long ByteToSend = 1;
                string ContentType = GetContentType(Filename);
                Socket Client = this.TempClient;
                this.TempClient = null;
                string Reply = null;
                //if (TempEndRange == FS.Length)
                //{
                //    Reply = SendHeadData();
                //}
                //else{
                Reply = ContentString(TempStartRange, TempEndRange, ContentType, FS.Length);
                //}
                
                Client.Send(UTF8Encoding.UTF8.GetBytes(Reply), SocketFlags.None);
                FS.Seek(TempStartRange, SeekOrigin.Begin);
                ByteToSend = TempEndRange - TempStartRange;
                if (ByteToSend < 0) return;
                byte[] Buf = new byte[ByteToSend];
                FS.Read(Buf, 0, Buf.Length);
                Client.Send(Buf);
                Client.Close();
            }
            catch { }
        }
    }
}
