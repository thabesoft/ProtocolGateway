namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// 读帧布局
/// </summary>
public interface IReadRequestLayout : IRequestLayout
{
    /// <summary>数量范围</summary>
    Range QuantityRange { get; }
}