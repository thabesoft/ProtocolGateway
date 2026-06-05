using System.ComponentModel;
using ThabeSoft.Primitives;

namespace ThabeSoft.Lifecycle;


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

public static class LifecycleExtensions
{
    extension(ILifecycle lifecycle)
    {
        /// <summary>
        /// 是否已运行
        /// </summary>
        public bool IsRunning => lifecycle.State == LifecycleState.Running;

        /// <summary>
        /// 是否已暂停
        /// </summary>
        public bool IsStopped => lifecycle.State == LifecycleState.Stopped;

        /// <summary>
        /// 是否可以启动
        /// </summary>
        public bool CanStart => lifecycle.State is not (LifecycleState.Running or LifecycleState.Disposed);

        /// <summary>
        /// 是否可以停止
        /// </summary>
        public bool CanStop => lifecycle.State == LifecycleState.Running;
    }
}