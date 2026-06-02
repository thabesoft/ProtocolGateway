using System.Collections.Concurrent;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Services.Channel;


/// <summary>
/// 通道运行时业务
/// </summary>
public interface IChannelRuntimeService
{
    /// <summary>
    /// 加载并激活所有通道
    /// </summary>
    Task<Result<IReadOnlyList<ChannelRuntimeContext>>> LoadAndActivateAllAsync();

    /// <summary>
    /// 获取所有运行中的通道
    /// </summary>
    IReadOnlyList<ChannelRuntimeContext> ActiveChannels { get; }

    /// <summary>
    /// 事件：通道激活成功
    /// </summary>
    event EventHandler<ChannelRuntimeContext>? ChannelActivated;

    /// <summary>
    /// 事件：通道停用
    /// </summary>
    event EventHandler<ChannelName>? ChannelDeactivated;
}


internal sealed class ChannelRuntimeService : IChannelRuntimeService
{
    private readonly IChannelConfigRepository _configRepository;
    private readonly IChannelHandleFactory _handleFactory;
    private readonly ConcurrentDictionary<ChannelName, ChannelRuntimeContext> _contexts = new();

    public event EventHandler<ChannelRuntimeContext>? ChannelActivated;
    public event EventHandler<ChannelName>? ChannelDeactivated;

    public IReadOnlyList<ChannelRuntimeContext> ActiveChannels => _contexts.Values.ToList();

    public ChannelRuntimeService(
        IChannelConfigRepository configRepository,
        IChannelHandleFactory handleFactory)
    {
        _configRepository = configRepository;
        _handleFactory = handleFactory;
    }

    public async Task<Result<IReadOnlyList<ChannelRuntimeContext>>> LoadAndActivateAllAsync()
    {
        var configs = await _configRepository.GetAllAsync();
        var activated = new List<ChannelRuntimeContext>();

        foreach (var config in configs)
        {
            var result = await ActivateChannelAsync(config);

            if (result.IsSuccess)
            {
                activated.Add(result.Value);
            }
        }

        return Result.Ok<IReadOnlyList<ChannelRuntimeContext>>(activated);
    }

    private async Task<Result<ChannelRuntimeContext>> ActivateChannelAsync(ChannelConfig config)
    {
        // 检查是否已激活
        if (_contexts.TryGetValue(config.Name, out var existing))
        {
            return Result.Ok(existing);
        }

        // 创建 Handle
        var handleResult = _handleFactory.GetHandle(config);
        if (!handleResult.IsSuccess)
        {
            return handleResult.PropagateError<ChannelRuntimeContext>();
        }

        var context = new ChannelRuntimeContext
        {
            Config = config,
            Handle = handleResult.Value
        };

        _contexts[config.Name] = context;
        ChannelActivated?.Invoke(this, context);

        return Result.Ok(context);
    }

    public async Task<Result> DeactivateChannelAsync(ChannelName name)
    {
        if (_contexts.TryRemove(name, out var context))
        {
            if (context.Handle.IsConnected)
            {
                await context.Handle.DisconnectAsync();
            }
            ChannelDeactivated?.Invoke(this, name);
        }

        return Result.Ok();
    }
}