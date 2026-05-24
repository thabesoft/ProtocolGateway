using System.Linq.Expressions;
using ThabeSoft.ProtocolGateway.Tags;

namespace ThabeSoft.ProtocolGateway.Builders;

/// <summary>
/// 设备配置构建器
/// </summary>
/// <typeparam name="TDevice">设备信息</typeparam>
public interface IDeviceOptionsBuilders<TDevice>
{
    IDeviceOptionsBuilders<TDevice> Connect();

    IDeviceOptionsBuilders<TDevice> Tag<TValue>(
        Expression<Func<TDevice, TValue>> valueSelector,
        Action<ITagBuilder<TValue>> optionsAction) where TValue : unmanaged;
}
