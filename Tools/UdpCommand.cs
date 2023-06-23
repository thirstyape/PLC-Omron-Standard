﻿using PLC_Omron_Standard.Enums;
using PLC_Omron_Standard.Interfaces;

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
        public byte[] MemoryAreaRead(MemoryAreaBits bit, ushort address, byte position, ushort length)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public bool MemoryAreaWrite(MemoryAreaBits bit, ushort address, byte position, byte[] data)
        {
            throw new System.NotImplementedException();
        }
    }
}