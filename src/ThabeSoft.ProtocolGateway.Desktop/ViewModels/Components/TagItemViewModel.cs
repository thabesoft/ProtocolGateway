using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Modbus;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Runtime;

namespace ThabeSoft.ProtocolGateway.ViewModels.Components;


/// <summary>
/// 标签元素
/// </summary>
public partial class TagItemViewModel : ViewModel
{
    // 名称
    [ObservableProperty]
    public partial string? Name { get; private set; }

    // 类型
    [ObservableProperty]
    public partial TagValueType? ValueType { get; private set; }

    // 值
    [ObservableProperty]
    public partial object? Value { get; private set; }

    // 值质量
    [ObservableProperty]
    public partial ValueQuality? ValueQuality { get; private set; }

    // 时间戳
    [ObservableProperty]
    public partial DateTime? Timestamp { get; private set; }


    #region --Modbus--

    // 是否是 Modbus 标签
    [ObservableProperty]
    public partial bool IsModbusTag { get; private set; }

    // 从站 Id
    [ObservableProperty]
    public partial byte? SlaveId { get; private set; }

    // 其实地址
    [ObservableProperty]
    public partial ushort? Address { get; private set; }

    // 功能码
    [ObservableProperty]
    public partial FunctionCode? FunctionCode { get; private set; }

    #endregion


    public TagItemViewModel()
    {

    }
    public TagItemViewModel(IRuntimeTag runtimeTag)
    {
        UpdateRuntimeTag(runtimeTag);
    }
    public void UpdateRuntimeTag(IRuntimeTag tag)
    {
        Name = tag.Config.Name;
        ValueType = tag.Config.ValueType;

        var type = tag.Config.Type;

        if(type == ChannelType.Modbus  && tag.Config is IModbusTagConfig modbus_tag_config)
        {
            IsModbusTag = true;
            SlaveId = modbus_tag_config.SlaveId;
            Address = modbus_tag_config.Address;
            FunctionCode = modbus_tag_config.FunctionCode;
        }
    }
}
