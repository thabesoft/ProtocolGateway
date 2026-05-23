using ThabeSoft.ProtocolGateway.Conversion;
using ThabeSoft.ProtocolGateway.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Crc;

namespace ThabeSoft.ProtocolGateway.Protocols.Serializer;


/// <summary>
/// Rtu写多个值帧布局
/// </summary>
public sealed class ModbusRtuWriteMultipleRequestSerializer :
    IModbusWriteMultipleRegistersRequestSerializer,
    IModbusWriteMultipleCoilsRequestSerializer
{
    public static ModbusRtuWriteMultipleRequestSerializer Instance { get; } = new();
    private ModbusRtuWriteMultipleRequestSerializer() { }



    bool IModbusWriteMultipleRegistersRequestSerializer.TryPack(Span<byte> destination, byte slaveId, ushort address, ReadOnlySpan<ushort> values)
    {
        var quantity = (byte)values.Length;
        var layout = ModbusRtuWriteMultipleRequestLayout.TryCreayeRegisters(quantity, out var result)

        return TryPackRegisters(layout, destination, slaveId, address, values);
    }
    bool IModbusWriteMultipleRegistersRequestSerializer.TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, Span<ushort> values)
    {
        var quantity = (byte)values.Length;
        var layout = ModbusRtuWriteMultipleRequestLayout.TryCreayeRegisters(quantity);

        return TryUnpackRegisters(layout, source, out slaveId, out address, values);
    }

    bool IModbusWriteMultipleCoilsRequestSerializer.TryPack(Span<byte> destination, byte slaveId, ushort address, ReadOnlySpan<bool> values)
    {
        var quantity = (ushort)values.Length;
        var layout = ModbusRtuWriteMultipleRequestLayout.WriteMultipleCoils(quantity);

        return TryPackCoils(layout, destination, slaveId, address, values);
    }
    bool IModbusWriteMultipleCoilsRequestSerializer.TryUnpack(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, Span<bool> values)
    {
        var quantity = (ushort)values.Length;
        var layout = ModbusRtuWriteMultipleRequestLayout.WriteMultipleCoils(quantity);

        return TryUnpackCoils(layout, source, out slaveId, out address, values);
    }


    /*------------------- 共用 -------------------*/

    private static bool TryPack(
        in ModbusRtuWriteMultipleRequestLayout layout,
        Span<byte> destination,
        in byte slaveId,
        in ModbusFunctionCode functionCode,
        in ushort address,
        in ushort quantity
        )
    {
        // 缓冲区长度不足
        if (destination.Length < layout.TotalLength) return false;
        // 参数数量超过预期
        if (quantity > layout.DataMaxQuantity) return false;

        // 从站
        destination[layout.SlaveIdIndex] = slaveId;
        // 功能码
        destination[layout.FunctionCodeIndex] = functionCode;
        // 起始地址
        if (!address.TryToByte(destination[layout.AddressRange], Endianness.BigEndian)) return false;
        // 寄存器数量
        if (!quantity.TryToByte(destination[layout.QuantityRange], Endianness.BigEndian)) return false;
        // 数据长度
        destination[layout.DataLengthIndex] = (byte)layout.DataByteLength;

        return true;
    }
    private static bool TryUnpack(
        in ModbusRtuWriteMultipleRequestLayout layout,
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ModbusFunctionCode functionCode,
        out ushort address,
        out ushort quantity
        )
    {
        slaveId = default;
        functionCode = default;
        address = default;
        quantity = default;

        // 校验包长度
        if (source.Length < layout.TotalLength) return false;

        // 从站Id
        var received_slaveId = source[layout.SlaveIdIndex];
        // 功能码
        if (!ModbusFunctionCode.TryFromCode(source[layout.FunctionCodeIndex], out var received_function_code)) return false;
        // 地址
        if (!source[layout.AddressRange].TryToUInt16(out var received_address, Endianness.BigEndian)) return false;
        // 数量
        if (!source[layout.QuantityRange].TryToUInt16(out var received_quantity, Endianness.BigEndian)) return false;

        // 数据长度
        var data_length = source[layout.DataLengthIndex];
        if (data_length != layout.DataByteLength) return false;

        // 校验Crc
        if (!source[layout.CrcRange].TryToUInt16(out var received_crc, Endianness.LittleEndian)) return false;
        if (!CrcCalculator.Validate(source[layout.PayloadRange], received_crc)) return false;

        slaveId = received_slaveId;
        functionCode = received_function_code;
        address = received_address;
        quantity = received_quantity;

        return true;
    }


    /*------------------- 写多个寄存器 -------------------*/

    public static bool TryPackRegisters(
        in ModbusRtuWriteMultipleRequestLayout layout,
        in Span<byte> destination,
        in byte slaveId,
        in ushort address,
        in ReadOnlySpan<ushort> values
        )
    {
        var quantity = (ushort)values.Length;

        if(TryPack(layout, destination, slaveId, ModbusFunctionCode.WriteMultipleRegisters, address, quantity))
        {
            // 数据
            if (!values.TryToByte(destination[layout.DataRange], Endianness.BigEndian)) return false;
            // 验证
            var crc = CrcCalculator.Calculate(destination[layout.PayloadRange]);
            return crc.TryToByte(destination[layout.CrcRange], Endianness.LittleEndian);
        }

        return false;
    }
    public static bool TryUnpackRegisters(
        in ModbusRtuWriteMultipleRequestLayout layout,
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        Span<ushort> values)
    {
        slaveId = default;
        address = default;

        if (TryUnpack(layout, source, out var received_slaveId, out var function_code, out var received_address, out var received_quantity))
        {
            // 功能码
            if (function_code != ModbusFunctionCode.WriteMultipleRegisters) return false;
            // 数量
            if (values.Length < received_quantity) return false;
            // 数据
            if (!source[layout.DataRange].TryToUInt16(values, Endianness.BigEndian)) return false;

            slaveId = received_slaveId;
            address = received_address;
            return true;
        }

        return false;
    }

    /*------------------- 写多个线圈 -------------------*/

    public static bool TryPackCoils(
        in ModbusRtuWriteMultipleRequestLayout layout,
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ReadOnlySpan<bool> values
        )
    {
        var quantity = (ushort)values.Length;

        if (TryPack(layout, destination, slaveId, ModbusFunctionCode.WriteMultipleCoils, address, quantity))
        {
            // 数据
            if (!values.TryToByte(destination[layout.DataRange], Endianness.LittleEndian)) return false;
            // 验证
            var crc = CrcCalculator.Calculate(destination[layout.PayloadRange]);
            return crc.TryToByte(destination[layout.CrcRange], Endianness.LittleEndian);
        }

        return false;
    }
    public static bool TryUnpackCoils(
        in ModbusRtuWriteMultipleRequestLayout layout,
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        Span<bool> values)
    {
        slaveId = default;
        address = default;

        if(TryUnpack(layout, source, out var received_slaveId, out var functionCode, out var received_address, out var received_quantity))
        {
            // 功能码
            if (functionCode != ModbusFunctionCode.WriteMultipleCoils) return false;
            // 数量
            if (values.Length < received_quantity) return false;
            // 数据
            if (!source[layout.DataRange].TryToBit(values, Endianness.LittleEndian)) return false;

            slaveId = received_slaveId;
            address = received_address;
            return true;
        }

        return false;
    }
}