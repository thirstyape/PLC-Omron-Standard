using PLC_Omron_Standard.Interfaces;
using System;

namespace PLC_Omron_Standard.Models
{
    internal class TcpPacket : IFinsPacket
    {
        byte ICF { get; set; }
        byte RSC { get; set; }
        byte GTC { get; set; }
        byte DNA { get; set; }
        byte DA1 { get; set; }
        byte DA2 { get; set; }
        byte SNA { get; set; }
        byte SA1 { get; set; }
        byte SA2 { get; set; }
        byte SID { get; set; }

        byte MC { get; set; }
        byte SC { get; set; }

        public byte[] ToArray()
        {
            throw new NotImplementedException();
        }
    }
}
