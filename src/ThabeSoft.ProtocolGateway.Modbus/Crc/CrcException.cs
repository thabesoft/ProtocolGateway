namespace ThabeSoft.IndustrialHub.Modbus.Crc;

/// <summary>
/// Crc 异常
/// </summary>
[Obsolete]
public class CrcException : Exception
{
    public CrcException()
    {
    }

    public CrcException(string message) : base(message)
    {
    }

    public CrcException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
