using System.Linq.Expressions;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 数据订阅器
/// </summary>
public interface ISubscriber
{
    /// <summary>
    /// 从指定标签信息订阅数据
    /// </summary>
    /// <typeparam name="TValue">具体值类型</typeparam>
    /// <param name="tag">标签信息</param>
    /// <param name="callback">新值回调</param>
    IDisposable Subscribe<TValue>(
            ITag<TValue> tag,
            Action<TValue> callback
        ) where TValue : unmanaged;
}