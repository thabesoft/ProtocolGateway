using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Modbus;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// Modbus Tag
/// </summary>
public sealed partial class TagViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string? Name { get; private set; }
    [ObservableProperty]
    public partial object? Value { get; private set; }
    [ObservableProperty]
    public partial TagValueType? ValueType { get; private set; }
    [ObservableProperty]
    public partial ValueQuality? ValueQuality { get; private set; }
    [ObservableProperty]
    public partial DateTime? Timestamp { get; private set; }



    [ObservableProperty]
    public partial byte? SlaveId { get; private set; }

    [ObservableProperty]
    public partial ushort? Address { get; private set; }

    [ObservableProperty]
    public partial FunctionCode? FunctionCode { get; private set; }


    public static TagViewModel FromTagConfig(ITagConfig config)
    {
        if (config is ModbusTagConfig modbusConfig)
        {
            return FromModbusConfig(modbusConfig);
        }

        throw new NotSupportedException($"Unsupported tag config type: {config.GetType().FullName}");
    }

    public static TagViewModel FromModbusConfig(ModbusTagConfig config)
    {
        return new TagViewModel()
        {
            Name = config.Name,
            ValueType = config.ValueType,
            SlaveId = config.SlaveId,
            Address = config.Address,
            FunctionCode = config.FunctionCode
        };
    }



    /// <summary>
    /// 更新值
    /// </summary>
    public void UpdateValue(object value, ValueQuality quality)
    {
        Value = value;
        ValueQuality = quality;
        Timestamp = DateTime.Now;
    }
}