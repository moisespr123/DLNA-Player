//Dr Gadgit from the Code project http://www.codeproject.com/Articles/893791/DLNA-made-easy-and-Play-To-for-any-device
using DLNAPlayer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace DLNA
{
    //This class is used to broadcast a SSDP message using UDP on port 1900 and to then wait for any replies send back on the LAN
    public static class SSDP
    {
        public static bool Run = true;
        public static List<String> Renderers;
        public static void Start()
        {
            if (Run)
            {
                Renderers = new List<string> { };
                SendRequest();
            }
        }
        public static void Stop()
        {
            Run = false;
        }

        private static void SendRequest()
        {//Uses UDP Multicast on 239.255.255.250 with port 1900 to send out invitations that are slow to be answered
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    try
                    {
                        IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Parse(ip.ToString()), 6000);
                        IPEndPoint MulticastEndPoint = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);//SSDP port
                        Socket UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        UdpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        UdpSocket.Bind(LocalEndPoint);
                        UdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(MulticastEndPoint.Address, LocalEndPoint.Address));
                        UdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
                        UdpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);
                        string SearchString = "M-SEARCH * HTTP/1.1\r\nHOST:239.255.255.250:1900\r\nMAN:\"ssdp:discover\"\r\nST:ssdp:all\r\nMX:3\r\n\r\n";
                        UdpSocket.SendTo(Encoding.UTF8.GetBytes(SearchString), SocketFlags.None, MulticastEndPoint);
                        byte[] ReceiveBuffer = new byte[4000];
                        int ReceivedBytes = 0;
                        for (int i = 0; i < 100; i++)
                        {
                            if (UdpSocket.Available > 0)
                            {
                                ReceivedBytes = UdpSocket.Receive(ReceiveBuffer, SocketFlags.None);
                                if (ReceivedBytes > 0)
                                {
                                    string Data = Encoding.UTF8.GetString(ReceiveBuffer, 0, ReceivedBytes);
                                    if (Data.ToUpper().IndexOf("LOCATION: ") > -1)
                                    {//ChopOffAfter is an extended string method added in Helper.cs
                                        Data = Data.ChopOffBefore("LOCATION: ").ChopOffAfter(Environment.NewLine);
                                        if (!Renderers.Contains(Data.ToLower()))
                                            Renderers.Add(Data.ToLower());
                                    }
                                }
                            }
                            else
                                Thread.Sleep(10);
                        }
                        UdpSocket.Close();
                    }
                    catch { }
                }
            }
        }
    }
}

