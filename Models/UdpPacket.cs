using PLC_Omron_Standard.Interfaces;
using System.Linq;

namespace PLC_Omron_Standard.Models
{
    /// <summary>
    /// Facilitates building UDP messages to send to Omron PLCs
    /// </summary>
    internal class UdpPacket : FinsPacketBase, IFinsPacket
    {
        public UdpPacket(byte remoteNode, byte localNode)
        {
            DA1 = remoteNode;
            SA1 = localNode;
        }

        /// <inheritdoc/>
        public override byte[] ToArray()
        {
            return base.ToArray()
                .Concat(Parameters)
                .Concat(Data)
                .ToArray();
        }
    }
}
