using System.Text.Json.Serialization;
using ThabeSoft.Modbus;
using ThabeSoft.Ports;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.JsonConverters;



[JsonSourceGenerationOptions(
    // 生成模式
    GenerationMode = JsonSourceGenerationMode.Metadata,
    // 大驼峰
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
    // 忽略大小写，允许驼峰和帕斯卡命名
    PropertyNameCaseInsensitive = true,
    // 格式化输出
    WriteIndented = true,
    // 转换器
    Converters = [
        typeof(ChannelNameConverter),
        typeof(FunctionCodeConverter),
        typeof(BaudRateConverter),
        typeof(JsonStringEnumConverter<ChannelType>),
        typeof(JsonStringEnumConverter<ProtocolType>),
        typeof(JsonStringEnumConverter<PortType>),
        typeof(JsonStringEnumConverter<TagValueType>),

        typeof(JsonStringEnumConverter<Parity>),
        typeof(JsonStringEnumConverter<StopBits>),
        typeof(JsonStringEnumConverter<DuplexMode>),
    ]
)]
// 通道
[JsonSerializable(typeof(ChannelConfig))]
[JsonSerializable(typeof(ChannelConfig[]))]

// 标签
[JsonSerializable(typeof(TagConfig))]
[JsonSerializable(typeof(ModbusTagConfig))]

// 端口
[JsonSerializable(typeof(PortConfig))]
[JsonSerializable(typeof(SerialPortConfig))]

// 值类型
[JsonSerializable(typeof(FunctionCode))]
public partial class JsonContext : JsonSerializerContext
{

}