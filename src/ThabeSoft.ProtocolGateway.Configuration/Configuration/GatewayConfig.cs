using System.Text;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 网关配置
/// </summary>
public sealed record class GatewayConfig : IGatewayConfig, IValidatable, IDeepCloneable<GatewayConfig>
{
    IReadOnlyList<IChannelConfig> IGatewayConfig.Channels => Channels;


    /// <summary>
    /// 名称
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// 标签
    /// </summary>
    public IReadOnlyList<ChannelConfig> Channels { get; init; } = [];


    public Result Validate()
    {
        StringBuilder err_message = new();

        if (string.IsNullOrWhiteSpace(Name))
        {
            return Result.Warning("网关名称不可为空");
        }

        var messages = Channels.Select(x => x.Validate()).Where(x => x.IsProblem).Select(x => x.Message!).ToArray();
        return messages.Length == 0 ? Result.Success() : Result.Error($"配置验证失败:\n- {string.Join("\n- ", messages)}");
    }

    public GatewayConfig DeepClone()
    {
        return this with
        {
            Channels = [.. Channels.Select(x => x.DeepClone())]
        };
    }
    GatewayConfig IDeepCloneable<GatewayConfig>.DeepClone()
    {
        return DeepClone();
    }
}