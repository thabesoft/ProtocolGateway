using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Modbus;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Tags;


/// <summary>
/// Modbus Tag
/// </summary>
public sealed partial class TagViewModel : ObservableObject
{
    [ObservableProperty]
    public partial ChannelName ChannelName { get; private set; }

    [ObservableProperty]
    public partial string Name { get; private set; }
    [ObservableProperty]
    public partial object Value { get; private set; }
    [ObservableProperty]
    public partial TagValueType ValueType { get; private set; }
    [ObservableProperty]
    public partial ValueQuality ValueQuality { get; private set; }
    [ObservableProperty]
    public partial DateTime Timestamp { get; private set; }



    [ObservableProperty]
    public partial byte SlaveId { get; private set; }

    [ObservableProperty]
    public partial ushort Address { get; private set; }

    [ObservableProperty]
    public partial FunctionCode FunctionCode { get; private set; }


    public static TagViewModel Create(ChannelName channelName)
    {
        return new TagViewModel() { ChannelName = channelName, Value = 10, ValueType = TagValueType.Byte, ValueQuality = ValueQuality.Good, Timestamp = DateTime.Now, SlaveId = 01, Address = 100, FunctionCode = FunctionCode.ReadDiscreteInputs };
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