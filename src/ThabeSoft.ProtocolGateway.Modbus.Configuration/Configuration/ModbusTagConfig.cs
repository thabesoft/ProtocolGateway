using ThabeSoft.Modbus;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// Modbus 标签配置
/// </summary>
public sealed class ModbusTagConfig : ITagConfig
{
    /// <summary>
    /// 名称
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 数据类型
    /// </summary>
    public required TagValueType ValueType { get; init; }

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