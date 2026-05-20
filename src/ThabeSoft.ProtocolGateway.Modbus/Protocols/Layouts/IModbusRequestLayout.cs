namespace ThabeSoft.ProtocolGateway.Protocols.Layouts;


/// <summary>
/// Modbus 请求帧布局
/// </summary>
public interface IModbusRequestLayout
{
    /// <summary>从站Id索引</summary>
    int SlaveIdIndex { get; }

    /// <summary>功能码索引</summary>
    int FunctionCodeIndex { get; }

    /// <summary>地址范围</summary>
    Range AddressRange { get; }

    /// <summary>内容范围</summary>
    Range PayloadRange { get; }

    /// <summary>帧总长度</summary>
    int TotalLength { get; }
}
