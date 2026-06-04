using System.Collections.Concurrent;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Runtime.Internal;

namespace ThabeSoft.ProtocolGateway.Services.Channel;


///// <summary>
///// 通道运行时业务
///// </summary>
//public interface IChannelRuntimeService
//{
//    /// <summary>
//    /// 加载并激活所有通道
//    /// </summary>
//    Task<Result<IReadOnlyList<ChannelRuntimeContext>>> LoadAndActivateAllAsync();

//    /// <summary>
//    /// 获取所有运行中的通道
//    /// </summary>
//    IReadOnlyList<ChannelRuntimeContext> ActiveChannels { get; }

//    /// <summary>
//    /// 事件：通道激活成功
//    /// </summary>
//    event EventHandler<ChannelRuntimeContext>? ChannelActivated;

//    /// <summary>
//    /// 事件：通道停用
//    /// </summary>
//    event EventHandler<ChannelName>? ChannelDeactivated;
//}


//internal sealed class ChannelRuntimeService : IChannelRuntimeService
//{
//    private readonly IGatewayConfigRepository _configRepository;
//    private readonly IRuntimeChannelFactory _handleFactory;
//    private readonly ConcurrentDictionary<ChannelName, ChannelRuntimeContext> _contexts = new();

//    public event EventHandler<ChannelRuntimeContext>? ChannelActivated;
//    public event EventHandler<ChannelName>? ChannelDeactivated;

//    public IReadOnlyList<ChannelRuntimeContext> ActiveChannels => _contexts.Values.ToList();

//    public ChannelRuntimeService(
//        IGatewayConfigRepository configRepository,
//        IRuntimeChannelFactory handleFactory)
//    {
//        _configRepository = configRepository;
//        _handleFactory = handleFactory;
//    }

//    public async Task<Result<IReadOnlyList<ChannelRuntimeContext>>> LoadAndActivateAllAsync()
//    {
//        var configs = await _configRepository.GetAllAsync();
//        var activated = new List<ChannelRuntimeContext>();

//        foreach (var config in configs)
//        {
//            var result = await ActivateChannelAsync(config);

//            if (result.IsSuccess)
//            {
//                activated.Add(result.Value);
//            }
//        }

//        return Result.Success<IReadOnlyList<ChannelRuntimeContext>>(activated);
//    }

//    private async Task<Result<ChannelRuntimeContext>> ActivateChannelAsync(ChannelConfig config)
//    {
//        // 检查是否已激活
//        if (_contexts.TryGetValue(config.Name, out var existing))
//        {
//            return Result.Success(existing);
//        }

//        // 创建 Handle
//        var handleResult = _handleFactory.CreateFromConfig(config);
//        if (!handleResult.IsSuccess)
//        {
//            return handleResult.Cast<ChannelRuntimeContext>();
//        }

//        var context = new ChannelRuntimeContext
//        {
//            Config = config,
//            Handle = handleResult.Value
//        };

//        _contexts[config.Name] = context;
//        ChannelActivated?.Invoke(this, context);

//        return Result.Success(context);
//    }

//    public async Task<Result> DeactivateChannelAsync(ChannelName name)
//    {
//        if (_contexts.TryRemove(name, out var context))
//        {
//            await context.Handle.StopAsync();
//             ChannelDeactivated?.Invoke(this, name);
//        }

//        return Result.Success();
//    }
//}