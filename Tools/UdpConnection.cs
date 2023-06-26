using PLC_Omron_Standard.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PLC_Omron_Standard.Tools
{
    /// <summary>
    /// Implementation of <see cref="IConnection"/> to facilitate UDP based connections
    /// </summary>
    internal class UdpConnection : IConnection
    {
        private const int CommandTimeoutMs = 2_000;
        private readonly Socket Socket;
        private readonly IPEndPoint Endpoint;

        public UdpConnection(IPAddress ip, int port)
        {
            Endpoint = new IPEndPoint(ip, port);

            Socket = new Socket(Endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = CommandTimeoutMs,
                SendTimeout = CommandTimeoutMs
            };
        }

        ~UdpConnection()
        {
            if (Socket != null)
            {
                Disconnect();
                Socket.Dispose();
            }
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
                Socket.Connect(Endpoint);
                return IsConnected;
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
                Socket.Shutdown(SocketShutdown.Both);
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
                var response = new byte[length];
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
