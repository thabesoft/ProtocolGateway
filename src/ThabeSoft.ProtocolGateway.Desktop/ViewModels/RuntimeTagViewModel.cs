using Avalonia.Controls;
using ThabeSoft.Modbus;
using ThabeSoft.Mvvm;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 标签配置
/// </summary>
public sealed class RuntimeTagViewModel : ViewModelBase
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


    public RuntimeTagViewModel()
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

    public static RuntimeTagViewModel CreateFromConfig(ITagConfig config)
    {
        var tag = new RuntimeTagViewModel();
        tag.LoadConfig(config);

        return tag;
    }


    public void LoadConfig(ITagConfig config)
    {
        if(config is IModbusTagConfig modbus)
        {
            LoadConfig(modbus);
        }
    }

    /// <summary>
    /// 从配置加载
    /// </summary>
    public void LoadConfig(IModbusTagConfig config)
    {
        using var _ = _lock.EnterScope();

        IsModbusTag = true;
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
        using var _ = _lock.EnterScope();

        Value = value;
        ValueQuality = quality;
        Timestamp = DateTime.Now;
    }
}