using System.Collections.Concurrent;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 网关
/// </summary>
public sealed class Gateway : IGateway
{
    // 所有通道
    private readonly ConcurrentDictionary<ChannelName, IChannel> _channels = new();
    // 通道运行时配置
    private readonly ConcurrentDictionary<ChannelName, ChannelRuntimeOptions> _channelOptions = [];
    // 通道管理器
    private readonly ConcurrentDictionary<ChannelName, IChannelManager> _channelManagers = [];


    // 添加
    public Result AddChannel(ChannelName name, IChannel channel)
    {
        if (_channels.TryGetValue(name, out _))
        {
            return Result.InvalidOperation($"无法添加通道, 名称已存在: {name}");
        }

        _channels[name] = channel;
        _channelOptions[name] = new();
        _channelManagers[name] = new ChannelManager(this, name);

        return Result.Ok();
    }
    // 删除
    public Result RemoveChannel(ChannelName name)
    {
        _channelOptions.TryRemove(name, out _);
        _channelManagers.TryRemove(name, out _);


        if (!_channels.TryRemove(name, out _))
        {
            return Result.InvalidOperation("通道不存在, 删除失败");
        }

        return Result.Ok();
    }
    // 获取
    public Result<IChannelManager> GetChannel(ChannelName name)
    {
        if (!_channelManagers.TryGetValue(name, out var manager))
        {
            return Result.InvalidOperation<IChannelManager>($"未查询到通道管理器 [{name}]");
        }

        return Result.Ok(manager);
    }
    // 读取
    public ValueTask<Result<TValue>> ReadAsync<TValue>(IRoutableTag<TValue> tag, CancellationToken cancellationToken = default)
        where TValue : unmanaged
    {
        // 是否启用
        if (_channelOptions.TryGetValue(tag.ChannelName, out var opts) && !opts.IsEnabled)
        {
            var result = Result.InvalidOperation<TValue>($"无法读取, 通道 [{tag.ChannelName}] 已禁用");
            return new ValueTask<Result<TValue>>(result);
        }

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
    public ValueTask<Result> WriteAsync<TValue>(IRoutableTag<TValue> tag, TValue value, CancellationToken cancellationToken = default)
        where TValue : unmanaged
    {
        // 是否启用
        if (_channelOptions.TryGetValue(tag.ChannelName, out var opts) && !opts.IsEnabled)
        {
            var result = Result.InvalidOperation($"无法写入, 通道 [{tag.ChannelName}] 已禁用");
            return new ValueTask<Result>(result);
        }

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
    // 释放
    public void Dispose()
    {
        _channels.Clear();
        _channelOptions.Clear();
        _channelManagers.Clear();
    }


    // 启用
    public Result EnableChannel(ChannelName name)
    {
        return GetChannelOptions(name).Tap(x => x.IsEnabled = true);
    }
    // 禁用
    public Result DisableChannel(ChannelName name)
    {
        return GetChannelOptions(name).Tap(x => x.IsEnabled = false);
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