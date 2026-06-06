using System.Diagnostics;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using ThabeSoft.Lifecycle;
using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Linq;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime;


/// <summary>
/// 运行时网关
/// </summary>
public sealed class RuntimeGateway : LifecycleObject, IRuntimeGateway, IAsyncDisposable
{
    private readonly SemaphoreSlim _addLock = new(1, 1);
    private readonly Dictionary<ChannelName, IRuntimeChannel> _channels = [];

    // 配追
    public IGatewayConfig Config { get; internal set; } = default!;
    // 运行时通道 
    public IReadOnlyCollection<IRuntimeChannel> Channels { get; internal set; } = default!;

    // 私有构造
    private RuntimeGateway() { }


    #region --通道管理--

    /// <summary>
    /// 添加通道
    /// </summary>
    public Result AddChannel(IChannelConfig config)
    {
        using var _ = _addLock.Lock();

        if (_channels.ContainsKey(config.Name))
        {
            return Result.Error($"通道名称已存在: {config.Name}");
        }

        // 创建运行时通道
        var channel_result = RuntimeChannel.Create(config);
        if (channel_result.IsFailure) return channel_result;

        // 添加
        _channels[config.Name] = channel_result.Value;

        return Result.Success();
    }
    public async ValueTask<Result> StartChannelAsync(ChannelName name, CancellationToken cancellationToken)
    {
        if (!_channels.TryGetValue(name, out var channel))
        {
            return Result.Error($"启动失败, 通道不存在 [{name}]");
        }

        return await channel.StartAsync(cancellationToken);
    }
    public async ValueTask<Result> StopChannelAsync(ChannelName name, CancellationToken cancellationToken)
    {
        if (!_channels.TryGetValue(name, out var channel))
        {
            return Result.Error($"停止失败, 通道不存在 [{name}]");
        }

        return await channel.StartAsync(cancellationToken);
    }
    public async ValueTask<Result> RemoveChannelAsync(ChannelName name)
    {
        if (!_channels.TryGetValue(name, out var channel))
        {
            return Result.Error($"删除失败, 通道不存在 [{name}]");
        }

        _channels.Remove(name);
        await channel.DisposeAsync();

        return Result.Success();
    }

    #endregion

    #region --标签分发--

    public ValueTask<Result<TValue>> ReadAsync<TValue>(IRoutableTag<TValue> tag, CancellationToken cancellationToken = default)
        where TValue : unmanaged
    {
        // 查询通道
        if (!_channels.TryGetValue(tag.ChannelName, out var channel))
        {
            var result = Result.Error<TValue>($"无法读取, 未知的通道 [{tag.ChannelName}]");
            return new ValueTask<Result<TValue>>(result);
        }
        // 未启用
        if (!channel.IsEnable)
        {
            var result = Result.Warning<TValue>($"无法读取, 通道未启用 [{tag.ChannelName}]");
            return new ValueTask<Result<TValue>>(result);
        }
        // 不支持读取
        if (channel is not IReadableChannel readable)
        {
            var result = Result.Error<TValue>($"无法读取, 通道 [{tag.ChannelName}] 不支持操作");
            return new ValueTask<Result<TValue>>(result);
        }

        // 读取
        return readable.ReadAsync(tag, cancellationToken);
    }
    public ValueTask<Result> WriteAsync<TValue>(IRoutableTag<TValue> tag, TValue value, CancellationToken cancellationToken = default)
        where TValue : unmanaged
    {
        // 查询通道
        if (!_channels.TryGetValue(tag.ChannelName, out var channel))
        {
            var result = Result.Error($"无法写入, 未知的通道名称 [{tag.ChannelName}]");
            return new ValueTask<Result>(result);
        }
        // 未启用
        if (!channel.IsEnable)
        {
            var result = Result.Warning($"无法写入, 通道未启用 [{tag.ChannelName}]");
            return new ValueTask<Result>(result);
        }
        // 不支持写入
        if (channel is not IWritableChannel writable)
        {
            var result = Result.Error($"无法写入, 通道 [{tag.ChannelName}] 不支持操作");
            return new ValueTask<Result>(result);
        }

        // 写入
        return writable.WriteAsync(tag, value, cancellationToken);
    }
    public IObservable<Result<TValue>> Poll<TValue>(IRoutableTag<TValue> tag) where TValue : unmanaged
    {
        return Observable.Create<Result<TValue>>(async (observer, ct) =>
        {
            var interval = TimeSpan.FromMilliseconds(100);

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    // 查询通道
                    if (!_channels.TryGetValue(tag.ChannelName, out var channel))
                    {
                        observer.OnNext(Result.Error<TValue>($"无法读取, 未知的通道 [{tag.ChannelName}]"));
                        continue;
                    }
                    // 未启用
                    if (!channel.IsEnable)
                    {
                        observer.OnNext(Result.Warning<TValue>($"无法读取, 通道未启用 [{tag.ChannelName}]"));
                        continue;
                    }
                    // 不支持读取
                    if (channel is not IReadableChannel readable)
                    {
                        observer.OnNext(Result.Error<TValue>($"无法读取, 通道 [{tag.ChannelName}] 不支持操作"));
                        continue;
                    }

                    // 通知
                    observer.OnNext(await readable.ReadAsync(tag, ct));
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    observer.OnNext(Result.Error<TValue>(ex.Message));
                }

                await Task.Delay(interval, ct);
            }

            observer.OnCompleted();
        });
    }

    #endregion


    /// <summary>
    /// 启动网关
    /// </summary>
    protected override async ValueTask<Result> StartProcessAsync(CancellationToken cancellationToken)
    {
        List<string> error_message = [];

        foreach (var channel in _channels.Values.ToArray())
        {
            var result = await channel.StartAsync(cancellationToken);
            if (result.IsProblem) error_message.Add(result.Message!);
        }

        if (error_message.Count != 0)
        {
            var message = string.Join(Environment.NewLine, error_message);
            return Result.Warning($"部分通道启动失败{Environment.NewLine}{message}");
        }

        return Result.Success();
    }
    /// <summary>
    /// 停止网关
    /// </summary>
    protected override async ValueTask<Result> StopProcessAsync(CancellationToken cancellationToken)
    {
        List<string> error_message = [];

        foreach (var i in _channels.Values.ToArray())
        {
            var result = await i.StopAsync(cancellationToken);
            if (result.IsProblem) error_message.Add(result.Message!);
        }

        if (error_message.Count != 0)
        {
            var message = string.Join(Environment.NewLine, error_message);
            return Result.Warning($"部分通道停止失败{Environment.NewLine}{message}");
        }

        return Result.Success();
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    protected override async ValueTask DisposeProcessAsync()
    {
        StringBuilder errror_messages = new();

        foreach (var i in _channels.Values.ToArray())
        {
            var result = await i.StopAsync();
            if (result.IsProblem) errror_messages.AppendLine(result.Message!);

            if (i is IDisposable disposable)
            {
                disposable.Dispose();
                continue;
            }

            if (i is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
        }

        Debug.WriteLineIf(errror_messages.Length != 0, errror_messages);
    }




    #region --工厂--


    public static Result<RuntimeGateway> Create(IGatewayConfig config)
    {
        return config.Channels
            .Select(RuntimeChannel.Create)
            .Merge()
            .Map (x => new RuntimeGateway() { Config = config, Channels = [.. x] });

        //bool has_warning = false;
        //StringBuilder warning_message = new();
        //List<IRuntimeChannel> channels = [];

        //var fuck = from i in config.Channels
        //           let result = RuntimeChannel.Create(i)
        //           select (has_error: !result.IsFailure, result);


        //foreach (var i in config.Channels)
        //{
        //    var result = RuntimeChannel.Create(i);
        //    if (result.IsFailure)
        //    {
        //        warning_message.AppendLine(result.Message);
        //        continue;
        //    }

        //    has_warning = true;
        //    channels.Add(result.Value);
        //}


        //var value = new RuntimeGateway() { Config = config, Channels = channels };

        //if (has_warning)
        //{
        //    var message = $"通道没有完全加载成功{Environment.NewLine}{warning_message}";
        //    return Result.Warning<RuntimeGateway>(message, value);
        //}
        //else
        //{
        //    return Result.Success<RuntimeGateway>(value);
        //}
    }

    #endregion
} 