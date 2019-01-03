//Dr Gadgit from the Code project http://www.codeproject.com/Articles/893791/DLNA-made-easy-and-Play-To-for-any-device
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace DLNA
{
    #region HelperDNL
    public static class HelperDLNA
    {
        public static string MakeRequest(string Methord, string Url, int ContentLength, string SOAPAction, string IP, int Port)
        {//Make a request that is sent out to the DLNA server on the LAN using TCP
            string R = Methord.ToUpper() + " /" + Url + " HTTP/1.1" + Environment.NewLine;
            R += "Cache-Control: no-cache" + Environment.NewLine;
            R += "Connection: Close" + Environment.NewLine;
            R += "Pragma: no-cache" + Environment.NewLine;
            R += "Host: " + IP + ":" + Port + Environment.NewLine;
            R += "User-Agent: Microsoft-Windows/6.3 UPnP/1.0 Microsoft-DLNA DLNADOC/1.50" + Environment.NewLine;
            R += "FriendlyName.DLNA.ORG: " + Environment.MachineName + Environment.NewLine;
            if (ContentLength > 0)
            {
                R += "Content-Length: " + ContentLength + Environment.NewLine;
                R += "Content-Type: text/xml; charset=\"utf-8\"" + Environment.NewLine;
            }
            if (SOAPAction.Length > 0)
                R += "SOAPAction: \"" + SOAPAction + "\"" + Environment.NewLine;
            return R + Environment.NewLine;
        }

        public static Socket MakeSocket(string ip, int port)
        {//Just returns a TCP socket ready to use
            IPEndPoint IPWeb = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket SocWeb = new Socket(IPWeb.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = 6000,
            };
            try
            {
                SocWeb.Connect(IPWeb);
                return SocWeb;
            }
            catch
            {
                return null;
            }
        }

        public static string ReadSocket(Socket Soc, bool CloseOnExit, ref int ReturnCode)
        {//We have some data to read on the socket 
            ReturnCode = 0;
            int ContentLength = 0;
            int HeadLength = 0;
            Thread.Sleep(20);
            MemoryStream MS = new MemoryStream();
            byte[] buffer = new byte[8000];
            int Count = 0;
            while (Count < 8)
            {
                Count++;
                if (Soc.Available > 0)
                {
                    int Size = Soc.Receive(buffer);
                    string Head = Encoding.UTF32.GetString(buffer).ToLower();
                    if (ContentLength == 0 && Head.IndexOf(Environment.NewLine + Environment.NewLine) > -1 && Head.IndexOf("content-length:") > -1)
                    {//We have a contant length so we can test if we have all the page data.
                        HeadLength = Head.LastIndexOf(Environment.NewLine + Environment.NewLine);
                        string StrCL = Head.ChopOffBefore("content-length:").ChopOffAfter(Environment.NewLine);
                        int.TryParse(StrCL, out ContentLength);
                    }
                    MS.Write(buffer, 0, Size);
                    if (ContentLength > 0 && MS.Length >= HeadLength + ContentLength)
                    {
                        if (CloseOnExit) Soc.Close();
                        return Encoding.UTF8.GetString(MS.ToArray());
                    }
                }
            }
            if (CloseOnExit) Soc.Close();
            string HTML = Encoding.UTF8.GetString(MS.ToArray());
            string Code = HTML.ChopOffBefore("HTTP/1.1").Trim().ChopOffAfter(" ");
            int.TryParse(Code, out ReturnCode);
            return HTML;
        }
    }
    #endregion
    #region DLNAService
    public class DLNAService
    {//Each DLNA server might offer several services so this class makes them easyer to read but the one we are looking for to use is AVTransport
        public string controlURL = "";
        public string Scpdurl = "";
        public string EventSubURL = "";
        public string ServiceType = "";
        public string ServiceID = "";
        public DLNAService(string HTML)
        {
            HTML = HTML.ChopOffBefore("<service>").Replace("url>/", "url>").Trim();
            HTML = HTML.Replace("URL>/", "URL>");
            if (HTML.ToLower().IndexOf("<servicetype>") > -1)
                ServiceType = HTML.ChopOffBefore("<servicetype>").ChopOffAfter("</servicetype>").Trim();
            if (HTML.ToLower().IndexOf("<serviceid>") > -1)
                ServiceID = HTML.ChopOffBefore("<serviceid>").ChopOffAfter("</serviceid>").Trim();
            if (HTML.ToLower().IndexOf("<controlurl>") > -1)
                controlURL = HTML.ChopOffBefore("<controlurl>").ChopOffAfter("</controlurl>").Trim();
            if (HTML.ToLower().IndexOf("<scpdurl>") > -1)
                Scpdurl = HTML.ChopOffBefore("<scpdurl>").ChopOffAfter("</scpdurl>").Trim();
            if (HTML.ToLower().IndexOf("<eventsuburl>") > -1)
                EventSubURL = HTML.ChopOffBefore("<eventsuburl>").ChopOffAfter("</eventsuburl>").Trim();
        }

        public static Dictionary<string, DLNAService> ReadServices(string HTML)
        {
            Dictionary<string, DLNAService> Dic = new Dictionary<string, DLNAService>();
            HTML = HTML.ChopOffBefore("<serviceList>").ChopOffAfter("</serviceList>").Replace("</service>", "¬");
            foreach (string Line in HTML.Split('¬'))
            {
                if (Line.Length > 20)
                {
                    DLNAService S = new DLNAService(Line);
                    Dic.Add(S.ServiceID, S);
                }
            }
            return Dic;
        }
    }
    #endregion

    public class DLNADevice
    {
        private Dictionary<int, string> PlayListQueue = new Dictionary<int, string>();
        public string ControlURL = "";
        public bool Connected = false;
        public int ReturnCode = 0;
        public int Port = 0;
        public string IP = "";
        public string Location = "";
        public string Server = "";
        public string USN = "";
        public string ST = "";
        public string SMP = "";
        public string HTML = "";
        public string FriendlyName = "";
        public Dictionary<string, DLNAService> Services = null;


        public bool IsConnected()
        {//Will send a request to the DLNA server and then see if we get a valid reply
            Connected = false;
            try
            {
                Socket SocWeb = HelperDLNA.MakeSocket(this.IP, this.Port);
                SocWeb.Send(Encoding.UTF8.GetBytes(HelperDLNA.MakeRequest("GET", this.SMP, 0, "", this.IP, this.Port)), SocketFlags.None);
                this.HTML = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                if (this.ReturnCode != 200) return false;
                this.Services = DLNAService.ReadServices(HTML);
                if (this.HTML.ToLower().IndexOf("<friendlyname>") > -1)
                    this.FriendlyName = this.HTML.ChopOffBefore("<friendlyName>").ChopOffAfter("</friendlyName>").Trim();
                foreach (DLNAService S in this.Services.Values)
                {
                    if (S.ServiceType.ToLower().IndexOf("avtransport:1") > -1)//avtransport is the one we will be using to control the device
                    {
                        this.ControlURL = S.controlURL;
                        this.Connected = true;
                        return true;
                    }
                }
            }
            catch {; }
            return false;
        }


        public string TryToPlayFile(string UrlToPlay, string[] info = null)
        {
            if (!this.Connected) this.Connected = this.IsConnected();//Someone might have turned the TV Off !
            if (!this.Connected) return "#ERROR# Not connected";
            try
            {
                foreach (DLNAService S in this.Services.Values)
                {
                    if (S.ServiceType.ToLower().IndexOf("avtransport:1") > -1)
                    {//This is the service we are using so upload the file and then start playing
                        string AddPlay = UploadFileToPlay(S.controlURL, UrlToPlay, info);
                        if (this.ReturnCode != 200) return AddPlay;
                        string PlayNow = StartPlay(S.controlURL, 0);
                        if (this.ReturnCode == 200) return "OK"; else return AddPlay;
                    }
                }
                return "#ERROR# Could not find avtransport:1";
            }
            catch (Exception Ex) { return "#ERROR# " + Ex.Message; }

        }

        private readonly string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<s:Body>" + Environment.NewLine;
        private readonly string XMLFoot = "</s:Body>" + Environment.NewLine + "</s:Envelope>" + Environment.NewLine;
        public string GetPosition()
        {//Returns the current position for the track that is playing on the DLNA server
            return GetPosition(this.ControlURL);
        }
        private string GetPosition(string ControlURL)
        {//Returns the current position for the track that is playing on the DLNA server
            string XML = XMLHead + "<m:GetPositionInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">0</InstanceID></m:GetPositionInfo>" + XMLFoot + Environment.NewLine;
            Socket SocWeb = HelperDLNA.MakeSocket(this.IP, this.Port);
            string Request = HelperDLNA.MakeRequest("POST", ControlURL, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetPositionInfo", this.IP, this.Port) + XML;
            if (SocWeb != null)
            {
                SocWeb.Send(Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
            }
            else
                return "";
        }

        public string GetTransportInfo()
        {//Returns the current position for the track that is playing on the DLNA server
            return GetTransportInfo(this.ControlURL);
        }

        private string GetTransportInfo(string ControlURL)
        {//Returns the current position for the track that is playing on the DLNA server
            string XML = XMLHead + "<m:GetTransportInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">0</InstanceID></m:GetTransportInfo>" + XMLFoot + Environment.NewLine;
            Socket SocWeb = HelperDLNA.MakeSocket(this.IP, this.Port);
            string Request = HelperDLNA.MakeRequest("POST", ControlURL, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetTransportInfo", this.IP, this.Port) + XML;
            if (SocWeb != null)
            {
                SocWeb.Send(Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
            }
            else
                return "";
        }

        public string Desc(string Url, string[] info)
        {//Gets a description of the DLNA server
            string XML = "<DIDL-Lite xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:upnp=\"urn:schemas-upnp-org:metadata-1-0/upnp/\" xmlns:r=\"urn:schemas-rinconnetworks-com:metadata-1-0/\" xmlns=\"urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/\">" + Environment.NewLine;
            XML += "<item>";
            XML += "<dc:title>" + info[0] + "</dc:title>";
            XML += "<dc:creator>" + info[1] + "</dc:creator>";
            XML += "<upnp:artist>" + info[1] + "</upnp:artist>";
            XML += "<upnp:class>object.item.audioItem.musicTrack</upnp:class>";
            XML += "<res>" + Url + "</res>";
            XML += "</item>";
            XML += "</DIDL-Lite>";
            return WebUtility.HtmlEncode(XML);
        }

        public string StartPlay(int Instance)
        {//Start playing the new upload film or music track 
            if (!this.Connected) this.Connected = this.IsConnected();
            if (!this.Connected) return "#ERROR# Not connected";
            return StartPlay(this.ControlURL, Instance);
        }

        private string StartPlay(string ControlURL, int Instance)
        {//Start playing the new upload film or music track 
            string XML = XMLHead;
            XML += "<u:Play xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + Instance + "</InstanceID><Speed>1</Speed></u:Play>" + Environment.NewLine;
            XML += XMLFoot + Environment.NewLine;
            Socket SocWeb = HelperDLNA.MakeSocket(this.IP, this.Port);
            string Request = HelperDLNA.MakeRequest("POST", ControlURL, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Play", this.IP, this.Port) + XML;
            SocWeb.Send(Encoding.UTF8.GetBytes(Request), SocketFlags.None);
            return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
        }

        public string StopPlay(bool ClearQueue = false)
        {//If we are playing music tracks and not just a movie then clear our queue of tracks
            if (!this.Connected) this.Connected = this.IsConnected();
            if (!this.Connected) return "#ERROR# Not connected";
            if (ClearQueue)
            {
                this.PlayListQueue = new Dictionary<int, string>();
            }
            return StopPlay(this.ControlURL, 0);
        }


        private string StopPlay(string ControlURL, int Instance)
        {//Called to stop playing a movie or a music track
            string XML = XMLHead;
            XML += "<u:Stop xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + Instance + "</InstanceID></u:Stop>" + Environment.NewLine;
            XML += XMLFoot + Environment.NewLine;
            Socket SocWeb = HelperDLNA.MakeSocket(this.IP, this.Port);
            string Request = HelperDLNA.MakeRequest("POST", ControlURL, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Stop", this.IP, this.Port) + XML;
            SocWeb.Send(Encoding.UTF8.GetBytes(Request), SocketFlags.None);
            return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
        }

        public string Seek(string position)
        {
            if (!this.Connected) this.Connected = this.IsConnected();
            if (!this.Connected) return "#ERROR# Not connected";
            return Seek(this.ControlURL, 0, position);
        }
        private string Seek(string ControlURL, int Instance, string position)
        {//Called to stop playing a movie or a music track
            string XML = XMLHead;
            XML += "<u:Seek xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + Instance + "</InstanceID><Unit>REL_TIME</Unit><Target>" + position + "</Target></u:Seek>" + Environment.NewLine;
            XML += XMLFoot + Environment.NewLine;
            Socket SocWeb = HelperDLNA.MakeSocket(this.IP, this.Port);
            string Request = HelperDLNA.MakeRequest("POST", ControlURL, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Seek", this.IP, this.Port) + XML;
            SocWeb.Send(Encoding.UTF8.GetBytes(Request), SocketFlags.None);
            return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
        }
        public string Pause()
        {//If we are playing music tracks and not just a movie then clear our queue of tracks
            if (!this.Connected) this.Connected = this.IsConnected();
            if (!this.Connected) return "#ERROR# Not connected";
            return Pause(this.ControlURL, 0);
        }
        private string Pause(string ControlURL, int Instance)
        {//Called to pause playing a movie or a music track
            string XML = XMLHead;
            XML += "<u:Pause xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + Instance + "</InstanceID></u:Pause>" + Environment.NewLine;
            XML += XMLFoot + Environment.NewLine;
            Socket SocWeb = HelperDLNA.MakeSocket(this.IP, this.Port);
            string Request = HelperDLNA.MakeRequest("POST", ControlURL, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Pause", this.IP, this.Port) + XML;
            SocWeb.Send(Encoding.UTF8.GetBytes(Request), SocketFlags.None);
            return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
        }



        //public int PlayPreviousQueue()
        //{//Play the previous track in our queue, we don't care if the current track has not completed or not, just do it
        //    PlayListPointer--;
        //    if (PlayListQueue.Count == 0) return 0;
        //    if (PlayListPointer == 0)
        //        PlayListPointer = PlayListQueue.Count;
        //    string Url = PlayListQueue[PlayListPointer];
        //    StopPlay(false);
        //    TryToPlayFile(Url);
        //    return 310;
        //}

        //private int NoPlayCount = 0;
        //public int PlayNextQueue(bool Force)
        //{//Play the next track in our queue but only if the current track is about to end or unless we are being forced  
        //    if (Force)
        //    {//Looks like someone has pressed the next track button
        //        PlayListPointer++;
        //        if (PlayListQueue.Count == 0) return 0;
        //        if (PlayListPointer > PlayListQueue.Count)
        //            PlayListPointer = 1;
        //        string Url = PlayListQueue[PlayListPointer];
        //        StopPlay(false);
        //        TryToPlayFile(Url);//Just play it
        //        NoPlayCount = 0;
        //        return 310;//Just guess for now how long the track is
        //    }
        //    else
        //    {
        //        string HTMLPosition = GetPosition();
        //        if (HTMLPosition.Length < 50) return 0;
        //        string TrackDuration = HTMLPosition.ChopOffBefore("<TrackDuration>").ChopOffAfter("</TrackDuration>").Substring(2);
        //        string RelTime = HTMLPosition.ChopOffBefore("<RelTime>").ChopOffAfter("</RelTime>").Substring(2);
        //        int RTime = TotalSeconds(RelTime);
        //        int TTime = TotalSeconds(TrackDuration);
        //        if (RTime < 3 || TTime < 2)
        //        {
        //            NoPlayCount++;
        //            if (NoPlayCount > 3)
        //            {
        //                StopPlay(false);
        //                return PlayNextQueue(true);//Force the next track to start because the current track is about to end
        //            }
        //            else
        //                return 0;

        //        }
        //        int SecondsToPlay = TTime - RTime - 5;
        //        if (SecondsToPlay < 0) SecondsToPlay = 0;//Just a safeguard
        //        if (SecondsToPlay <10)
        //        {//Current track is about to end so wait a few seconds and then force the next track in our queue to play
        //            Thread.Sleep((SecondsToPlay * 1000) +100);
        //            return PlayNextQueue(true);
        //        }
        //        return SecondsToPlay;//Will have to wait to be polled again before playing the next track in our queue
        //    }
        //}

        private int TotalSeconds(string Value)
        {//Convert the time left for the track to play back to seconds
            try
            {
                Value = Value.ChopOffAfter(".");
                int Mins = int.Parse(Value.Split(':')[0]);
                int Secs = int.Parse(Value.Split(':')[1]);
                return Mins * 60 + Secs;
            }
            catch {; }
            return 0;
        }

        //public bool AddToQueue(string UrlToPlay, ref bool NewTrackPlaying)
        //{//We add music tracks to a play list queue and then we poll the server so we know when to send the next track in the queue to play
        //    if (!this.Connected) this.Connected = this.IsConnected();
        //    if (!this.Connected) return false;
        //    foreach (string Url in PlayListQueue.Values)
        //    {
        //        if (Url.ToLower() == UrlToPlay.ToLower())
        //            return false;
        //    }
        //    PlayListQueue.Add(PlayListQueue.Count + 1, UrlToPlay);
        //    if (!NewTrackPlaying)
        //    {
        //        PlayListPointer = PlayListQueue.Count + 1;
        //        StopPlay(false);
        //        TryToPlayFile(UrlToPlay);
        //        NewTrackPlaying = true;
        //    }
        //    return false;
        //}

        //private string NextPlayList(string ControlURL, string UrlToPlay, int Instance)
        //{//Yes  this would be nice but it does not queue the track up and that is why we use our own queue and then poll the DLNA server to know when to play the next track
        //    string XML = XMLHead;
        //    XML += "<u:SetNextAVTransportURI xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">" + Environment.NewLine;
        //    XML += "<InstanceID>" + Instance + "</InstanceID>" + Environment.NewLine;
        //    XML += "<NextURI>" + UrlToPlay.Replace(" ", "%20") + "</NextURI>" + Environment.NewLine;
        //    XML += "<NextURIMetaData>" + Desc() + "</NextURIMetaData>" + Environment.NewLine;
        //    XML += "</u:SetNextAVTransportURI>" + Environment.NewLine;
        //    XML += XMLFoot + Environment.NewLine;
        //    Socket SocWeb = HelperDLNA.MakeSocket(this.IP, this.Port);
        //    string Request = HelperDLNA.MakeRequest("POST", ControlURL, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#SetNextAVTransportURI", this.IP, this.Port) + XML;
        //    SocWeb.Send(Encoding.UTF8.GetBytes(Request), SocketFlags.None);
        //    string HTML = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
        //    return HTML;
        //}

        private string UploadFileToPlay(string ControlURL, string UrlToPlay, string[] info = null)
        {///Later we will send a message to the DLNA server to start the file playing
            string XML = XMLHead;
            XML += "<u:SetAVTransportURI xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">" + Environment.NewLine;
            XML += "<InstanceID>0</InstanceID>" + Environment.NewLine;
            XML += "<CurrentURI>" + encodeUrl(UrlToPlay) + "</CurrentURI>" + Environment.NewLine;
            if (info != null)
                XML += "<CurrentURIMetaData>" + Desc(UrlToPlay, info) + "</CurrentURIMetaData>" + Environment.NewLine;
            else
                XML += "<CurrentURIMetaData></CurrentURIMetaData>" + Environment.NewLine;
            XML += "</u:SetAVTransportURI>" + Environment.NewLine;
            XML += XMLFoot + Environment.NewLine;
            Socket SocWeb = HelperDLNA.MakeSocket(this.IP, this.Port);
            string Request = HelperDLNA.MakeRequest("POST", ControlURL, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#SetAVTransportURI", this.IP, this.Port) + XML;
            SocWeb.Send(Encoding.UTF8.GetBytes(Request), SocketFlags.None);
            return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
        }
        private string encodeUrl(string Url)
        {
            return WebUtility.HtmlEncode(Url);
        }
        public DLNADevice(string url)
        {
            this.IP = url.ChopOffBefore("http://").ChopOffAfter(":");
            this.SMP = url.ChopOffBefore(this.IP).ChopOffBefore("/");
            string StrPort = url.ChopOffBefore(this.IP).ChopOffBefore(":").ChopOffAfter("/");
            int.TryParse(StrPort, out this.Port);
        }

        public DLNADevice(string ip, int port, string smp)
        {//Constructor
            this.IP = ip;
            this.Port = port;
            this.SMP = smp;
        }
    }
}
