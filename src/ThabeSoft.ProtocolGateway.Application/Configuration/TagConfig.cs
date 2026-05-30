using System.Text.Json.Serialization;
using ThabeSoft.Modbus;
using ThabeSoft.ProtocolGateway.Enums;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 标签配置
/// </summary>
[JsonDerivedType(typeof(ModbusTagOptions), typeDiscriminator: nameof(ChannelType.Modbus))]
public abstract class TagConfig
{
    /// <summary>
    /// 值类型
    /// </summary>
    public TagValueType ValueType { get; private set; }
}


/// <summary>
/// Modbus 标签配置
/// </summary>
public sealed class ModbusTagOptions : TagConfig
{
    /// <summary>
    /// 从站Id
    /// </summary>
    public required byte SlaveId { get; init; }
    /// <summary>
    /// 地址
    /// </summary>
    public required ushort Address { get; init; }

    /// <summary>
    /// 功能码
    /// </summary>
    public required FunctionCode FunctionCode { get; init; }
}