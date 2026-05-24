namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// 错误响应布局
/// </summary>
public interface IErrorResponseLayout
{
    public int SlaveIdIndex { get; }
    public int FunctionCodeIndex { get; }
    public int ErrorCodeIndex { get; }
}