using PLC_Omron_Standard.Interfaces;
using System;

namespace PLC_Omron_Standard.Models
{
    /// <summary>
    /// Base class to construct messages for communicating with Omron PLCs via FINS
    /// </summary>
    internal class FinsPacketBase : IFinsPacket
    {
        /// <summary>
        /// Information control field
        /// </summary>
        protected byte ICF { get; } = 0x80;

        /// <summary>
        /// Reserved
        /// </summary>
        protected byte RSC { get; } = 0x00;

        /// <summary>
        /// Gateway count
        /// </summary>
        protected byte GTC { get; } = 0x02;

        /// <summary>
        /// Destination network address (0 = local network)
        /// </summary>
        protected byte DNA { get; } = 0x00;

        /// <summary>
        /// Destination node number
        /// </summary>
        protected byte DA1 { get; set; } = 0x00;

        /// <summary>
        /// Destination unit address
        /// </summary>
        protected byte DA2 { get; } = 0x00;

        /// <summary>
        /// Source network address (0 = local network)
        /// </summary>
        protected byte SNA { get; } = 0x00;

        /// <summary>
        /// Source node number
        /// </summary>
        protected byte SA1 { get; set; } = 0x00;

        /// <summary>
        /// Source unit address
        /// </summary>
        protected byte SA2 { get; } = 0x00;

        /// <summary>
        /// Service ID
        /// </summary>
        protected byte SID { get; set; } = 0x01;

        /// <summary>
        /// Main command
        /// </summary>
        public byte MC { get; set; } = 0x00;

        /// <summary>
        /// Subcommand
        /// </summary>
        public byte SC { get; set; } = 0x00;

        /// <summary>
        /// Additional options to send with the packet
        /// </summary>
        public byte[] Parameters { get; set; } = Array.Empty<byte>();

        /// <inheritdoc/>
        public byte[] Data { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// The length of the FINS command array that will be generated
        /// </summary>
        protected const int FinsLength = 12;

        /// <inheritdoc/>
        public virtual byte[] ToArray()
        {
            return new byte[] {
                ICF,
                RSC,
                GTC,
                DNA,
                DA1,
                DA2,
                SNA,
                SA1,
                SA2,
                SID,
                MC,
                SC
            };
        }
    }
}
