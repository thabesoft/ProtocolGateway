using ThabeSoft.Modbus;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 标签
/// </summary>
public abstract class TagModel
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
/// 标签
/// </summary>
public sealed class ModbusTagModel : TagModel
{
    /// <summary>
    /// 通道类型
    /// </summary>
    public required ChannelType ChannelType { get; init; }
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


    private ModbusTagModel() { }
    public static Result<ModbusTagModel> Create(ModbusTagConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.Name))
        {
            return Result.Error<ModbusTagModel>($"名称不可为空");
        }
        // 验证：线圈/离散输入只能是 bool
        if (config.FunctionCode.IsCoil && config.ValueType != TagValueType.Bool)
        {
            return Result.Error<ModbusTagModel>($"功能码 {config.FunctionCode} 要求值类型为 Bool，当前为 {config.ValueType}");
        }
        // 验证：保持寄存器/输入寄存器不能是 bool
        if (config.FunctionCode.IsRegister && config.ValueType == TagValueType.Bool)
        {
            return Result.Error<ModbusTagModel>($"功能码 {config.FunctionCode} 不支持 Bool 类型");
        }

        var value = new ModbusTagModel()
        {
            Name = config.Name,
            ValueType = config.ValueType,
            ChannelType = ChannelType.Modbus,
            SlaveId = config.SlaveId,
            Address = config.Address,
            FunctionCode = config.FunctionCode,
        };

        return Result.Success(value);
    }
}
