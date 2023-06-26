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
        /// <param name="length">The total amount of data to read</param>
        public byte[] Read(ushort address, ushort length)
        {
            return Command.MemoryAreaRead(MemoryAreaBits.DataMemory, address, 0, length);
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, bool value)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, short value)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, ushort value)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, int value)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, uint value)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, float value)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, BitConverter.GetBytes(value).Reverse().ToArray());
        }

        /// <summary>
        /// Writes the provided value to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The value to write to the PLC</param>
        public bool Write(ushort address, string value)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, Encoding.ASCII.GetBytes(value).ToArray());
        }

        /// <summary>
        /// Writes the provided values to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The values to write to the PLC</param>
        public bool Write(ushort address, bool[] values)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray());
        }

        /// <summary>
        /// Writes the provided values to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The values to write to the PLC</param>
        public bool Write(ushort address, short[] values)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray());
        }

        /// <summary>
        /// Writes the provided values to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The values to write to the PLC</param>
        public bool Write(ushort address, ushort[] values)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray());
        }

        /// <summary>
        /// Writes the provided values to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The values to write to the PLC</param>
        public bool Write(ushort address, int[] values)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray());
        }

        /// <summary>
        /// Writes the provided values to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The values to write to the PLC</param>
        public bool Write(ushort address, uint[] values)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray());
        }

        /// <summary>
        /// Writes the provided values to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The values to write to the PLC</param>
        public bool Write(ushort address, float[] values)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, values.SelectMany(x => BitConverter.GetBytes(x).Reverse()).ToArray());
        }

        /// <summary>
        /// Writes the provided values to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The values to write to the PLC</param>
        public bool Write(ushort address, string[] values)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, values.SelectMany(x => Encoding.ASCII.GetBytes(x)).ToArray());
        }
        
        /// <summary>
        /// Writes the provided value(s) to the PLC
        /// </summary>
        /// <param name="address">The address on the PLC to write to</param>
        /// <param name="values">The values to write to the PLC</param>
        public bool Write(ushort address, byte[] values)
        {
            return Command.MemoryAreaWrite(MemoryAreaBits.DataMemory, address, 0, values);
        }
    }
}
