using System.Diagnostics;
using ThabeSoft.IndustrialHub.Modbus;

namespace ThabeSoft.ProtocolGateway.Protocols.Layouts;


/// <summary>
/// Modbus Rtu 写多值请求布局
/// </summary>
public readonly struct ModbusRtuWriteMultipleRequestLayout : IModbusRtuLayoutExtension,
    IModbusWriteMultipleRequestLayout
{
    public static readonly ModbusRtuWriteMultipleRequestLayout Empty;
    static ModbusRtuWriteMultipleRequestLayout() => Empty = default;



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
    /// <summary>数据最大数量</summary>
    public readonly int DataMaxQuantity { get; }
    /// <summary>帧总长度</summary>
    public readonly int TotalLength { get; }


    private ModbusRtuWriteMultipleRequestLayout(
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
        DataMaxQuantity = dataMaxQuantity;
    }




    /// <summary>
    /// 创建一个拥有指定数量寄存器的帧布局
    /// </summary>
    /// <param name="quantity">寄存器数量</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static bool TryCreayeRegisters(byte quantity, out ModbusRtuWriteMultipleRequestLayout result)
    {
        result = default;

        if (quantity <= 0)
        {
            Debug.Fail("寄存器数量必须大于0");
            return false;
        }
        if (quantity > 125)
        {
            Debug.Fail("寄存器数量不能超过125");
            return false;
        }

        // Data
        var data_byte_length = ModbusFrameLayout.GetRegistersToByteLength(quantity);
        const int data_start = 7;
        int data_end = data_start + data_byte_length;
        var data_range = new Range(data_start, data_end);

        // Content
        var content = new Range(0, data_end);

        // Crc
        var crc_start = data_end;
        var crc_end = data_end + 2;
        var crc_range = new Range(crc_start, crc_end);

        result = new ModbusRtuWriteMultipleRequestLayout(
            dataRange: data_range,
            contentRange: content,
            crcRange: crc_range,
            dataByteLength: data_byte_length,
            fullByteLength: crc_end,
            dataMaxQuantity: quantity);

        return true;
    }
    /// <summary>
    /// 创建一个拥有指定数量线圈的帧布局
    /// </summary>
    /// <param name="quantity">线圈数量</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static bool TryCreateCoils(ushort quantity, out ModbusRtuWriteMultipleRequestLayout result)
    {
        result = default;

        if (quantity <= 0)
        {
            Debug.Fail("线圈数量必须大于0"); 
            return false;
        }
        if (quantity > 2000)
        {
            Debug.Fail("线圈数量不能超过2000"); 
            return false;
        }

        // Data
        var data_byte_length = ModbusFrameLayout.GetColisToByteLength(quantity);
        const int data_start = 7;
        int data_end = data_start + data_byte_length;
        var data_range = new Range(data_start, data_end);

        // Content
        var content = new Range(0, data_end);

        // Crc
        var crc_start = data_end;
        var crc_end = data_end + 2;
        var crc_range = new Range(crc_start, crc_end);

        result = new ModbusRtuWriteMultipleRequestLayout(
            dataRange: data_range,
            contentRange: content,
            crcRange: crc_range,
            dataByteLength: data_byte_length,
            fullByteLength: crc_end,
            dataMaxQuantity: quantity);

        return true;
    }
}

