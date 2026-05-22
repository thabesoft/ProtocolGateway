using System.Linq.Expressions;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway;

/// <summary>
/// 数据写入器
/// </summary>
public interface IWriter
{
    /// <summary>
    /// 写入数据到指定标签信息
    /// </summary>
    /// <typeparam name="TValue">具体值类型</typeparam>
    /// <param name="tagInfo">标签信息</param>
    /// <param name="value">写入的值</param>
    ValueTask<Result> WriteAsync<TValue>(
            ITag<TValue> tagInfo,
            TValue value,
            CancellationToken cancellationToken = default
        ) where TValue : unmanaged;
}