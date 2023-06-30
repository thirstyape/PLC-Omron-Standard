namespace PLC_Omron_Standard.Interfaces
{
    /// <param name="sent">The data that was sent to the PLC</param>
	public delegate void SentData(byte[] sent);

    /// <param name="received">The data that was received from the PLC</param>
	public delegate void ReceivedData(byte[] received);

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
        /// Stores the remote node address for communication
        /// </summary>
        byte RemoteNode { get; }

        /// <summary>
        /// Stores the local node address for communication
        /// </summary>
        byte LocalNode { get; }

        /// <summary>
        /// Notifies data was sent to the PLC and returns the bytes that were sent
        /// </summary>
		event SentData NotifySentData;

		/// <summary>
		/// Notifies data was received from the PLC and returns the bytes that were received
		/// </summary>
		event ReceivedData NotifyReceivedData;

		/// <summary>
		/// Opens a connection with the PLC
		/// </summary>
		bool Connect();

        /// <summary>
        /// Closes the connection with the PLC
        /// </summary>
        bool Disconnect();

        /// <summary>
        /// Sends the provided packet to the PLC
        /// </summary>
        /// <param name="packet">The packet to send</param>
        bool SendData(IFinsPacket packet);

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
