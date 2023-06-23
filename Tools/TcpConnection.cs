using PLC_Omron_Standard.Interfaces;
using System;
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
            if (Socket != null)
            {
                Disconnect();
                Socket.Dispose();
            }
        }

        /// <inheritdoc/>
        public bool IsConnected => Socket?.Connected ?? false;

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
            if (Socket == null || IsConnected == false)
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
                var response = new byte[length];

                Socket.Receive(response, length, SocketFlags.None);
                return response;
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

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
