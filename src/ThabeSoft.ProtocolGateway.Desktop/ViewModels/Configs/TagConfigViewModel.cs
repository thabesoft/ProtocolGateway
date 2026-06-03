using Avalonia.Controls;
using Avalonia.Threading;
using ThabeSoft.Modbus;
using ThabeSoft.Mvvm;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 标签配置
/// </summary>
public sealed class TagConfigViewModel : ViewModelBase
{
    private readonly Lock _lock = new();


    public string? Name { get; private set => Change(field, value, x => field = x).NotNullOrWhiteSpace(() => "名称不可为空").Apply(); }
    public object? Value { get; private set => Apply(field, value, x => field = x);}
    public TagValueType? ValueType { get; private set => Apply(field, value, x => field = x); }
    public ValueQuality? ValueQuality { get; private set => Apply(field, value, x => field = x); }
    public DateTime? Timestamp { get; private set => Apply(field, value, x => field = x); }


    #region --Modbus--

    public bool IsModbusTag { get; private set => Apply(field, value, x => field = x); }
    public  byte? SlaveId { get; private set => Apply(field, value, x => field = x); }
    public  ushort? Address { get; private set => Apply(field, value, x => field = x); }
    public  FunctionCode? FunctionCode { get; private set => Apply(field, value, x => field = x); }

    #endregion


    public TagConfigViewModel()
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
        using var _ = _lock.EnterScope();

        Dispatcher.UIThread.Post(() =>
        {
            IsModbusTag = true;
            Name = config.Name;
            ValueType = config.ValueType;
            SlaveId = config.SlaveId;
            Address = config.Address;
            FunctionCode = config.FunctionCode;

        }, DispatcherPriority.Background);
    }


    /// <summary>
    /// 更新值
    /// </summary>
    public void UpdateValue(object value, ValueQuality quality)
    {
        Dispatcher.UIThread.Post(() =>
        {
            Value = value;
            ValueQuality = quality;
            Timestamp = DateTime.Now;

        }, DispatcherPriority.Input);
    }
}