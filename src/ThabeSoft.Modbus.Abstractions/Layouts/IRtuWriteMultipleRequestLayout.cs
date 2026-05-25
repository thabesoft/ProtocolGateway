namespace ThabeSoft.Modbus.Layouts;

public interface IRtuWriteMultipleRequestLayout : ILayout, IDatable
{
    /// <summary>地址范围</summary>
    Range AddressRange { get; }

    /// <summary>值数量范围</summary>
    public Range QuantityRange { get; }

    /// <summary> 数据长度索引 </summary>
    int DataLengthIndex { get; }
}