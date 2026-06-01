using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Services;


internal sealed class ChannelHandleFactory(IEnumerable<IChannelHandleProvider> providers) : IChannelHandleFactory
{
    private readonly IChannelHandleProvider[] _buffer = [.. providers];

    public Result<IChannelHandle> GetHandle(IChannelConfig config)
    {
        var provider = _buffer.FirstOrDefault(x => x.CanCreate(config));
        return provider?.Create(config) ?? Result.InvalidOperation<IChannelHandle>($"无法创建通道句柄, 未知的配置类型: {config.GetType().Name}");
    }
}
