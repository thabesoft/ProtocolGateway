using ThabeSoft.Modbus;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration.Internal;

/// <summary>
/// Modbus 标签配置
/// </summary>
internal sealed class ModbusTagConfig : TagConfig, IModbusTagConfig
{
    /// <summary>
    /// 从站Id
    /// </summary>
    public required byte SlaveId { get; set; }
    /// <summary>
    /// 地址
    /// </summary>
    public required ushort Address { get; set; }
    /// <summary>
    /// 功能码
    /// </summary>
    public required FunctionCode FunctionCode { get; set; }



    public Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return Result.Error($"名称不可为空");
        }
        // 验证：线圈/离散输入只能是 bool
        if (FunctionCode.IsCoil && ValueType != TagValueType.Bool)
        {
            return Result.Error($"功能码 {FunctionCode} 要求值类型为 Bool，当前为 {ValueType}");
        }
        // 验证：保持寄存器/输入寄存器不能是 bool
        if (FunctionCode.IsRegister && ValueType == TagValueType.Bool)
        {
            return Result.Error($"功能码 {FunctionCode} 不支持 Bool 类型");
        }

        return Result.Success();
    }
}