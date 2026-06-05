using System.Text.Json.Serialization;
using ThabeSoft.Modbus;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Configuration.Json.Converters;

namespace ThabeSoft.ProtocolGateway.Infrastructure.Json;


[JsonSourceGenerationOptions(
    // 生成模式
    GenerationMode = JsonSourceGenerationMode.Metadata,
    // 大驼峰
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
    // 忽略大小写，允许驼峰和帕斯卡命名
    PropertyNameCaseInsensitive = true,
    // 格式化输出
    WriteIndented = true,
    // 使用枚举名称转换器
    UseStringEnumConverter = true,

    // 转换器
    Converters = [
        typeof(ChannelNameConverter),
        typeof(FunctionCodeConverter),
        typeof(BaudRateConverter),
        //typeof(JsonStringEnumConverter<ChannelType>),
        //typeof(JsonStringEnumConverter<ProtocolType>),
        //typeof(JsonStringEnumConverter<PortType>),
        //typeof(JsonStringEnumConverter<TagValueType>),

        //typeof(JsonStringEnumConverter<Parity>),
        //typeof(JsonStringEnumConverter<StopBits>),
        //typeof(JsonStringEnumConverter<DuplexMode>),
    ]
)]
// 网关
[JsonSerializable(typeof(GatewayConfig))]
// 通道
[JsonSerializable(typeof(ChannelConfig))]
[JsonSerializable(typeof(ChannelConfig[]))]

// 标签
[JsonSerializable(typeof(TagConfig[]))]
[JsonSerializable(typeof(TagConfig))]
[JsonSerializable(typeof(ModbusTagConfig))]

// 端口
[JsonSerializable(typeof(PortConfig))]
[JsonSerializable(typeof(SerialPortConfig))]

// 值类型
[JsonSerializable(typeof(FunctionCode))]
internal partial class ConfigJsonSerializerContext : JsonSerializerContext;