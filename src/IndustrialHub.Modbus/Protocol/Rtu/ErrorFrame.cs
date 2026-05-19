using System.Diagnostics;

namespace IndustrialHub.Modbus.Protocol.Rtu;


/// <summary>
/// 错误帧
/// </summary>
public readonly struct ErrorFrame
{
    public static ErrorFrame Empty => default;


    public readonly byte SlaveId;
    public readonly ErrorFunctionCode FunctionCode;
    public readonly byte ErrorCode;


    [Obsolete("优先调用工厂方法 TryUnpack/TryCreate，除非你知道自己在做什么！")]
    internal ErrorFrame(byte slaveId, ErrorFunctionCode errorFunctionCode, byte errorCode)
    {
        SlaveId = slaveId;
        FunctionCode = errorFunctionCode;
        ErrorCode = errorCode;
    }
}