namespace PLC_Omron_Standard.Interfaces
{
    /// <summary>
    /// Defines properties and methods required for a FINS command packet
    /// </summary>
    internal interface IFinsPacket
    {
        byte[] ToArray();
    }
}
