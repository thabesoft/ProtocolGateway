using ThabeSoft.ProtocolGateway.Modbus.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;

/// <summary>
/// Rtu 写多个寄存器布局
/// </summary>
public readonly record struct RtuWriteMultipleRegisterRequestLayout : ICrcRangeable, IWriteMultipleRequestLayout
{
    public static readonly RtuWriteMultipleRegisterRequestLayout Empty = default;


    private readonly RtuWriteMultipleRequestLayout _layout;
    private RtuWriteMultipleRegisterRequestLayout(RtuWriteMultipleRequestLayout layout) => _layout = layout;


    /// <summary>从站Id索引</summary>
    public int SlaveIdIndex => _layout.SlaveIdIndex;

    /// <summary>功能码索引</summary>
    public int FunctionCodeIndex => _layout.FunctionCodeIndex;

    /// <summary>地址范围</summary>
    public Range AddressRange => _layout.AddressRange;

    /// <summary>值数量范围</summary>
    public Range QuantityRange => _layout.QuantityRange;

    /// <summary>数据长度索引</summary>
    public int DataLengthIndex => _layout.DataLengthIndex;

    /// <summary>数据范围</summary>
    public Range DataRange => _layout.DataRange;

    /// <summary>内容范围</summary>
    public Range PayloadRange => _layout.PayloadRange;

    /// <summary>Crc范围</summary>
    public Range CrcRange => _layout.CrcRange;

    /// <summary>数据总字节数</summary>
    public int DataByteLength => _layout.DataByteLength;

    /// <summary>数据数量</summary>
    public int DataQuantity => _layout.DataQuantity;

    /// <summary>帧总长度</summary>
    public int TotalLength => _layout.TotalLength;


    /// <summary>
    /// 创建一个拥有指定数量寄存器的帧布局
    /// </summary>
    /// <param name="quantity">寄存器数量</param>
    public static RtuWriteMultipleRegisterRequestLayout Create(WriteRegistersQuantity quantity)
    {
        // Data
        var data_byte_length = ProtocolExtensions.GetRegistersToByteLength(quantity);
        const int data_start = 7;
        int data_end = data_start + data_byte_length;
        var data_range = new Range(data_start, data_end);

        // Content
        var content = new Range(0, data_end);

        // Crc
        var crc_start = data_end;
        var crc_end = data_end + 2;
        var crc_range = new Range(crc_start, crc_end);

        var layout = new RtuWriteMultipleRequestLayout(
            dataRange: data_range,
            contentRange: content,
            crcRange: crc_range,
            dataByteLength: data_byte_length,
            fullByteLength: crc_end,
            dataMaxQuantity: quantity);

        return new RtuWriteMultipleRegisterRequestLayout(layout);
    }

    public override string ToString() => _layout.ToString();
}
