using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration;

/// <summary>
/// 通道配置
/// </summary>
public sealed record class ChannelConfig : IChannelConfig, IValidatable, IDeepCloneable<ChannelConfig>
{
    IReadOnlyList<ITagConfig> IChannelConfig.Tags => Tags;


    /// <summary>
    /// 通道类型
    /// </summary>
    public ChannelType Type => Protocol.ToChannelType();

    /// <summary>
    /// 名称
    /// </summary>
    public required ChannelName Name { get; init; }

    /// <summary>
    /// 协议
    /// </summary>
    public required ProtocolType Protocol { get; init; }

    /// <summary>
    /// 通信端口
    /// </summary>
    public required PortConfig Port { get; init; }

    /// <summary>
    /// 标签
    /// </summary>
    public IReadOnlyList<TagConfig> Tags { get; init; } = [];

    


    public Result Validate()
    {
        var errors = new List<string>();

        if (Port is IValidatable port)
        {
            var result = port.Validate();
            if (!result.IsSuccess) errors.Add(result.Message!);
        }

        foreach (var i in Tags)
        {
            if (i is IValidatable tag)
            {
                var result = tag.Validate();
                if (!result.IsSuccess) errors.Add(result.Message!);
            }
        }

        return errors.Count == 0 ? Result.Success() : Result.Error($"配置验证失败:\n- {string.Join("\n- ", errors)}");
    }

    public ChannelConfig DeepClone()
    {
        return this with
        {
            Port = Port.DeepClone(),
            Tags = [.. Tags.Select(x => x.DeepClone())]
        };
    }
}