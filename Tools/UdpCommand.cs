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
            if (Connection.IsConnected == false)
            {
				NotifyCommandError?.Invoke("PLC is not connected");
				return Array.Empty<byte>();
			}

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

            if (Connection.SendData(packet) == false)
            {
				NotifyCommandError?.Invoke("Failed to request data from PLC for memory area read");
				return Array.Empty<byte>();
			}

			var data = Connection.ReceiveData(0);

            if (data.Length == 0)
            {
				NotifyCommandError?.Invoke("Failed reading FINS header from PLC memory area");
				return Array.Empty<byte>();
			}
            else if (data.Length < 14)
            {
				NotifyCommandError?.Invoke("Failed receiving data from PLC memory area");
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
            if (Connection.IsConnected == false)
            {
				NotifyCommandError?.Invoke("PLC is not connected");
				return false;
			}

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

            if (Connection.SendData(packet) == false)
            {
				NotifyCommandError?.Invoke("Failed writing to PLC memory area");
				return false;
			}
            
            var response = Connection.ReceiveData(0);

			if (response.Length == 0)
            {
				NotifyCommandError?.Invoke("Failed to receive response from PLC for memory area write");
				return false;
			}
            else if (response.Length < 14)
            {
				NotifyCommandError?.Invoke("Did not receive correct FINS header during write to memory area");
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
