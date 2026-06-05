using System.Text.Json.Serialization;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 标签配置
/// </summary>
[JsonDerivedType(typeof(ModbusTagConfig), typeDiscriminator: nameof(ChannelType.Modbus))]
public abstract record class TagConfig : ITagConfig, IValidatable, IDeepCloneable<TagConfig>
{
    public required string Name { get; set; }
    public required TagValueType ValueType { get; set; }


    public virtual Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return Result.Error("名称不可为空");
        }

        return Result.Success();
    }

    public TagConfig DeepClone()
    {
        return this with { };
    }
}