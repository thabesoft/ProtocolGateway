using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// Rtu 读取响应布局
/// </summary>
public readonly record struct RtuReadResponseLayout : IReadResponseLayout, ICrcable
{
    public static RtuErrorResponseLayout Empty = default;


    /// <summary>从站Id索引</summary>
    public int SlaveIdIndex => 0;

    /// <summary>功能码索引</summary>
    public int FunctionCodeIndex => 1;

    /// <summary>数据长度索引</summary>
    public int DataLengthIndex => 2;

    /// <summary>数据范围</summary>
    public Range DataRange { get; }

    /// <summary>Crc范围</summary>
    public Range CrcRange { get; }


    /// <summary>负载范围(不含Crc之外的数据)</summary>
    public Range PayloadRange { get; }

    /// <summary>数据长度 (字节)</summary>
    public int DataLength { get; }

    /// <summary>数据数量</summary>
    public int DataQuantity { get; }

    /// <summary>总长度 (字节)</summary>
    public int TotalLength { get; }


    internal RtuReadResponseLayout(
        Range dataRange,
        Range payloadRange,
        Range crcRange,
        int dataQuantity,
        int dataLength,
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

    /// <summary>
    /// 读取多个寄存器
    /// </summary>
    public static RtuReadResponseLayout FromRegistersQuantity(ReadRegistersQuantity quantity)
    {
        // Data
        const int data_start = 3;
        var data_length = ProtocolExtensions.GetRegistersToByteLength(quantity);
        int data_end = data_start + data_length;
        var data_range = new Range(data_start, data_end);

        // Content
        var content = new Range(0, data_end);

        // Crc
        var crc_start = data_end;
        var crc_end = data_end + 2;
        var crc_range = new Range(crc_start, crc_end);

        return new RtuReadResponseLayout(
            dataRange: data_range,
            payloadRange: content,
            crcRange: crc_range,
            dataQuantity: quantity,
            dataLength: data_length,
            totalLength: crc_end);
    }

    /// <summary>
    /// 读取多个线圈
    /// </summary>
    public static RtuReadResponseLayout FromCoilsQuantity(ReadCoilsQuantity quantity)
    {
        // Data
        const int data_start = 3;
        var data_length = ProtocolExtensions.GetColisToByteLength(quantity);
        int data_end = data_start + data_length;
        var data_range = new Range(data_start, data_end);

        // Content
        var content = new Range(0, data_end);

        // Crc
        var crc_start = data_end;
        var crc_end = data_end + 2;
        var crc_range = new Range(crc_start, crc_end);

        return new RtuReadResponseLayout(
            dataRange: data_range,
            payloadRange: content,
            crcRange: crc_range,
            dataQuantity: quantity,
            dataLength: data_length,
            totalLength: crc_end);
    }



    public override string ToString()
    {
        // [10] Id(1) Func(2) Addr(2..4) Qua(4..6) Crc(10..12)
        return $"总长度={TotalLength}, Id({SlaveIdIndex})Func({FunctionCodeIndex})Len({DataLength})Data({DataRange})Crc({CrcRange})";
    }
}