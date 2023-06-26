namespace PLC_Omron_Standard.Interfaces
{
    /// <summary>
    /// Defines properties and methods required for a FINS command packet
    /// </summary>
    internal interface IFinsPacket
    {
        /// <summary>
        /// The information to send to the PLC
        /// </summary>
        byte[] Data { get; set; }

        /// <summary>
        /// Returns the packet as a byte array ready for sending to the PLC
        /// </summary>
        byte[] ToArray();
    }
}
