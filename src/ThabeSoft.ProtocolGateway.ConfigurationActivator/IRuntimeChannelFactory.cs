using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 运行时通道工厂
/// </summary>
public interface IRuntimeChannelFactory
{
    /// <summary>
    /// 创建
    /// </summary>
    Result<IRuntimeChannel> Create(IChannelConfig config);
}


internal sealed class ChannelHandleFactory(IEnumerable<IChannelHandleProvider> providers) : IRuntimeChannelFactory
{
    private readonly IChannelHandleProvider[] _buffer = [.. providers];

    public Result<IRuntimeChannel> Create(IChannelConfig config)
    {
        var result = config.Validate();
        if (!result.IsSuccess) return result.Cast<IRuntimeChannel>();


        var provider = _buffer.FirstOrDefault(x => x.CanCreate(config));
        return provider?.Create(config) ?? Result.Error<IRuntimeChannel>($"无法创建通道句柄, 未知的配置类型: {config.GetType().Name}");
    }
}
