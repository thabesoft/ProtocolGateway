using System.ComponentModel;
using ThabeSoft.Primitives;

namespace ThabeSoft.Startable;


/// <summary>
/// 生命周期
/// </summary>
public interface ILifecycle : IAsyncDisposable, INotifyPropertyChanged
{
    /// <summary>
    /// 生命周期状态
    /// </summary>
    LifecycleState State { get; }

    /// <summary>
    /// 启动
    /// </summary>
    ValueTask<Result> StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 停止
    /// </summary>
    ValueTask<Result> StopAsync(CancellationToken cancellationToken = default);
}