namespace PLC_Omron_Standard.Enums
{
    /// <summary>
    /// A listing of functions available on the PLC
    /// </summary>
    public enum PlcFunctions : byte
    {
        /// <summary>
        /// Use to read and write to memory on the PLC
        /// </summary>
        MemoryArea = 0x01,

        /// <summary>
        /// 
        /// </summary>
        ParameterArea = 0x02,

        /// <summary>
        /// 
        /// </summary>
        ProgramArea = 0x03,

        /// <summary>
        /// 
        /// </summary>
        OperatingMode = 0x04,

        /// <summary>
        /// 
        /// </summary>
        MachineConfiguration = 0x05,

        /// <summary>
        /// 
        /// </summary>
        Status = 0x06,

        /// <summary>
        /// 
        /// </summary>
        TimeData = 0x07,

        /// <summary>
        /// 
        /// </summary>
        MessageDisplay = 0x09,

        /// <summary>
        /// 
        /// </summary>
        AccessRights = 0x0C,

        /// <summary>
        /// 
        /// </summary>
        ErrorLog = 0x21,

        /// <summary>
        /// 
        /// </summary>
        FileMemory = 0x22,

        /// <summary>
        /// 
        /// </summary>
        Debugging = 0x23,

        /// <summary>
        /// 
        /// </summary>
        SerialGateway = 0x28
    }
}
