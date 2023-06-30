using PLC_Omron_Standard.Enums;

namespace PLC_Omron_Standard.Interfaces
{
	/// <param name="message">The error message from the PLC</param>
	public delegate void CommandError(string message);

	/// <summary>
	/// Defines properties and methods required to interact with a PLC
	/// </summary>
	internal interface ICommand
    {
		/// <summary>
		/// Notifies an error occurred during communications with the PLC
		/// </summary>
		event CommandError NotifyCommandError;

		/// <summary>
		/// Reads information from a memory area on the PLC
		/// </summary>
		/// <param name="bit">The area to read from</param>
		/// <param name="address">The specific item to read</param>
		/// <param name="position">The position within the address to read</param>
		/// <param name="length">The total amount of data to read</param>
		byte[] MemoryAreaRead(MemoryAreaBits bit, ushort address, byte position, ushort length);

        /// <summary>
        /// Writes information to a memory area on the PLC
        /// </summary>
        /// <param name="bit">The area to write to</param>
        /// <param name="address">The specific item to write</param>
        /// <param name="position">The position within the address to write</param>
        /// <param name="count">The number of items to write</param>
        /// <param name="data">The content to write</param>
        bool MemoryAreaWrite(MemoryAreaBits bit, ushort address, byte position, ushort count, byte[] data);
    }
}
