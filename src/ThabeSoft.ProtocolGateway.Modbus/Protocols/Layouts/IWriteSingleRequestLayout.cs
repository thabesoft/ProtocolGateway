namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// Rtu写单个值帧布局
/// </summary>
public interface IWriteSingleRequestLayout : IRequestLayout
{
    /// <summary>值范围</summary>
    Range ValueRange { get; }
}