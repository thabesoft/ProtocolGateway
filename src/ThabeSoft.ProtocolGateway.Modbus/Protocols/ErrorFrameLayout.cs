namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// 错误帧布局
/// </summary>
public readonly struct ErrorFrameLayout
{
    public static readonly int SlaveIdIndex;

    public static readonly int FunctionCodeIndex = 1;

    public static readonly int ErrorCodeIndex = 2;

    public static readonly Range CrcRange = new(3, 5);
}