namespace System;



/// <summary>
/// 释放器
/// </summary>
/// <param name="action">释放过程</param>
public readonly struct Disposer(Action action) : IDisposable
{
    public void Dispose() => action();


    public static Disposer Create(Action action) => new(action);
    public static Disposer<TState> Create<TState>(TState state, Action<TState> action) => new Disposer<TState>(state, action);
}

/// <summary>
/// 包含一个自定义参数的释放器 (主要用于释放过程中传递参数, 避免闭包问题)
/// </summary>
/// <typeparam name="TState">自定义参数类型</typeparam>
/// <param name="action">释放过程</param>
/// <param name="state">自定义参数值</param>
public readonly struct Disposer<TState>(TState state, Action<TState> action) : IDisposable
{
    public void Dispose() => action(state);
}