using System.Collections.Concurrent;
using System.Reactive.Linq;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Services;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 网关
/// </summary>
public sealed class Gateway(IChannelHandleFactory factory) : IConfigureGateway
{
    // 所有通道
    private readonly ConcurrentDictionary<ChannelName, IChannel> _channels = new();
    private readonly ConcurrentDictionary<ChannelName, IChannelHandle> _handles = new();
    // 通道运行时配置
    private readonly ConcurrentDictionary<ChannelName, ChannelRuntimeOptions> _channelOptions = [];


    // 添加
    public Result<IChannelHandle> AddOrUpdateChannel(IChannelConfig config)
    {
        return factory.GetHandle(config)
            .Tap(x => _handles[config.Name] = x);
    }
    // 添加
    public Result AddChannel(ChannelName name, IChannel channel)
    {
        if (_channels.TryGetValue(name, out _))
        {
            return Result.InvalidOperation($"无法添加通道, 名称已存在: {name}");
        }

        _channels[name] = channel;
        _channelOptions[name] = new();

        return Result.Ok();
    }
    // 删除
    public Result RemoveChannel(ChannelName name)
    {
        _channelOptions.TryRemove(name, out _);

        if (!_channels.TryRemove(name, out _))
        {
            return Result.InvalidOperation("通道不存在, 删除失败");
        }

        return Result.Ok();
    }


    // 启用
    public Result ResumeChannel(ChannelName name)
    {
        return GetChannelOptions(name).Tap(x => x.IsSuspend = false);
    }
    // 禁用
    public Result SuspendChannel(ChannelName name)
    {
        return GetChannelOptions(name).Tap(x => x.IsSuspend = true);
    }



    // 读取
    public ValueTask<Result<TValue>> ReadAsync<TValue>(IRoutableTag<TValue> tag, CancellationToken cancellationToken = default)
        where TValue : unmanaged
    {
        // 是否启用
        if (_channelOptions.TryGetValue(tag.ChannelName, out var opts) || opts?.IsSuspend != true)
        {
            var result = Result.InvalidOperation<TValue>($"无法读取, 通道 [{tag.ChannelName}] 已禁用");
            return new ValueTask<Result<TValue>>(result);
        }

        return InternalReadAsync(tag, cancellationToken);
    }
    // 写入
    public ValueTask<Result> WriteAsync<TValue>(IRoutableTag<TValue> tag, TValue value, CancellationToken cancellationToken = default)
        where TValue : unmanaged
    {
        // 是否启用
        if (_channelOptions.TryGetValue(tag.ChannelName, out var opts) || opts?.IsSuspend != true)
        {
            var result = Result.InvalidOperation($"无法写入, 通道 [{tag.ChannelName}] 已禁用");
            return new ValueTask<Result>(result);
        }

        return InternalWriteAsync(tag, value, cancellationToken);
    }
    // 轮询
    public IObservable<Result<TValue>> Poll<TValue>(IRoutableTag<TValue> tag) where TValue : unmanaged
    {
        return Observable.Create<Result<TValue>>(async (observer, ct) =>
        {
            var interval = TimeSpan.FromMilliseconds(100);

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var options = GetChannelOptions(tag.ChannelName).Bind(x => x.IsSuspend);
                    if (!options.IsSuccess || options.Value)
                    {
                        await Task.Delay(10, ct);
                        continue;
                    }

                    var result = await InternalReadAsync(tag, ct);
                    observer.OnNext(result);
                }
                catch(OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    observer.OnNext(Result.InvalidData<TValue>(ex.Message));
                }

                await Task.Delay(interval, ct);
            }

            observer.OnCompleted();
        });
    }


    // 释放
    public void Dispose()
    {
        _channels.Clear();
        _channelOptions.Clear();
    }





    private ValueTask<Result<TValue>> InternalReadAsync<TValue>(IRoutableTag<TValue> tag, CancellationToken cancellationToken = default)
        where TValue : unmanaged
    {
        // 查询通道
        if (!_channels.TryGetValue(tag.ChannelName, out var channel))
        {
            var result = Result.InvalidOperation<TValue>($"无法读取, 未知的通道 [{tag.ChannelName}]");
            return new ValueTask<Result<TValue>>(result);
        }
        // 不支持读取
        if (channel is not IReadableChannel readable)
        {
            var result = Result.InvalidOperation<TValue>($"无法读取, 通道 [{tag.ChannelName}] 不支持操作");
            return new ValueTask<Result<TValue>>(result);
        }

        // 读取
        return readable.ReadAsync(tag, cancellationToken);
    }
    // 写入
    private ValueTask<Result> InternalWriteAsync<TValue>(IRoutableTag<TValue> tag, TValue value, CancellationToken cancellationToken = default)
        where TValue : unmanaged
    {
        // 查询通道
        if (!_channels.TryGetValue(tag.ChannelName, out var channel))
        {
            var result = Result.InvalidOperation($"无法写入, 未知的通道名称 [{tag.ChannelName}]");
            return new ValueTask<Result>(result);
        }
        // 不支持写入
        if (channel is not IWritableChannel writable)
        {
            var result = Result.InvalidOperation($"无法写入, 通道 [{tag.ChannelName}] 不支持操作");
            return new ValueTask<Result>(result);
        }

        // 写入
        return writable.WriteAsync(tag, value, cancellationToken);
    }

    // 获取通道配置
    private Result<ChannelRuntimeOptions> GetChannelOptions(ChannelName name)
    {
        if (!_channelOptions.TryGetValue(name, out var options))
        {
            return Result.InvalidOperation<ChannelRuntimeOptions>($"未查询到通道配置 [{name}]");
        }

        return Result.Ok(options);
    }
}