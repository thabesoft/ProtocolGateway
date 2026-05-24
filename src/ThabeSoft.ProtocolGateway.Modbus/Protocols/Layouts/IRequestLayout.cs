namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// Modbus 请求帧布局
/// </summary>
public interface IRequestLayout : ILayout
{

    /// <summary>地址范围</summary>
    Range AddressRange { get; }

    /// <summary>内容范围</summary>
    Range PayloadRange { get; }
}