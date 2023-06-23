using PLC_Omron_Standard.Enums;

namespace PLC_Omron_Standard.Interfaces
{
    /// <summary>
    /// Defines properties and methods required to interact with a PLC
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bit"></param>
        /// <param name="address"></param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        byte[] MemoryAreaRead(MemoryAreaBits bit, ushort address, byte position, ushort length);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bit"></param>
        /// <param name="address"></param>
        /// <param name="position"></param>
        /// <param name="data"></param>
        bool MemoryAreaWrite(MemoryAreaBits bit, ushort address, byte position, byte[] data);
    }
}
