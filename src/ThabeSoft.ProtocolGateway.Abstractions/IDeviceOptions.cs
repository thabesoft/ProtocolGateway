using System.Linq.Expressions;
using ThabeSoft.ProtocolGateway.Primitives;
using static ThabeSoft.ProtocolGateway.Test;

namespace ThabeSoft.ProtocolGateway;

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
    ITag<TValue> GetTagInfo<TValue>(string name);
}


/// <summary>
/// 设备配置构建器
/// </summary>
/// <typeparam name="TDevice">设备信息</typeparam>
public interface IDeviceOptionsBuilders<TDevice>
{
    IDeviceOptionsBuilders<TDevice> Connect();
    IDeviceOptionsBuilders<TDevice> Tag<TValue>(Expression<Func<TDevice, TValue>> valueSelector, Action<ITagInfoBuilder<TValue>> optionsAction);
}

/// <summary>
/// 标签信息构建器
/// </summary>
public interface ITagInfoBuilder<TValue> where TValue : unmanaged
{
    ITagInfoBuilder<TValue> Name(string name);
    ITagInfoBuilder<TValue> Address(string address);
    ITagInfoBuilder<TValue> Converter(IValueConverter<TValue> bytesConverter);

    ITag<TValue> Build();
}
