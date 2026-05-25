using ThabeSoft.Modbus.Helpers;
using ThabeSoft.Modbus.Primitives;

namespace ThabeSoft.Modbus.Layouts;

public readonly record struct RtuReadResponseLayout : IReadResponseLayout, IPayloadRangeable
{
    public static RtuReadResponseLayout Empty => default;


    /// <summary>从站Id索引</summary>
    public int SlaveIdIndex => 0;
    /// <summary>功能码索引</summary>
    public int FunctionCodeIndex => 1;
    /// <summary>地址范围</summary>
    public int DataLengthIndex => 2;
    /// <summary>参数数量</summary>
    public Range DataRange { get; }
    /// <summary>Crc范围</summary>
    public Range CrcRange { get; }


    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public Range PayloadRange { get; }
    /// <summary> 数据长度 (字节)  </summary>
    public int DataLength { get; }
    /// <summary> 总长度(字节) </summary>
    public int TotalLength { get; }
    /// <summary> 数据数量 </summary>
    public int DataQuantity { get; }


    [Obsolete("")]
    public RtuReadResponseLayout() { }
    internal RtuReadResponseLayout(
        Range dataRange,
        Range crcRange,
        Range payloadRange,
        int dataLength,
        int dataQuantity,
        int totalLength
        )
    {
        DataRange = dataRange;
        PayloadRange = payloadRange;
        CrcRange = crcRange;

        DataQuantity = dataQuantity;
        DataLength = dataLength;
        TotalLength = totalLength;
    }

    private static RtuReadResponseLayout Create(int dataLength, int dataQuantity)
    {
        // 数据范围
        const int data_start = 3;
        var data_end = dataLength;
        var data_range = data_start..data_end;

        // Crc范围
        var crc_start = data_end;
        var crc_end = crc_start + 2;
        var crc_range = crc_start..crc_end;

        // 数据荷载范围
        const int payload_start = 0;
        var payload_end = crc_start;
        var payload_range = payload_start..payload_end;

        return new RtuReadResponseLayout(
           dataRange: data_range,
           crcRange: crc_range,
           payloadRange: payload_range,
           dataLength: dataLength,
           dataQuantity: dataQuantity,
           totalLength: crc_end);
    }

    public static RtuReadResponseLayout FromQuantity(ReadCoilsQuantity quantity)
    {
        return Create(quantity.ByteLength, quantity);
    }
    public static RtuReadResponseLayout FromQuantity(ReadRegistersQuantity quantity)
    {
        return Create(quantity.ByteLength, quantity);
    }


    public override string ToString()
    {
        return $"总长度={TotalLength}, Id({SlaveIdIndex})Func({FunctionCodeIndex})Len({DataLengthIndex})Data({DataRange})Crc({CrcRange})";
    }
}