using System.ComponentModel;
using ThabeSoft.Primitives;

namespace ThabeSoft.Lifecycle;

/// <summary>
/// 一个可通知状态的启动对象
/// </summary>
public abstract class LifecycleObject : ILifecycle
{
    // 线程锁
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// 状态改变
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// 当前状态
    /// </summary>
    public LifecycleState State
    {
        get;
        private set
        {
            if (value == field) return;
            field = value;
            OnStateChanged();
        }
    }

    /// <summary>
    /// 是否在运行
    /// </summary>
    public bool IsRunning => State == LifecycleState.Running;
    /// <summary>
    /// 是否已停止
    /// </summary>
    public bool IsStopped => State == LifecycleState.Stopped;


    // 启动
    public async ValueTask<Result> StartAsync(CancellationToken cancellationToken)
    {
        using var _ = await _lock.LockAsync(cancellationToken);

        try
        {
            // 是否能启动
            var can_start_result = CanStart();
            if (can_start_result.IsFailure)
            {
                State = LifecycleState.Faulted;
                return can_start_result;
            }

            // 启动中
            State = LifecycleState.Starting;

            // 启动结果
            var start_result = await StartProcessAsync(cancellationToken);
            State = start_result.IsFailure ? LifecycleState.Faulted : LifecycleState.Running;

            // 返回结果
            return start_result;
        }
        catch (Exception ex)
        {
            State = LifecycleState.Faulted;
            return Result.Error($"启动失败, {ex.Message}");
        }
    }
    // 停止
    public async ValueTask<Result> StopAsync(CancellationToken cancellationToken)
    {
        using var _ = await _lock.LockAsync(cancellationToken);

        try
        {
            // 是否能停止
            var can_stop_result = CanStop();
            if (can_stop_result.IsFailure)
            {
                State = LifecycleState.Faulted;
                return can_stop_result;
            }

            // 停止中
            State = LifecycleState.Stopping;

            // 停止结果
            var stop_result = await StopProcessAsync(cancellationToken);
            State = stop_result.IsFailure ? LifecycleState.Faulted : LifecycleState.Stopped;

            // 返回结果
            return stop_result;
        }
        catch (Exception ex)
        {
            State = LifecycleState.Faulted;
            return Result.Error($"停止失败, {ex.Message}");
        }
    }
    // 释放
    public async ValueTask DisposeAsync()
    {
        if (State == LifecycleState.Disposed) return;

        using var _ = await _lock.LockAsync();

        await DisposeProcessAsync();
        State = LifecycleState.Disposed;

        GC.SuppressFinalize(this);
    }



    /// <summary>
    /// 是否能启动
    /// </summary>
    protected virtual Result CanStart()
    {
        if (State is LifecycleState.Starting or LifecycleState.Running or LifecycleState.Disposed)
        {
            return Result.Error($"当前状态不能启动: {State}");
        }

        return Result.Success();
    }

    /// <summary>
    /// 是否能停止
    /// </summary>
    protected virtual Result CanStop()
    {
        if (State is LifecycleState.Stopping or LifecycleState.Stopped or LifecycleState.Disposed)
        {
            return Result.Error($"当前状态不能启动: {State}");
        }

        return Result.Success();
    }

    /// <summary>
    /// 触发状态改变通知
    /// </summary>
    protected void OnStateChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
    }

    /// <summary>
    /// 获取生命周期锁，确保状态转换的原子性
    /// </summary>
    protected async Task<IDisposable> LockAsync(CancellationToken cancellationToken = default)
    {
        return await _lock.LockAsync(cancellationToken);
    }

    /// <summary>
    /// 获取生命周期锁，确保状态转换的原子性
    /// </summary>
    protected IDisposable Lock()
    {
        return _lock.Lock();
    }


    /// <summary>
    /// 启动过程
    /// </summary>
    protected abstract ValueTask<Result> StartProcessAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 停止过程
    /// </summary>
    protected abstract ValueTask<Result> StopProcessAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 释放过程
    /// </summary>
    protected virtual ValueTask DisposeProcessAsync() => default;
}
