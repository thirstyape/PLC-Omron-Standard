using PLC_Omron_Standard.Models;
using System.Collections.Generic;
using System.Linq;

namespace PLC_Omron_Standard.Tools
{
	/// <summary>
	/// Stores standard response codes and provides lookup methods
	/// </summary>
	internal static class ResponseCodesDictionary
	{
		private static readonly List<ResponseCodes> ResponseCodes = new List<ResponseCodes>()
		{
			new ResponseCodes() { Code = 0x00, Message = "Ok", IsError = false },
			new ResponseCodes() { Code = 0x01, Message = "Invalid memory address parameter", IsError = true },
			new ResponseCodes() { Code = 0x02, Message = "Invalid or illegal command parameter", IsError = true },
			new ResponseCodes() { Code = 0x03, Message = "Response SID did not match", IsError = true },
			new ResponseCodes() { Code = 0x04, Message = "NSB did not respond to send request", IsError = true },
			new ResponseCodes() { Code = 0x05, Message = "Timed out no response", IsError = true },
			new ResponseCodes() { Code = 0x06, Message = "Timed out waiting for response", IsError = true },
			new ResponseCodes() { Code = 0x07, Message = "Bad received CRC", IsError = true },
			new ResponseCodes() { Code = 0x08, Message = "Unmatched message IDs", IsError = true },
			new ResponseCodes() { Code = 0x09, Message = "Unmatched command or response", IsError = true },
			new ResponseCodes() { Code = 0x0A, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x0B, Message = "Network address out of range", IsError = true },
			new ResponseCodes() { Code = 0x0C, Message = "Node address out of range", IsError = true },
			new ResponseCodes() { Code = 0x0D, Message = "Unit address out of range", IsError = true },
			new ResponseCodes() { Code = 0x0E, Message = "Invalid address parameter", IsError = true },
			new ResponseCodes() { Code = 0x0F, Message = "Timed out waiting for echo", IsError = true },
			new ResponseCodes() { Code = 0x10, Message = "Bad received FCS", IsError = true },
			new ResponseCodes() { Code = 0x11, Message = "Response from different host link unit", IsError = true },
			new ResponseCodes() { Code = 0x12, Message = "No valid response code", IsError = true },
			new ResponseCodes() { Code = 0x13, Message = "No FINS response packet", IsError = true },
			new ResponseCodes() { Code = 0x14, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x15, Message = "Local node not part of network", IsError = true },
			new ResponseCodes() { Code = 0x16, Message = "Token timeout, node number too high", IsError = true },
			new ResponseCodes() { Code = 0x17, Message = "Number of transmit retries exceeded", IsError = true },
			new ResponseCodes() { Code = 0x18, Message = "Max number of frames exceeded", IsError = true },
			new ResponseCodes() { Code = 0x19, Message = "Node number setting error", IsError = true },
			new ResponseCodes() { Code = 0x1A, Message = "Node number duplication error", IsError = true },
			new ResponseCodes() { Code = 0x1B, Message = "Destination node not part of network", IsError = true },
			new ResponseCodes() { Code = 0x1C, Message = "No node with node number specified", IsError = true },
			new ResponseCodes() { Code = 0x1D, Message = "Third node not part of network", IsError = true },
			new ResponseCodes() { Code = 0x1E, Message = "Busy error, destination node busy", IsError = true },
			new ResponseCodes() { Code = 0x1F, Message = "Response timeout, noise or watchdog", IsError = true },
			new ResponseCodes() { Code = 0x20, Message = "Error in communication controller", IsError = true },
			new ResponseCodes() { Code = 0x21, Message = "PLC error in destination node", IsError = true },
			new ResponseCodes() { Code = 0x22, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x23, Message = "Undefined command used", IsError = true },
			new ResponseCodes() { Code = 0x24, Message = "Cannot process command", IsError = true },
			new ResponseCodes() { Code = 0x25, Message = "Routing error", IsError = true },
			new ResponseCodes() { Code = 0x26, Message = "Command is too long", IsError = true },
			new ResponseCodes() { Code = 0x27, Message = "Command is too short", IsError = true },
			new ResponseCodes() { Code = 0x28, Message = "Specified data items differ from actual", IsError = true },
			new ResponseCodes() { Code = 0x29, Message = "Incorrect command format", IsError = true },
			new ResponseCodes() { Code = 0x2A, Message = "Incorrect header", IsError = true },
			new ResponseCodes() { Code = 0x2B, Message = "Memory area code error", IsError = true },
			new ResponseCodes() { Code = 0x2C, Message = "Access size specified is wrong", IsError = true },
			new ResponseCodes() { Code = 0x2D, Message = "First address is inaccessible", IsError = true },
			new ResponseCodes() { Code = 0x2E, Message = "Address range exceeded", IsError = true },
			new ResponseCodes() { Code = 0x2F, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x30, Message = "Non-existent program number specified", IsError = true },
			new ResponseCodes() { Code = 0x31, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x32, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x33, Message = "Data size in command is wrong", IsError = true },
			new ResponseCodes() { Code = 0x34, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x35, Message = "Response block too long", IsError = true },
			new ResponseCodes() { Code = 0x36, Message = "Incorrect parameter code", IsError = true },
			new ResponseCodes() { Code = 0x37, Message = "Program area protected", IsError = true },
			new ResponseCodes() { Code = 0x38, Message = "Registered table error", IsError = true },
			new ResponseCodes() { Code = 0x39, Message = "Area read-only or write protected", IsError = true },
			new ResponseCodes() { Code = 0x3A, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x3B, Message = "Mode is wrong", IsError = true },
			new ResponseCodes() { Code = 0x3C, Message = "Mode is wrong (Running)", IsError = true },
			new ResponseCodes() { Code = 0x3D, Message = "PLC is in Program mode", IsError = true },
			new ResponseCodes() { Code = 0x3E, Message = "PLC is in Debug mode", IsError = true },
			new ResponseCodes() { Code = 0x3F, Message = "PLC is in Monitor mode", IsError = true },
			new ResponseCodes() { Code = 0x40, Message = "PLC is in Run mode", IsError = false },
			new ResponseCodes() { Code = 0x41, Message = "Specified node is not control node", IsError = true },
			new ResponseCodes() { Code = 0x42, Message = "Specified memory does not exist", IsError = true },
			new ResponseCodes() { Code = 0x43, Message = "No clock exists", IsError = true },
			new ResponseCodes() { Code = 0x44, Message = "Data link table error", IsError = true },
			new ResponseCodes() { Code = 0x45, Message = "Unit error", IsError = true },
			new ResponseCodes() { Code = 0x46, Message = "Command error", IsError = true },
			new ResponseCodes() { Code = 0x47, Message = "Destination address setting error", IsError = true },
			new ResponseCodes() { Code = 0x48, Message = "No routing tables", IsError = true },
			new ResponseCodes() { Code = 0x49, Message = "Routing table error", IsError = true },
			new ResponseCodes() { Code = 0x4A, Message = "Too many relays", IsError = true },
			new ResponseCodes() { Code = 0x4B, Message = "The header is not FINS", IsError = true },
			new ResponseCodes() { Code = 0x4C, Message = "The data length is too long", IsError = true },
			new ResponseCodes() { Code = 0x4D, Message = "The command is not supported", IsError = true },
			new ResponseCodes() { Code = 0x4E, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x4F, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x50, Message = "Timed out waiting for port semaphore", IsError = true },
			new ResponseCodes() { Code = 0x51, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x52, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x53, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x54, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x55, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x56, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x57, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x58, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x59, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x5A, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x5B, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x5C, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x5D, Message = "", IsError = false },
			new ResponseCodes() { Code = 0x5E, Message = "All connections are in use", IsError = true },
			new ResponseCodes() { Code = 0x5F, Message = "The specified node is already connected", IsError = true },
			new ResponseCodes() { Code = 0x60, Message = "Attempt to access protected node from unspecified IP", IsError = true },
			new ResponseCodes() { Code = 0x61, Message = "The client FINS node address is out of range", IsError = true },
			new ResponseCodes() { Code = 0x62, Message = "Same FINS node address is being used by client and server", IsError = true },
			new ResponseCodes() { Code = 0x63, Message = "No node addresses are available to allocate", IsError = true }
		};

		/// <summary>
		/// Returns the message for the provided code
		/// </summary>
		/// <param name="code">The code to lookup</param>
		public static string GetCodeMessage(byte code) => ResponseCodes.FirstOrDefault(x => x.Code == code)?.Message ?? "Code not found";

		/// <summary>
		/// Returns whether the provided code represents an error
		/// </summary>
		/// <param name="code">The code to lookup</param>
		public static bool IsError(byte code) => ResponseCodes.FirstOrDefault(x => x.Code == code)?.IsError ?? true;
	}
}
