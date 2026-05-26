namespace ThabeSoft.ProtocolGateway.Builders;

/// <summary>
/// 设备配置
/// </summary>
/// <typeparam name="TDevice">设备信息模型</typeparam>
public interface IDeviceOptions<TDevice>
{
    /// <summary>
    /// 获取标签信息
    /// </summary>
    /// <typeparam name="TValue">值类型</typeparam>
    /// <param name="name">标签名称</param>
    ITag<TValue> GetTag<TValue>(string name) where TValue : unmanaged;
}
