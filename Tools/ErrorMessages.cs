namespace PLC_Omron_Standard.Tools
{
	/// <summary>
	/// Stores standard error messages
	/// </summary>
	internal static class ErrorMessages
	{
		#region User Methods

		/// <summary>
		/// Returns an error message indicating the PLC is not connected.
		/// </summary>
		internal static string PlcNotConnected => "PLC is not connected.";

		/// <summary>
		/// Returns an error message indiciating no data was received on a PLC read.
		/// </summary>
		internal static string NoDataReceived => "Did not receive data from PLC on read.";

		/// <summary>
		/// Returns an error message indicating not enough data was received on a PLC read.
		/// </summary>
		/// <param name="expected">The expected amount of bytes from the PLC.</param>
		/// <param name="actual">The actual amount of bytes from the PLC.</param>
		internal static string NotEnoughDataRecieved(int expected, int actual) => string.Format("Did not receive enough data from PLC on read. Expected length is {0} bytes, actual length was {1}.", expected, actual);

		/// <summary>
		/// Returns an error message indicating too much data was received from a PLC read.
		/// </summary>
		/// <param name="expected">The expected amount of bytes from the PLC.</param>
		/// <param name="actual">The actual amount of bytes from the PLC.</param>
		internal static string ReceivedTooMuchData(int expected, int actual) => string.Format("Received too much data from PLC on read. Expected length is {0} bytes, actual length was {1}.", expected, actual);

		#endregion

		#region TCP UDP

		/// <summary>
		/// Returns an error message indicating the provided local node was invalid.
		/// </summary>
		internal static string InvalidLocalUdpNode => "Must specify local node for UDP connections.";

		/// <summary>
		/// Returns an error message indicating the provided remote node was invalid.
		/// </summary>
		internal static string InvalidRemoteUdpNode => "Must specify remote node for UDP connections.";

		/// <summary>
		/// Returns an error message indicating the TCP header was invalid.
		/// </summary>
		/// <param name="action">The function being requested from the PLC.</param>
		/// <param name="header">The first 4 bytes of the TCP header.</param>
		internal static string InvalidTcpHeader(string action, byte[] header) => string.Format("TCP FINS header from PLC for {0} was invalid. Expected F I N S, received {1} {2} {3} {4}", action, (char)header[0], (char)header[1], (char)header[2], (char)header[3]);

		/// <summary>
		/// Returns an error message indicating the TCP header was not the corect length.
		/// </summary>
		/// <param name="action">The function being requested from the PLC.</param>
		/// <param name="actual">The actual amount of bytes from the PLC.</param>
		internal static string InvalidTcpHeader(string action, int actual) => string.Format("TCP FINS header from PLC for {0} was invalid. Expected length is 16 bytes, actual length was {1}.", action, actual);

		#endregion

		#region Memory Area Read and Write

		/// <summary>
		/// Returns an error message indicating writing to the PLC failed.
		/// </summary>
		internal static string FailedWritingToPlc => "Failed writing to PLC memory area.";

		/// <summary>
		/// Returns an error message indicating requesting data from the PLC failed.
		/// </summary>
		internal static string FailedRequestingPlcRead => "Failed to request data from PLC for memory area read.";

		/// <summary>
		/// Returns an error message indicating no confirmation response was received while writing to the PLC.
		/// </summary>
		/// <param name="action">The function being requested from the PLC.</param>
		internal static string NoResponse(string action) => string.Format("Failed to receive response from PLC for {0}.", action);

		/// <summary>
		/// Returns an error message indicating the FINS header was incorrect while writing to the PLC.
		/// </summary>
		/// <param name="action">The function being requested from the PLC.</param>
		/// <param name="actual">The actual amount of bytes from the PLC.</param>
		internal static string InvalidFinsHeader(string action, int actual) => string.Format("Did not receive correct FINS header during {0}. Expected length is 14 bytes, actual length was {1}.", action, actual);

		#endregion
	}
}
