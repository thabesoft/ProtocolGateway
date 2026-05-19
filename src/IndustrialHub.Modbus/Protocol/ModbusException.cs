namespace IndustrialHub.Modbus.Protocol;


/// <summary>
/// Modbus异常
/// </summary>
/// <param name="slaveId"></param>
/// <param name="functionCode"></param>
/// <param name="errorCode"></param>
public sealed class ModbusException(byte slaveId, byte functionCode, byte errorCode) : Exception
{
    public byte SlaveId { get; } = slaveId;
    public ErrorFunctionCode FunctionCode { get; } = functionCode;
    public byte ErrorCode { get; } = errorCode;
}
