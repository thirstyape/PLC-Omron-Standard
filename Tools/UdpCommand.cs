using PLC_Omron_Standard.Enums;
using PLC_Omron_Standard.Interfaces;
using PLC_Omron_Standard.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PLC_Omron_Standard.Tools
{
    /// <summary>
    /// Implementation of <see cref="ICommand"/> to facilitate UDP based interactions
    /// </summary>
    internal class UdpCommand : ICommand
    {
        private readonly IConnection Connection;

        public UdpCommand(IConnection connection)
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

            var packet = new UdpPacket(Connection.RemoteNode, Connection.LocalNode)
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

			// Receive FINS header + data
			var data = Connection.ReceiveData(0);

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
			}

			// Build request
			var action = "memory area write";
			var parameters = new List<byte>
            {
                (byte)bit
            };

            parameters.AddRange(BitConverter.GetBytes(address).Reverse());
            parameters.Add(position);
            parameters.AddRange(BitConverter.GetBytes(count).Reverse());

            var packet = new UdpPacket(Connection.RemoteNode, Connection.LocalNode)
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

			// Receive FINS header
			var response = Connection.ReceiveData(0);

			if (response.Length == 0)
            {
				NotifyCommandError?.Invoke(ErrorMessages.NoResponse(action));
				return false;
			}
            else if (response.Length < 14)
            {
				NotifyCommandError?.Invoke(ErrorMessages.InvalidFinsHeader(action, response.Length));
				return true;
            }

			if (response[12] != 0 && ResponseCodesDictionary.IsError(response[12]))
			{
				NotifyCommandError?.Invoke(ResponseCodesDictionary.GetCodeMessage(response[12]));
				return false;
			}
			else if (response[13] != 0 && ResponseCodesDictionary.IsError(response[13]))
			{
				NotifyCommandError?.Invoke(ResponseCodesDictionary.GetCodeMessage(response[13]));
				return false;
			}

			return true;
        }
    }
}
