using ThabeSoft.Primitives;

namespace ThabeSoft.Startable;

/// <summary>
/// 带事件的可启动对象
/// </summary>
public interface INotifyStartable : IObservableState, IStartable;


/// <summary>
/// 可观察的启动状态对象
/// </summary>
public abstract class ObservableStateObject : IObservableState
{
    public event Action<StartableState>? StateChanged;

    public StartableState State
    {
        get;
        private set
        {
            if (value == field) return;
            field = value;
            OnStateChanged();
        }
    }

    protected void OnStateChanged()
    {
        StateChanged?.Invoke(State);
    }
}


/// <summary>
/// 一个可通知状态的启动对象
/// </summary>
public abstract class StartableObject : INotifyStartable
{
    // 线程锁
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// 状态改变
    /// </summary>
    public event Action<StartableState>? StateChanged;

    /// <summary>
    /// 当前状态
    /// </summary>
    public StartableState State
    {
        get;
        private set
        {
            if (value == field) return;
            field = value;
            OnStateChanged();
        }
    }

    // 启动
    async ValueTask<Result> IStartable.StartAsync(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            // 是否能启动
            var can_start_result = CanStart();
            if (can_start_result.IsFailure)
            {
                State = StartableState.Faulted;
                return can_start_result;
            }

            // 启动中
            State = StartableState.Starting;

            // 启动结果
            var start_result = await StartAsync(cancellationToken);
            State = can_start_result.IsSuccess ? StartableState.Running : StartableState.Faulted;

            // 返回结果
            return can_start_result;
        }
        catch (Exception ex)
        {
            State = StartableState.Faulted;
            return Result.Error($"启动失败, {ex.Message}");
        }
        finally
        {
            _lock.Release();
        }
    }
    // 停止
    async ValueTask<Result> IStartable.StopAsync(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            // 是否能停止
            var can_stop_result = CanStop();
            if (can_stop_result.IsFailure)
            {
                State = StartableState.Faulted;
                return can_stop_result;
            }

            // 停止中
            State = StartableState.Stopping;

            // 停止结果
            var start_result = await StopAsync(cancellationToken);
            State = can_stop_result.IsSuccess ? StartableState.Stopped : StartableState.Faulted;

            // 返回结果
            return can_stop_result;
        }
        catch (Exception ex)
        {
            State = StartableState.Faulted;
            return Result.Error($"停止失败, {ex.Message}");
        }
        finally
        {
            _lock.Release();
        }
    }
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (State == StartableState.Disposed) return;

        await _lock.WaitAsync();
        try
        {
            await DisposeAsync();
            State = StartableState.Disposed;
        }
        finally
        {
            _lock.Release();
        }

        GC.SuppressFinalize(this);
    }



    /// <summary>
    /// 是否能启动
    /// </summary>
    protected virtual Result CanStart()
    {
        if (State is StartableState.Starting or StartableState.Running or StartableState.Disposed)
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
        if (State is StartableState.Stopping or StartableState.Stopped or StartableState.Disposed)
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
        StateChanged?.Invoke(State);
    }


    /// <summary>
    /// 启动过程
    /// </summary>
    protected abstract ValueTask<Result> StartAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 停止过程
    /// </summary>
    protected abstract ValueTask<Result> StopAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 释放过程
    /// </summary>
    protected virtual ValueTask DisposeAsync() => default;
}
