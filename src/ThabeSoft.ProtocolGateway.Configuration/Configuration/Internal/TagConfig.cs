using System.Text.Json.Serialization;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration.Internal;


/// <summary>
/// 标签配置
/// </summary>
[JsonDerivedType(typeof(ModbusTagConfig), typeDiscriminator: nameof(ChannelType.Modbus))]
internal abstract class TagConfig : ITagConfig
{
    public required string Name { get; set; }
    public required TagValueType ValueType { get; set; }


    public Result Validate()
    {
        if(string.IsNullOrWhiteSpace(Name))
        {
            return Result.Error("名称不可为空");
        }

        return Result.Success();
    }
}
