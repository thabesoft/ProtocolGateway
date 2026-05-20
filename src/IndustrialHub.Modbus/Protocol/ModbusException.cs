namespace IndustrialHub.Modbus.Protocol;


/// <summary>
/// Modbus异常
/// </summary>
public sealed class ModbusException: Exception
{
    public byte SlaveId { get; set; }
    public ErrorFunctionCode FunctionCode { get; set; }
    public byte ErrorCode { get; set; }


    public ModbusException()
    {
    }

    public ModbusException(string message) : base(message)
    {
    }

    public ModbusException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
