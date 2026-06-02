using System.Text.Json.Serialization;
using ThabeSoft.Modbus;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 标签配置
/// </summary>
[JsonDerivedType(typeof(ModbusTagConfig), typeDiscriminator: "Modbus")]
public abstract class TagConfig
{
    /// <summary>
    /// 名称
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 值类型
    /// </summary>
    public required TagValueType ValueType { get; init; }
}

/// <summary>
/// Modbus标签配置
/// </summary>
public sealed class ModbusTagConfig : TagConfig
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