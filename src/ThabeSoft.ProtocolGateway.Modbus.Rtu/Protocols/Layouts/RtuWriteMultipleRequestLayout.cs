namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;

/// <summary>
/// Rtu 写多个值
/// </summary>
internal readonly record struct RtuWriteMultipleRequestLayout : ICrcable, IWriteMultipleRequestLayout
{
    /// <summary>从站Id索引</summary>
    public readonly int SlaveIdIndex => 0;
    /// <summary>功能码索引</summary>
    public readonly int FunctionCodeIndex => 1;
    /// <summary>地址范围[</summary>
    public readonly Range AddressRange => new(2, 4);
    /// <summary>值数量范围</summary>
    public readonly Range QuantityRange => new(4, 6);
    /// <summary>数据长度索引</summary>
    public readonly int DataLengthIndex => 6;
    /// <summary>数据范围</summary>
    public readonly Range DataRange { get; }
    /// <summary>内容范围</summary>
    public readonly Range PayloadRange { get; }
    /// <summary>Crc范围</summary>
    public readonly Range CrcRange { get; }
    /// <summary>数据总字节数</summary>
    public readonly int DataLength { get; }
    /// <summary>数据数量</summary>
    public readonly int DataQuantity { get; }
    /// <summary>帧总长度</summary>
    public readonly int TotalLength { get; }


    internal RtuWriteMultipleRequestLayout(
        Range dataRange,
        Range contentRange,
        Range crcRange,
        int dataByteLength,
        int fullByteLength,
        int dataMaxQuantity
        )
    {
        DataRange = dataRange;
        PayloadRange = contentRange;
        CrcRange = crcRange;
        DataLength = dataByteLength;
        TotalLength = fullByteLength;
        DataQuantity = dataMaxQuantity;
    }

    public override string ToString()
    {
        // [10] Id(1) Func(2) Addr(2..4) Qua(4..6) Len(6) Data(7..10) Crc(10..12)
        return $"总长度={TotalLength}, Id({SlaveIdIndex})Func({FunctionCodeIndex})Addr({AddressRange})Qua({QuantityRange})Len({DataLengthIndex})Data({DataRange})Crc({CrcRange})";
    }
}