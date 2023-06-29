namespace PLC_Omron_Standard.Models
{
	/// <summary>
	/// Defines standard responses that are received from Omron PLCs over FINS
	/// </summary>
	internal class ResponseCodes
	{
		/// <summary>
		/// The code from the PLC
		/// </summary>
		public byte Code { get; set; }

		/// <summary>
		/// The message associated with the code
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Specifies whether this code represents and error
		/// </summary>
		public bool IsError { get; set; }
	}
}
