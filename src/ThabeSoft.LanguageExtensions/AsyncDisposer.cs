namespace System;


/// <summary>
/// 异步释放器
/// </summary>
public static class AsyncDisposer
{
    public static AsyncDisposerTask Create(Func<Task> action) => new AsyncDisposerTask(action);
    public static AsyncDisposerTask<TState> Create<TState>(TState state, Func<TState, Task> action) => new(state, action);


    public static AsyncDisposerValueTask Create(Func<ValueTask> action) => new(action);
    public static AsyncDisposerValueTask<TState> Create<TState>(TState state, Func<TState, ValueTask> action) => new(state, action);
}


public readonly struct AsyncDisposerTask(Func<Task> action) : IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        return new ValueTask(action());
    }
}
public readonly struct AsyncDisposerTask<TState>(TState state, Func<TState, Task> action) : IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        return new ValueTask(action(state));
    }
}


public readonly struct AsyncDisposerValueTask(Func<ValueTask> action) : IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        return action();
    }
}
public readonly struct AsyncDisposerValueTask<TState>(TState state, Func<TState, ValueTask> action) : IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        return action(state);
    }
}