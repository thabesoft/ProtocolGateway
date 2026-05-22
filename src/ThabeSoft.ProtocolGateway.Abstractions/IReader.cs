using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 数据读取器
/// </summary>
public interface IReader
{
    /// <summary>
    /// 从指定标签信息读取数据
    /// </summary>
    /// <typeparam name="TValue">具体值类型</typeparam>
    /// <param name="tagInfo">标签信息</param>
    ValueTask<Result<TValue>> ReadAsync<TValue>(
            ITag<TValue> tagInfo,
            CancellationToken cancellationToken = default
        ) where TValue : unmanaged;
}