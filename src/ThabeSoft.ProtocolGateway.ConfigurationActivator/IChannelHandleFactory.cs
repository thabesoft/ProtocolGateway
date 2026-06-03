using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Handles;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 通道句柄工厂
/// </summary>
public interface IChannelHandleFactory
{
    /// <summary>
    /// 获取句柄
    /// </summary>
    Result<IChannelHandle> GetHandle(ChannelConfig config);
}


internal sealed class ChannelHandleFactory(IEnumerable<IChannelHandleProvider> providers) : IChannelHandleFactory
{
    private readonly IChannelHandleProvider[] _buffer = [.. providers];

    public Result<IChannelHandle> GetHandle(ChannelConfig config)
    {
        var provider = _buffer.FirstOrDefault(x => x.CanCreate(config));
        return provider?.Create(config) ?? Result.Error<IChannelHandle>($"无法创建通道句柄, 未知的配置类型: {config.GetType().Name}");
    }
}
