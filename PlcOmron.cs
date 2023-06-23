using PLC_Omron_Standard.Interfaces;
using PLC_Omron_Standard.Tools;
using System.Net;

namespace PLC_Omron_Standard
{
    /// <summary>
    /// Main class to communicate with Omron PLCs
    /// </summary>
    public class PlcOmron
    {
        private readonly IConnection Connection;
        private readonly ICommand Command;

        public PlcOmron(string ip, int port, bool useTcp = true) 
        {
            var address = IPAddress.Parse(ip);

            if (useTcp)
            {
                Connection = new TcpConnection(address, port);
                Command = new TcpCommand(Connection);
            }
            else
            {
                Connection = new UdpConnection(address, port);
                Command = new UdpCommand(Connection);
            }
        }

        public PlcOmron(IPAddress ip, int port, bool useTcp = true)
        {
            if (useTcp)
            {
                Connection = new TcpConnection(ip, port);
                Command = new TcpCommand(Connection);
            }
            else
            {
                Connection = new UdpConnection(ip, port);
                Command = new UdpCommand(Connection);
            }
        }
    }
}
