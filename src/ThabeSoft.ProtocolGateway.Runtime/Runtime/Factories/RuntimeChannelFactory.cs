using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime.Factories;


/// <summary>
/// 运行时通道工厂
/// </summary>
internal sealed class RuntimeChannelFactory(IEnumerable<IRuntimeChannelProvider> providers) : IRuntimeChannelFactory
{
    private readonly IRuntimeChannelProvider[] _buffer = [.. providers];

    /// <summary>
    /// 从配置创建
    /// </summary>
    public Result<IRuntimeChannel> CreateFromConfig(IChannelConfig config)
    {
        var result = config.Validate();
        if (!result.IsSuccess) return result.Cast<IRuntimeChannel>();


        var provider = _buffer.FirstOrDefault(x => x.CanCreate(config));
        return provider?.Create(config) ?? Result.Error<IRuntimeChannel>($"无法创建通道句柄, 未知的配置类型: {config.GetType().Name}");
    }
}
