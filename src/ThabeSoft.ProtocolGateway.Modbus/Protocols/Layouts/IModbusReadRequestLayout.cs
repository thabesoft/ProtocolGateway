namespace ThabeSoft.ProtocolGateway.Protocols.Layouts;


/// <summary>
/// 读帧布局
/// </summary>
public interface IModbusReadRequestLayout : IModbusRequestLayout
{
    /// <summary>数量范围</summary>
    Range QuantityRange { get; }
}