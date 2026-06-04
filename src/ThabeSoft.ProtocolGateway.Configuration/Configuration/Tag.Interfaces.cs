using ThabeSoft.Modbus;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 只读配置
/// </summary>
public interface ITagConfig
{
    /// <summary>
    /// 名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 值类型
    /// </summary>
    TagValueType ValueType { get; }
}


/// <summary>
/// 只读配置
/// </summary>
public interface IModbusTagConfig : ITagConfig, IValidatable
{
    /// <summary>
    /// 从站Id
    /// </summary>
    byte SlaveId { get; }

    /// <summary>
    /// 地址
    /// </summary>
    ushort Address { get; }

    /// <summary>
    /// 功能码
    /// </summary>
    FunctionCode FunctionCode { get; }
}