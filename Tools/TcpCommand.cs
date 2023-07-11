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
		public event CommandError NotifyCommandError;

		/// <inheritdoc/>
		public byte[] MemoryAreaRead(MemoryAreaBits bit, ushort address, byte position, ushort length)
        {
			// Check connection
            if (Connection.IsConnected == false)
            {
				NotifyCommandError?.Invoke(ErrorMessages.PlcNotConnected);
				return Array.Empty<byte>();
			}

			// Build request
			var action = "memory area read";
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

			// Send request
			if (Connection.SendData(packet) == false)
            {
				NotifyCommandError?.Invoke(ErrorMessages.FailedRequestingPlcRead);
				return Array.Empty<byte>();
			}

			// Receive TCP header
			var header = Connection.ReceiveData(16);

            if (header.Length == 0)
            {
				NotifyCommandError?.Invoke(ErrorMessages.NoResponse(action));
                return Array.Empty<byte>();
			}
			else if (header.Length >= 4 && header.Take(4).SequenceEqual(packet.FinsHeader) == false)
			{
				NotifyCommandError?.Invoke(ErrorMessages.InvalidTcpHeader(action, header.Take(4).ToArray()));
				return Array.Empty<byte>();
			}
			else if (header.Length < 16)
            {
				NotifyCommandError?.Invoke(ErrorMessages.InvalidTcpHeader(action, header.Length));
				return Array.Empty<byte>();
			}

			// Receive FINS header + data
			var available = BitConverter.ToUInt16(new byte[] { header[6], header[7] }.Reverse().ToArray(), 0);
            var data = Connection.ReceiveData(available + 14);

			if (data.Length == 0)
			{
				NotifyCommandError?.Invoke(ErrorMessages.NoResponse(action));
				return Array.Empty<byte>();
			}
			else if (data.Length < 14)
			{
				NotifyCommandError?.Invoke(ErrorMessages.InvalidFinsHeader(action, data.Length));
				return Array.Empty<byte>();
			}

			if (data[12] != 0 && ResponseCodesDictionary.IsError(data[12]))
                NotifyCommandError?.Invoke(ResponseCodesDictionary.GetCodeMessage(data[12]));
            else if (data[13] != 0 && ResponseCodesDictionary.IsError(data[13]))
				NotifyCommandError?.Invoke(ResponseCodesDictionary.GetCodeMessage(data[13]));

			return data.Skip(14).ToArray();
        }

        /// <inheritdoc/>
        public bool MemoryAreaWrite(MemoryAreaBits bit, ushort address, byte position, ushort count, byte[] data)
        {
			// Check connection
			if (Connection.IsConnected == false)
            {
				NotifyCommandError?.Invoke(ErrorMessages.PlcNotConnected);
				return false;
			};

			// Build request
			var action = "memory area write";
			var parameters = new List<byte>
            {
                (byte)bit
            };

            parameters.AddRange(BitConverter.GetBytes(address).Reverse());
            parameters.Add(position);
            parameters.AddRange(BitConverter.GetBytes(count).Reverse());

            var packet = new TcpPacket(TcpPacketTypes.Fins, Connection.RemoteNode, Connection.LocalNode)
            {
                MC = (byte)PlcFunctions.MemoryArea,
                SC = (byte)MemoryAreaSubfunctions.Write,
                Parameters = parameters.ToArray(),
                Data = data
            };

			// Send data
            if (Connection.SendData(packet) == false)
            {
				NotifyCommandError?.Invoke(ErrorMessages.FailedWritingToPlc);
				return false;
			}

			// Receive TCP header + FINS header
			var response = Connection.ReceiveData(30);

			if (response.Length == 0)
            {
				NotifyCommandError?.Invoke(ErrorMessages.NoResponse(action));
				return false;
			}
            else if (response.Length >= 4 && response.Take(4).SequenceEqual(packet.FinsHeader) == false)
            {
				NotifyCommandError?.Invoke(ErrorMessages.InvalidTcpHeader(action, response.Take(4).ToArray()));
				return false;
			}
			else if (response.Length < 16)
			{
				NotifyCommandError?.Invoke(ErrorMessages.InvalidTcpHeader(action, response.Length));
				return true;
			}
			else if (response.Length < 30)
			{
				NotifyCommandError?.Invoke(ErrorMessages.InvalidFinsHeader(action, response.Length - 16));
				return true;
			}

			if (response[28] != 0 && ResponseCodesDictionary.IsError(response[28]))
			{
				NotifyCommandError?.Invoke(ResponseCodesDictionary.GetCodeMessage(response[28]));
				return false;
			}
			else if (response[29] != 0 && ResponseCodesDictionary.IsError(response[29]))
			{
				NotifyCommandError?.Invoke(ResponseCodesDictionary.GetCodeMessage(response[29]));
				return false;
			}

			return true;
		}
    }
}
