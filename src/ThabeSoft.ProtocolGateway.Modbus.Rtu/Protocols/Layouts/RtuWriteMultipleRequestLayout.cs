using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// Modbus Rtu 写多值请求布局
/// </summary>
public readonly struct RtuWriteMultipleRequestLayout : ICrcRangeable,
    IWriteMultipleRequestLayout
{
    public static readonly RtuWriteMultipleRequestLayout Empty;
    static RtuWriteMultipleRequestLayout() => Empty = default;



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
    public readonly int DataByteLength { get; }
    /// <summary>数据数量</summary>
    public readonly int DataQuantity { get; }
    /// <summary>帧总长度</summary>
    public readonly int TotalLength { get; }


    private RtuWriteMultipleRequestLayout(
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
        DataByteLength = dataByteLength;
        TotalLength = fullByteLength;
        DataQuantity = dataMaxQuantity;
    }


    /// <summary>
    /// 创建一个拥有指定数量寄存器的帧布局
    /// </summary>
    /// <param name="quantity">寄存器数量</param>
    public static RtuWriteMultipleRequestLayout CreateRegisters(WriteRegistersQuantity quantity)
    {
        // Data
        var data_byte_length = LayoutExtensions.GetRegistersToByteLength(quantity);
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

    /// <summary>
    /// 创建一个拥有指定数量寄存器的帧布局
    /// </summary>
    /// <param name="quantity">寄存器数量</param>
    public static Result<RtuWriteMultipleRequestLayout> CreateRegisters(int quantity)
    {
        return WriteRegistersQuantity.Create(quantity)
            .Then(CreateRegisters);
    }


    /// <summary>
    /// 创建一个拥有指定数量线圈的帧布局
    /// </summary>
    /// <param name="quantity">线圈数量</param>
    public static RtuWriteMultipleRequestLayout CreateCoils(WriteCoilsQuantity quantity)
    {
        // Data
        var data_byte_length = LayoutExtensions.GetColisToByteLength(quantity);
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

    /// <summary>
    /// 创建一个拥有指定数量线圈的帧布局
    /// </summary>
    /// <param name="quantity">线圈数量</param>
    public static Result<RtuWriteMultipleRequestLayout> CreateCoils(int quantity)
    {
        return WriteCoilsQuantity.Create(quantity)
            .Then(CreateCoils);
    }
}
