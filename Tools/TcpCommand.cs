using PLC_Omron_Standard.Enums;
using PLC_Omron_Standard.Interfaces;
using PLC_Omron_Standard.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PLC_Omron_Standard.Tools
{
    /// <summary>
    /// Implementation of <see cref="ICommand"/> to facilitate TCP based interactions
    /// </summary>
    internal class TcpCommand : ICommand
    {
        private readonly IConnection Connection;

        public TcpCommand(IConnection connection)
        {
            Connection = connection;
        }

        /// <inheritdoc/>
        public byte[] MemoryAreaRead(MemoryAreaBits bit, ushort address, byte position, ushort length)
        {
            if (Connection.IsConnected == false)
                return Array.Empty<byte>();

            var parameters = new List<byte>
            {
                (byte)bit
            };

            parameters.AddRange(BitConverter.GetBytes(address).Reverse());
            parameters.Add(position);
            parameters.AddRange(BitConverter.GetBytes(length).Reverse());

            var packet = new TcpPacket(TcpPacketTypes.Fins, Connection.RemoteNode, Connection.LocalNode)
            {
                MC = (byte)PlcFunctions.MemoryArea,
                SC = (byte)MemoryAreaSubfunctions.Read,
                Parameters = parameters.ToArray()
            };

            if (Connection.SendData(packet) == false)
                return Array.Empty<byte>();

            var header = Connection.ReceiveData(16);

            if (header.Length < 16)
				return Array.Empty<byte>();

            var amount = BitConverter.ToUInt16(new byte[] { header[6], header[7] }.Reverse().ToArray(), 0);
			return Connection.ReceiveData(amount + 14).Skip(14).ToArray();
        }

        /// <inheritdoc/>
        public bool MemoryAreaWrite(MemoryAreaBits bit, ushort address, byte position, byte[] data)
        {
            if (Connection.IsConnected == false)
                return false;

            var parameters = new List<byte>
            {
                (byte)bit
            };

            parameters.AddRange(BitConverter.GetBytes(address).Reverse());
            parameters.Add(position);
            parameters.AddRange(BitConverter.GetBytes(data.Length).Reverse());

            var packet = new TcpPacket(TcpPacketTypes.Fins, Connection.RemoteNode, Connection.LocalNode)
            {
                MC = (byte)PlcFunctions.MemoryArea,
                SC = (byte)MemoryAreaSubfunctions.Write,
                Parameters = parameters.ToArray(),
                Data = data
            };

            if (Connection.SendData(packet) == false)
                return false;

            var response = Connection.ReceiveData(16);
            return response.Length >= 16 && response.Take(4).SequenceEqual(packet.FinsHeader);
        }
    }
}
