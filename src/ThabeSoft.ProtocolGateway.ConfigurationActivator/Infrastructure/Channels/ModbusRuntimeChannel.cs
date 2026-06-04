using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Infrastructure.Channels;


/// <summary>
/// ModbusRtu 运行时通道
/// </summary>
internal static class ModbusRuntimeChannel
{
    public static Result<IRuntimeChannel> Create(IChannelConfig config)
    {
        var result = config.Validate();
        if (!result.IsSuccess) return result.Cast<IRuntimeChannel>();


        // 传输层结果
        var transport_result = config.Port.CreateTransport();
        if (!transport_result.IsSuccess) return transport_result.Cast<IRuntimeChannel>();

        // 主站
        var master_result = config.Protocol.CreateModbusMaster(transport_result.Value);
        if (!master_result.IsSuccess) return master_result.Cast<IRuntimeChannel>();

        // 通道
        var channel = new ModbusChannel(master_result.Value);
        // 运行时
        var runtime = new RuntimeChannel(config, channel);


        return Result.Success<IRuntimeChannel>(runtime);
    }
}