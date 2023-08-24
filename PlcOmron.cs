using PLC_Omron_Standard.Enums;
using PLC_Omron_Standard.Interfaces;
using PLC_Omron_Standard.Tools;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace PLC_Omron_Standard
{
	/// <summary>
	/// Main class to communicate with Omron PLCs
	/// </summary>
	public class PlcOmron
	{
		private readonly IConnection Connection;
		private readonly ICommand Command;

		/// <summary>
		/// Prepares the class for communications with a PLC
		/// </summary>
		/// <param name="ip">The IP address of the PLC</param>
		/// <param name="port">The port the PLC uses for FINS communication</param>
		/// <param name="useTcp">Spcifies whether to use TCP or UDP communication</param>
		/// <param name="remoteNode">The remote node ID to communicate with the PLC (only required for UDP)</param>
		/// <param name="localNode">The local node ID to communicate with the PLC (only required for UDP)</param>
		/// <exception cref="ArgumentException"></exception>
		public PlcOmron(string ip, int port = 9_600, bool useTcp = true, byte remoteNode = 0, byte localNode = 0)
		{
			var address = IPAddress.Parse(ip);

			if (useTcp)
			{
				Connection = new TcpConnection(address, port);
				Command = new TcpCommand(Connection);
			}
			else
			{
				if (localNode <= 0)
					throw new ArgumentException(ErrorMessages.InvalidLocalUdpNode, nameof(localNode));
				else if (remoteNode <= 0)
					throw new ArgumentException(ErrorMessages.InvalidRemoteUdpNode, nameof(remoteNode));

				Connection = new UdpConnection(address, port, remoteNode, localNode);
				Command = new UdpCommand(Connection);
			}

			Connection.NotifyReceivedData += Connection_NotifyReceivedData;
			Connection.NotifySentData += Connection_NotifySentData;
			Command.NotifyCommandError += Command_NotifyCommandError;
		}

		/// <summary>
		/// Prepares the class for communications with a PLC
		/// </summary>
		/// <param name="ip">The IP address of the PLC</param>
		/// <param name="port">The port the PLC uses for FINS communication</param>
		/// <param name="useTcp">Spcifies whether to use TCP or UDP communication</param>
		/// <param name="remoteNode">The remote node ID to communicate with the PLC (only required for UDP)</param>
		/// <param name="localNode">The local node ID to communicate with the PLC (only required for UDP)</param>
		/// <exception cref="ArgumentException"></exception>
		public PlcOmron(IPAddress ip, int port = 9_600, bool useTcp = true, byte remoteNode = 0, byte localNode = 0)
		{
			if (useTcp)
			{
				Connection = new TcpConnection(ip, port);
				Command = new TcpCommand(Connection);
			}
			else
			{
				if (localNode <= 0)
					throw new ArgumentException(ErrorMessages.InvalidLocalUdpNode, nameof(localNode));
				else if (remoteNode <= 0)
					throw new ArgumentException(ErrorMessages.InvalidRemoteUdpNode, nameof(remoteNode));

				Connection = new UdpConnection(ip, port, remoteNode, localNode);
				Command = new UdpCommand(Connection);
			}

			Connection.NotifyReceivedData += Connection_NotifyReceivedData;
			Connection.NotifySentData += Connection_NotifySentData;
			Command.NotifyCommandError += Command_NotifyCommandError;
		}

		/// <summary>
		/// Unsubscribes from events and disconnects from the PLC
		/// </summary>
		~PlcOmron()
		{
			Connection.NotifyReceivedData -= Connection_NotifyReceivedData;
			Connection.NotifySentData -= Connection_NotifySentData;
			Command.NotifyCommandError -= Command_NotifyCommandError;

			if (Connection.IsConnected)
				Connection.Disconnect();
		}

		/// <summary>
		/// Opens a connection with the PLC
		/// </summary>
		public bool Connect() => Connection.IsConnected || Connection.Connect();

		/// <summary>
		/// Closes the connection with the PLC
		/// </summary>
		public bool Disconnect() => Connection.IsConnected == false || Connection.Disconnect();

		/// <inheritdoc cref="IConnection.NotifySentData" />
		public event SentData NotifySentData;

		/// <inheritdoc cref="IConnection.NotifyReceivedData" />
		public event ReceivedData NotifyReceivedData;

		/// <inheritdoc cref="ICommand.NotifyCommandError" />
		public event CommandError NotifyCommandError;

		private void Connection_NotifySentData(byte[] sent) => NotifySentData?.Invoke(sent);
		private void Connection_NotifyReceivedData(byte[] received) => NotifyReceivedData?.Invoke(received);
		private void Command_NotifyCommandError(string message) => NotifyCommandError?.Invoke(message);

		/// <summary>
		/// Reads data from the PLC
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read (only applies when items is greater than 1)</param>
		/// <param name="area">The memory area on the PLC to read</param>
		public byte[] Read(ushort address, ushort items = 1, byte startIndex = 0, MemoryAreaBits area = MemoryAreaBits.DataMemory)
		{
			return Command.MemoryAreaRead(area, address, startIndex, items);
		}

		/// <summary>
		/// Writes the provided value(s) to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="count">The number of items to write</param>
		/// <param name="startIndex">The first position to write</param>
		/// <param name="area">The memory area on the PLC to write</param>
		public bool Write(ushort address, byte[] values, byte startIndex = 0, ushort count = 1, MemoryAreaBits area = MemoryAreaBits.DataMemory)
		{
			return Command.MemoryAreaWrite(area, address, startIndex, count, values);
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="bool"/>
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public bool ReadBool(ushort address)
		{
			var raw = Read(address);
			var expected = 2;

			if (raw.Length == expected)
				return BitConverter.ToBoolean(raw.Reverse().ToArray(), 1);
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="short"/>
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public short ReadShort(ushort address)
		{
			var raw = Read(address);
			var expected = 2;

			if (raw.Length == expected)
				return BitConverter.ToInt16(raw.Reverse().ToArray(), 0);
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="ushort"/>
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public ushort ReadUShort(ushort address)
		{
			var raw = Read(address);
			var expected = 2;

			if (raw.Length == expected)
				return BitConverter.ToUInt16(raw.Reverse().ToArray(), 0);
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="int"/>
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public int ReadInt(ushort address)
		{
			var raw = Read(address, 2);
			var expected = 4;

			if (raw.Length == expected)
				return BitConverter.ToInt32(raw.Reverse().ToArray(), 0);
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="uint"/>
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public uint ReadUInt(ushort address)
		{
			var raw = Read(address, 2);
			var expected = 4;

			if (raw.Length == expected)
				return BitConverter.ToUInt32(raw.Reverse().ToArray(), 0);
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="float"/>
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public float ReadFloat(ushort address)
		{
			var raw = Read(address, 2);
			var expected = 4;

			if (raw.Length == expected)
				return BitConverter.ToSingle(raw.Reverse().ToArray(), 0);
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="string"/>
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <exception cref="NullReferenceException"></exception>
		public string ReadString(ushort address)
		{
			var raw = Read(address);

			if (raw.Length > 0)
				return Encoding.ASCII.GetString(Read(address));
			else
				throw new NullReferenceException(ErrorMessages.NoDataReceived);
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="bool"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public bool[] ReadBoolArray(ushort address, ushort items, byte startIndex = 0)
		{
			var raw = Read(address, items, startIndex);
			var expected = items * 2;

			if (raw.Length == expected)
				return raw.Partition(2, false).Select(x => BitConverter.ToBoolean(x.Reverse().ToArray(), 1)).ToArray();
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="short"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public short[] ReadShortArray(ushort address, ushort items, byte startIndex = 0)
		{
			var raw = Read(address, items, startIndex);
			var expected = items * 2;

			if (raw.Length == expected)
				return raw.Partition(2, false).Select(x => BitConverter.ToInt16(x.Reverse().ToArray(), 0)).ToArray();
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="ushort"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public ushort[] ReadUShortArray(ushort address, ushort items, byte startIndex = 0)
		{
			var raw = Read(address, items, startIndex);
			var expected = items * 2;

			if (raw.Length == expected)
				return raw.Partition(2, false).Select(x => BitConverter.ToUInt16(x.Reverse().ToArray(), 0)).ToArray();
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="int"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public int[] ReadIntArray(ushort address, ushort items, byte startIndex = 0)
		{
			var raw = Read(address, (ushort)(items * 2), startIndex);
			var expected = items * 4;

			if (raw.Length == expected)
				return raw.Partition(4, false).Select(x => BitConverter.ToInt32(x.Reverse().ToArray(), 0)).ToArray();
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="uint"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">TThe total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public uint[] ReadUIntArray(ushort address, ushort items, byte startIndex = 0)
		{
			var raw = Read(address, (ushort)(items * 2), startIndex);
			var expected = items * 4;

			if (raw.Length == expected)
				return raw.Partition(4, false).Select(x => BitConverter.ToUInt32(x.Reverse().ToArray(), 0)).ToArray();
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="float"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public float[] ReadFloatArray(ushort address, ushort items, byte startIndex = 0)
		{
			var raw = Read(address, (ushort)(items * 2), startIndex);
			var expected = items * 4;

			if (raw.Length == expected)
				return raw.Partition(4, false).Select(x => BitConverter.ToSingle(x.Reverse().ToArray(), 0)).ToArray();
			else if (raw.Length > expected)
				throw new ArgumentException(ErrorMessages.ReceivedTooMuchData(expected, raw.Length));
			else
				throw new NullReferenceException(ErrorMessages.NotEnoughDataRecieved(expected, raw.Length));
		}

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="string"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		/// <exception cref="NullReferenceException"></exception>
		public string[] ReadStringArray(ushort address, ushort items, byte startIndex = 0)
		{
			var raw = Read(address, items, startIndex);

			if (raw.Length > 0)
				return Encoding.ASCII.GetString(raw).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
			else
				throw new NullReferenceException(ErrorMessages.NoDataReceived);
		}

		/// <summary>
		/// Writes the provided value to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="value">The value to write to the PLC</param>
		public bool Write(ushort address, bool value)
		{
			return Write(address, BitConverter.GetBytes(value).Concat(new byte[] { 0 }).Reverse().ToArray());
		}

		/// <summary>
		/// Writes the provided value to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="value">The value to write to the PLC</param>
		public bool Write(ushort address, short value)
		{
			return Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
		}

		/// <summary>
		/// Writes the provided value to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="value">The value to write to the PLC</param>
		public bool Write(ushort address, ushort value)
		{
			return Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
		}

		/// <summary>
		/// Writes the provided value to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="value">The value to write to the PLC</param>
		/// <remarks>
		/// Writes to the two consecutive sub-addresses at the provided start index
		/// </remarks>
		public bool Write(ushort address, int value)
		{
			return Write(address, BitConverter.GetBytes(value).Reverse().ToArray(), count: 2);
		}

		/// <summary>
		/// Writes the provided value to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="value">The value to write to the PLC</param>
		/// <remarks>
		/// Writes to the two consecutive sub-addresses at the provided start index
		/// </remarks>
		public bool Write(ushort address, uint value)
		{
			return Write(address, BitConverter.GetBytes(value).Reverse().ToArray(), count: 2);
		}

		/// <summary>
		/// Writes the provided value to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="value">The value to write to the PLC</param>
		/// <remarks>
		/// Writes to the two consecutive sub-addresses at the provided start index
		/// </remarks>
		public bool Write(ushort address, float value)
		{
			return Write(address, BitConverter.GetBytes(value).Reverse().ToArray(), count: 2);
		}

		/// <summary>
		/// Writes the provided value to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="value">The value to write to the PLC</param>
		public bool Write(ushort address, string value)
		{
			return Write(address, Encoding.ASCII.GetBytes(value).ToArray());
		}

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, bool[] values, byte startIndex = 0)
		{
			return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Concat(new byte[] { 0 }).Reverse()).ToArray(), startIndex, (ushort)values.Length);
		}

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, short[] values, byte startIndex = 0)
		{
			return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex, (ushort)values.Length);
		}

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, ushort[] values, byte startIndex = 0)
		{
			return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex, (ushort)values.Length);
		}

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		/// <remarks>
		/// Writes values to pairs of two consecutive sub-addresses
		/// </remarks>
		public bool Write(ushort address, int[] values, byte startIndex = 0)
		{
			return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex, (ushort)(values.Length * 2));
		}

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		/// <remarks>
		/// Writes values to pairs of two consecutive sub-addresses
		/// </remarks>
		public bool Write(ushort address, uint[] values, byte startIndex = 0)
		{
			return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex, (ushort)(values.Length * 2));
		}

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		/// <remarks>
		/// Writes values to pairs of two consecutive sub-addresses
		/// </remarks>
		public bool Write(ushort address, float[] values, byte startIndex = 0)
		{
			return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex, (ushort)(values.Length * 2));
		}

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, string[] values, byte startIndex = 0)
		{
			return Write(address, values.SelectMany(x => Encoding.ASCII.GetBytes(x)).ToArray(), startIndex, (ushort)values.Length);
		}
	}
}
