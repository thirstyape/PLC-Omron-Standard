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

        public PlcOmron(string ip, int port = 9_600, bool useTcp = true)
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

        public PlcOmron(IPAddress ip, int port = 9_600, bool useTcp = true)
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

        ~PlcOmron()
        {
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

        /// <summary>
        /// Reads data from the PLC
        /// </summary>
        /// <param name="address">The specific item to read</param>
        /// <param name="items">The total number of data points to read from the PLC</param>
        /// <param name="startIndex">The first position to read (only applies when items is greater than 1)</param>
        public byte[] Read(ushort address, ushort items = 1, byte startIndex = 0)
        {
            return Command.MemoryAreaRead(MemoryAreaBits.DataMemory, address, startIndex, items);
        }

        /// <summary>
        /// Reads data from the PLC and converts the result to a <see cref="bool"/>
        /// </summary>
        /// <param name="address">The specific item to read</param>
        public bool ReadBool(ushort address)
        {
            return BitConverter.ToBoolean(Read(address), 0);
        }

        /// <summary>
        /// Reads data from the PLC and converts the result to a <see cref="short"/>
        /// </summary>
        /// <param name="address">The specific item to read</param>
        public short ReadShort(ushort address)
        {
            return BitConverter.ToInt16(Read(address).Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Reads data from the PLC and converts the result to a <see cref="ushort"/>
        /// </summary>
        /// <param name="address">The specific item to read</param>
        public ushort ReadUShort(ushort address)
        {
            return BitConverter.ToUInt16(Read(address).Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Reads data from the PLC and converts the result to a <see cref="int"/>
        /// </summary>
        /// <param name="address">The specific item to read</param>
        public int ReadInt(ushort address)
        {
            return BitConverter.ToInt32(Read(address).Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Reads data from the PLC and converts the result to a <see cref="uint"/>
        /// </summary>
        /// <param name="address">The specific item to read</param>
        public uint ReadUInt(ushort address)
        {
            return BitConverter.ToUInt32(Read(address).Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Reads data from the PLC and converts the result to a <see cref="float"/>
        /// </summary>
        /// <param name="address">The specific item to read</param>
        public float ReadFloat(ushort address)
        {
            return BitConverter.ToSingle(Read(address).Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Reads data from the PLC and converts the result to a <see cref="string"/>
        /// </summary>
        /// <param name="address">The specific item to read</param>
        public string ReadString(ushort address)
        {
            return Encoding.ASCII.GetString(Read(address));
        }

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="bool"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		public bool[] ReadBoolArray(ushort address, ushort items, byte startIndex = 0)
        {
            return Read(address, items, startIndex).Select(x => BitConverter.ToBoolean(new byte[] { x }, 0)).ToArray();
        }

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="short"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		public short[] ReadShortArray(ushort address, ushort items, byte startIndex = 0)
        {
            return Read(address, items, startIndex).Partition(2, false).Select(x => BitConverter.ToInt16(x.Reverse().ToArray(), 0)).ToArray();
        }

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="ushort"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		public ushort[] ReadUShortArray(ushort address, ushort items, byte startIndex = 0)
        {
            return Read(address, items, startIndex).Partition(2, false).Select(x => BitConverter.ToUInt16(x.Reverse().ToArray(), 0)).ToArray();
        }

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="int"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		public int[] ReadIntArray(ushort address, ushort items, byte startIndex = 0)
        {
            return Read(address, items, startIndex).Partition(4, false).Select(x => BitConverter.ToInt32(x.Reverse().ToArray(), 0)).ToArray();
        }

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="uint"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">TThe total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		public uint[] ReadUIntArray(ushort address, ushort items, byte startIndex = 0)
        {
            return Read(address, items, startIndex).Partition(4, false).Select(x => BitConverter.ToUInt32(x.Reverse().ToArray(), 0)).ToArray();
        }

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="float"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		public float[] ReadFloatArray(ushort address, ushort items, byte startIndex = 0)
        {
            return Read(address, items, startIndex).Partition(4, false).Select(x => BitConverter.ToSingle(x.Reverse().ToArray(), 0)).ToArray();
        }

		/// <summary>
		/// Reads data from the PLC and converts the result to a <see cref="string"/> array
		/// </summary>
		/// <param name="address">The specific item to read</param>
		/// <param name="items">The total number of data points to read from the PLC</param>
		/// <param name="startIndex">The first position to read</param>
		public string[] ReadStringArray(ushort address, ushort items, byte startIndex = 0)
        {
            return Encoding.ASCII.GetString(Read(address, items, startIndex)).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, bool value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, short value)
        {
            return Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, ushort value)
        {
            return Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, int value)
        {
            return Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, uint value)
        {
            return Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, float value)
        {
            return Write(address, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
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
            return Write(address, values.SelectMany(x => BitConverter.GetBytes(x)).ToArray(), startIndex);
        }

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, short[] values, byte startIndex = 0)
        {
            return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex);
        }

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, ushort[] values, byte startIndex = 0)
        {
            return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex);
        }

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, int[] values, byte startIndex = 0)
        {
            return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex);
        }

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, uint[] values, byte startIndex = 0)
        {
            return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex);
        }

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, float[] values, byte startIndex = 0)
        {
            return Write(address, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray(), startIndex);
        }

		/// <summary>
		/// Writes the provided values to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, string[] values, byte startIndex = 0)
        {
            return Write(address, values.SelectMany(x => Encoding.ASCII.GetBytes(x)).ToArray(), startIndex);
        }

		/// <summary>
		/// Writes the provided value(s) to the PLC
		/// </summary>
		/// <param name="address">The address on the PLC to write to</param>
		/// <param name="values">The values to write to the PLC</param>
		/// <param name="startIndex">The first position to write</param>
		public bool Write(ushort address, byte[] values, byte startIndex = 0)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, startIndex, values);
        }
    }
}
