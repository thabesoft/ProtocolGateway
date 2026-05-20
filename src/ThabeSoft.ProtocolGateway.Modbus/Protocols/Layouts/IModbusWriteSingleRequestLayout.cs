namespace ThabeSoft.ProtocolGateway.Protocols.Layouts;


/// <summary>
/// Rtu写单个值帧布局
/// </summary>
public interface IModbusWriteSingleRequestLayout : IModbusRequestLayout
{
    /// <summary>值范围</summary>
    Range ValueRange { get; }
}