using ThabeSoft.Primitives;

namespace ThabeSoft.Startable;


/// <summary>
/// 可以启动的
/// </summary>
public interface IStartable
{
    /// <summary>
    /// 状态
    /// </summary>
    StartableState State { get; }

    /// <summary>
    /// 启动
    /// </summary>
    ValueTask<Result> StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 停止
    /// </summary>
    ValueTask<Result> StopAsync(CancellationToken cancellationToken = default);
}