namespace PLC_Omron_Standard.Enums
{
    /// <summary>
    /// A listing of bits available in the PLC memory area
    /// </summary>
    public enum MemoryAreaBits : byte
    {
        /// <summary>
        /// 
        /// </summary>
        DataMemory = 0x82,

        /// <summary>
        /// 
        /// </summary>
        CommonIO = 0x30,

        /// <summary>
        /// 
        /// </summary>
        Work = 0x31,

        /// <summary>
        /// 
        /// </summary>
        Holding = 0x32,

        /// <summary>
        /// 
        /// </summary>
        Auxiliary = 0x33
    }
}
