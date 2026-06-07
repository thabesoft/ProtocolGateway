using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Layouts;

/// <summary>
/// Rtu 写多个值
/// </summary>
public readonly record struct RtuWriteMultipleRequestLayout : IRtuWriteMultipleRequestLayout
{
    public static RtuWriteMultipleRequestLayout Empty => default;


    /// <summary>从站Id索引</summary>
    public int SlaveIdIndex => 0;
    /// <summary>功能码索引</summary>
    public int FunctionCodeIndex => 1;
    /// <summary>地址范围[</summary>
    public Range AddressRange => 2..4;
    /// <summary>值数量范围</summary>
    public Range QuantityRange => 4..6;
    /// <summary>数据长度索引</summary>
    public int DataLengthIndex => 6;
    /// <summary>数据范围</summary>
    public Range DataRange { get; }
    /// <summary>Crc范围</summary>
    public Range CrcRange { get; }


    /// <summary>内容范围</summary>
    public readonly Range PayloadRange { get; }
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



    /// <summary>
    /// 从线圈数量
    /// </summary>
    public static RtuWriteMultipleRequestLayout FromQuantity(WriteCoilsQuantity quantity)
    {
        // Data
        var data_byte_length = quantity.ByteLength;
        const int data_start = 7;
        int data_end = data_start + data_byte_length;
        var data_range = new Range(data_start, data_end);

        // Content
        var content = new Range(0, data_end);

        // Crc
        var crc_start = data_end;
        var crc_end = data_end + 2;
        var crc_range = new Range(crc_start, crc_end);

        return new RtuWriteMultipleRequestLayout(
            dataRange: data_range,
            contentRange: content,
            crcRange: crc_range,
            dataByteLength: data_byte_length,
            fullByteLength: crc_end,
            dataMaxQuantity: quantity);
    }
    public static Result<RtuWriteMultipleRequestLayout> FromCoilsQuantity(int quantity)
    {
        return WriteCoilsQuantity.Create(quantity).Then(FromQuantity);
    }


    /// <summary>
    /// 从寄存器数量创建
    /// </summary>
    public static RtuWriteMultipleRequestLayout FromQuantity(WriteRegistersQuantity quantity)
    {
        // Data
        var data_byte_length = quantity.ByteLength;
        const int data_start = 7;
        int data_end = data_start + data_byte_length;
        var data_range = new Range(data_start, data_end);

        // Content
        var content = new Range(0, data_end);

        // Crc
        var crc_start = data_end;
        var crc_end = data_end + 2;
        var crc_range = new Range(crc_start, crc_end);

        return new RtuWriteMultipleRequestLayout(
            dataRange: data_range,
            contentRange: content,
            crcRange: crc_range,
            dataByteLength: data_byte_length,
            fullByteLength: crc_end,
            dataMaxQuantity: quantity);
    }
    public static Result<RtuWriteMultipleRequestLayout> FromRegistersQuantity(int quantity)
    {
        return WriteRegistersQuantity.Create(quantity).Then(FromQuantity);
    }

    public override string ToString()
    {
        // [10] Id(1) Func(2) Addr(2..4) Qua(4..6) Len(6) Data(7..10) Crc(10..12)
        return $"总长度={TotalLength}, Id({SlaveIdIndex})Func({FunctionCodeIndex})Addr({AddressRange})Qua({QuantityRange})Len({DataLengthIndex})Data({DataRange})Crc({CrcRange})";
    }
}