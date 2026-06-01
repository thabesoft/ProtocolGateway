using System.Text.Json.Serialization;
using ThabeSoft.Modbus;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Internal.Json;


// 转换器
[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true,
    Converters = [
        typeof(FunctionCodeConverter),
        typeof(BaudRateConverter),
        typeof(FunctionCodeConverter)
    ]
)]
// 通道
[JsonSerializable(typeof(ChannelConfig[]))]
[JsonSerializable(typeof(ChannelConfig))]
// 标签
[JsonSerializable(typeof(TagConfig))]
[JsonSerializable(typeof(ModbusTagConfig))]
[JsonDerivedType(typeof(ModbusTagConfig), typeDiscriminator: "modbus")]
// 端口
[JsonSerializable(typeof(PortConfig))]
[JsonSerializable(typeof(SerialPortConfig))]
// 值类型
[JsonSerializable(typeof(FunctionCode))]
public partial class JsonContext : JsonSerializerContext
{
}