 using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Modbus;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// Modbus Tag
/// </summary>
public sealed partial class TagViewModel : ObservableObject, IViewModel
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


    #region --Modbus--

    [ObservableProperty]
    public partial bool IsModbusTag { get; private set; }

    [ObservableProperty]
    public partial byte? SlaveId { get; private set; }

    [ObservableProperty]
    public partial ushort? Address { get; private set; }

    [ObservableProperty]
    public partial FunctionCode? FunctionCode { get; private set; }

    #endregion


    public TagViewModel()
    {
        if (Design.IsDesignMode)
        {
            Name = "设计时名称";
            ValueType = TagValueType.Int16;

            Span<ValueQuality> arr = [ProtocolGateway.ValueQuality.Good, ProtocolGateway.ValueQuality.Bad, ProtocolGateway.ValueQuality.Uncertain, ProtocolGateway.ValueQuality.Stale];
            Value = Random.Shared.Next(ushort.MinValue, ushort.MaxValue);
            ValueQuality = arr[Random.Shared.Next(arr.Length)];
            Timestamp = DateTime.Now;

            SlaveId = 1;
            Address = 100;
            FunctionCode = Modbus.FunctionCode.ReadHoldingRegisters;
        }
    }

    /// <summary>
    /// 从配置加载
    /// </summary>
    public void LoadConfig(ModbusTagConfig config)
    {
        Name = config.Name;
        ValueType = config.ValueType;
        SlaveId = config.SlaveId;
        Address = config.Address;
        FunctionCode = config.FunctionCode;
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