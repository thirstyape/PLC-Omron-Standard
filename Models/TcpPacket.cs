using PLC_Omron_Standard.Enums;
using PLC_Omron_Standard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PLC_Omron_Standard.Models
{
    /// <summary>
    /// Facilitates building TCP messages to send to Omron PLCs
    /// </summary>
    internal class TcpPacket : FinsPacketBase, IFinsPacket
    {
        public TcpPacket(TcpPacketTypes type, byte remoteNode, byte localNode)
        {
            CommandType = type;
            DA1 = remoteNode;
            SA1 = localNode;
        }

        /// <summary>
        /// Returns the TCP header packet
        /// </summary>
        public IEnumerable<byte> FinsHeader => new byte[] { (byte)'F', (byte)'I', (byte)'N', (byte)'S' };

        /// <summary>
        /// Returns the length of the message to be sent
        /// </summary>
        private IEnumerable<byte> PacketLength => CommandType == TcpPacketTypes.Fins ?
            BitConverter.GetBytes(Convert.ToUInt32(TcpLength + FinsLength + Parameters.Length + Data.Length)).Reverse() :
            BitConverter.GetBytes(Convert.ToUInt32(TcpLength + Parameters.Length)).Reverse();

        /// <summary>
        /// Returns the command type to issue to the PLC
        /// </summary>
        private IEnumerable<byte> FinsCommand => new byte[] { 0x00, 0x00, 0x00, (byte)CommandType };

        /// <summary>
        /// Stores the selected command type
        /// </summary>
        private TcpPacketTypes CommandType { get; set; } = TcpPacketTypes.Fins;

        /// <summary>
        /// Stores any errors that occur during communication
        /// </summary>
        private IEnumerable<byte> FinsError => new byte[] { 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// The length of the TCP headers without the FINS and Length portions
        /// </summary>
        protected const int TcpLength = 8;

        /// <inheritdoc/>
        public override byte[] ToArray()
        {
            if (CommandType == TcpPacketTypes.Fins)
                return FinsHeader
                    .Concat(PacketLength)
                    .Concat(FinsCommand)
                    .Concat(FinsError)
                    .Concat(base.ToArray())
                    .Concat(Parameters)
                    .Concat(Data)
                    .ToArray();
            else
                return FinsHeader
                    .Concat(PacketLength)
                    .Concat(FinsCommand)
                    .Concat(FinsError)
                    .Concat(Parameters)
                    .ToArray();
        }
    }
}
