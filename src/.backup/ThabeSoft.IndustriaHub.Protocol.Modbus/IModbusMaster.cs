namespace ThabeSoft.IndustriaHub.Protocol;


/// <summary>
/// Modbus 主站
/// </summary>
public interface IModbusMaster
{
    /// <summary>
    /// 读线圈
    /// </summary>
    /// <param name="address">地址</param>
    /// <param name="quantity">数量</param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task<bool[]> ReadColisAsync(byte slaveId, ushort address, ushort quantity, CancellationToken cancellation = default); 
}