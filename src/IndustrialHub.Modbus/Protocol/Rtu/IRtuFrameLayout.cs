namespace IndustrialHub.Modbus.Protocol.Rtu;

/// <summary>
/// Rtu帧布局
/// </summary>
public interface IRtuFrameLayout : IFrameLayout
{
    /// <summary>Crc范围</summary>
    Range CrcRange { get; }
}