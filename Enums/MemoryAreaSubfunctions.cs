namespace PLC_Omron_Standard.Enums
{
    /// <summary>
    /// A list of sub-functions available on the memory area function
    /// </summary>
    public enum MemoryAreaSubfunctions : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Read = 0x01,

        /// <summary>
        /// 
        /// </summary>
        Write = 0x02,

        /// <summary>
        /// 
        /// </summary>
        Fill = 0x03,

        /// <summary>
        /// 
        /// </summary>
        MultipleRead = 0x04,

        /// <summary>
        /// 
        /// </summary>
        Transfer = 0x05
    }
}
