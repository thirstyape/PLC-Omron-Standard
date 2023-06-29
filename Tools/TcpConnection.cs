using PLC_Omron_Standard.Enums;
using PLC_Omron_Standard.Interfaces;
using PLC_Omron_Standard.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PLC_Omron_Standard.Tools
{
    /// <summary>
    /// Implementation of <see cref="IConnection"/> to facilitate TCP based connections
    /// </summary>
    internal class TcpConnection : IConnection
    {
        private const int CommandTimeoutMs = 2_000;
        private readonly Socket Socket;
        private readonly IPEndPoint Endpoint;

        public TcpConnection(IPAddress ip, int port)
        {
            Endpoint = new IPEndPoint(ip, port);

            Socket = new Socket(Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = CommandTimeoutMs,
                SendTimeout = CommandTimeoutMs
            };
        }

        ~TcpConnection()
        {
            Disconnect();
            Socket.Dispose();
        }

        /// <inheritdoc/>
        public bool IsConnected => Socket.Connected;

        /// <inheritdoc/>
        public byte RemoteNode { get; private set; }

        /// <inheritdoc/>
        public byte LocalNode { get; private set; }

        /// <inheritdoc/>
        public bool Connect()
        {
            try
            {
                // Connect
                Socket.Connect(Endpoint);

                if (IsConnected == false)
                    return false;

                // Get node id values
                var packet = new TcpPacket(TcpPacketTypes.ClientToPlc, 0x00, 0x00)
                {
                    Parameters = new byte[] { 0x00, 0x00, 0x00, 0x00 }
                };

                if (SendData(packet) == false)
                    return false;

                var response = ReceiveData(24);

                if (response.Length < 24)
                    return false;

                RemoteNode = response[23];
                LocalNode = response[19];

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Disconnect()
        {
            if (IsConnected == false)
                return true;

            try
            {
                Socket.Disconnect(true);
                Socket.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public byte[] ReceiveData(int length)
        {
            if (IsConnected == false)
                return Array.Empty<byte>();

            try
            {
                var response = new byte[4_096];
                var received = Socket.Receive(response, length, SocketFlags.None);

                return response.Take(received).ToArray();
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        /// <inheritdoc/>
        public bool SendData(IFinsPacket packet) => SendData(packet.ToArray());

        /// <inheritdoc/>
        public bool SendData(byte[] data)
        {
            if (IsConnected == false)
                return false;

            try
            {
                return Socket.Send(data, data.Length, SocketFlags.None) == data.Length;
            }
            catch
            {
                return false;
            }
        }
    }
}
