using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime;


/// <summary>
/// 运行时标签
/// </summary>
public sealed class RuntimeTag : IRuntimeTag
{
    public ITagConfig Config { get; internal set; } = default!;
    public IAddress Address { get; internal set; } = default!;
    public int DataLength { get; internal set; } = default!;
    public TagValueType ValueType { get; internal set; } = default!;


    public object? Value { get; private set; } = 0;
    public ValueQuality ValueQuality { get; private set; } = ValueQuality.Stale;


    internal RuntimeTag() { }


    #region --工厂--

    public static Result<RuntimeTag> Create(ITagConfig config)
    {
        if (config is IModbusTagConfig modbus)
        {
            return CreateModbus(modbus);
        }

        return Result.Error<RuntimeTag>($"无法创建标签, 不支持的配置类型 [{config.Type}]");
    }
    public static Result<RuntimeTag> CreateModbus(IModbusTagConfig config)
    {
        // 数据长度
        var data_length_result = config.ValueType.GetByteLength();
        if (data_length_result.IsFailure) return data_length_result.Cast<RuntimeTag>();

        // 地址
        var address = new ModbusAddress(config.SlaveId, config.FunctionCode, config.Address);

        // 运行时标签
        var runtime_tag = new RuntimeTag()
        {
            Config = config,
            Address = address,
            DataLength = data_length_result.Value,
            ValueType = config.ValueType,
        };

        return Result.Success(runtime_tag);
    }

    #endregion
}