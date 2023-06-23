namespace PLC_Omron_Standard.Interfaces
{
    /// <summary>
    /// Defines methods and properties required to communicate with a PLC
    /// </summary>
    internal interface IConnection
    {
        /// <summary>
        /// Specifies whether there is an active connection to the PLC
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
		/// Opens a connection with the PLC
		/// </summary>
		bool Connect();

        /// <summary>
        /// Closes the connection with the PLC
        /// </summary>
        bool Disconnect();

        /// <summary>
        /// Sends the provided data to the PLC
        /// </summary>
        /// <param name="data">The data to send</param>
        bool SendData(byte[] data);

        /// <summary>
        /// Receives the specified amount of data from the PLC
        /// </summary>
        /// <param name="length">The amount of data to receive</param>
        byte[] ReceiveData(int length);
    }
}
