using PLC_Omron_Standard.Interfaces;
using System;
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
		private readonly UdpClient Client;
		private IPEndPoint Endpoint;

		public UdpConnection(IPAddress ip, int port, byte remoteNode, byte localNode)
		{
			Endpoint = new IPEndPoint(ip, port);

			Client = new UdpClient();

			Client.Client.SendTimeout = CommandTimeoutMs;
			Client.Client.ReceiveTimeout = CommandTimeoutMs;

			RemoteNode = remoteNode;
			LocalNode = localNode;
		}

		~UdpConnection()
		{
			Disconnect();
			Client.Dispose();
		}

		/// <inheritdoc/>
		public event SentData NotifySentData;

		/// <inheritdoc/>
		public event ReceivedData NotifyReceivedData;

		/// <inheritdoc/>
		public bool IsConnected => Client?.Client?.Connected ?? false;

		/// <inheritdoc/>
		public byte RemoteNode { get; private set; }

		/// <inheritdoc/>
		public byte LocalNode { get; private set; }

		/// <inheritdoc/>
		public bool Connect()
		{
			try
			{
				Client.Connect(Endpoint);
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
				Client.Close();
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
				var received = Client.Receive(ref Endpoint);
				NotifyReceivedData?.Invoke(received);

				return received;
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
				var sent = Client.Send(data, data.Length);
				NotifySentData?.Invoke(data);

				return sent == data.Length;
			}
			catch
			{
				return false;
			}
		}
	}
}
