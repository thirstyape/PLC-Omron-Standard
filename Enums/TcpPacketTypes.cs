namespace PLC_Omron_Standard.Enums
{
    /// <summary>
    /// A listing of command types to use with TCP packets
    /// </summary>
    public enum TcpPacketTypes : byte
    {
        /// <summary>
        /// The client is initializing a connection with the PLC
        /// </summary>
        ClientToPlc = 0,

        /// <summary>
        /// 
        /// </summary>
        PlcToClient = 1,

        /// <summary>
        /// The packet will be a standard FINS packet
        /// </summary>
        Fins = 2
    }
}
