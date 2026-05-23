using System.Net;
using ThabeSoft.IndustrialHub.Modbus.Crc;
using ThabeSoft.ProtocolGateway.Conversion;
using ThabeSoft.ProtocolGateway.Modbus.Crc;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Protocols.Layouts;

namespace ThabeSoft.ProtocolGateway.Protocols.Serializer;


/// <summary>
/// Rtu读帧布局
/// </summary>
public sealed class ModbusRtuReadRequestSerializer :
    IModbusReadCoilsRequestSerializer,
    IModbusReadDiscreteInputsRequestSerializer,
    IModbusReadHoldingRegistersRequestSerializer,
    IModbusReadInputRegistersRequestSerializer
{
    public static readonly ModbusRtuReadRequestSerializer Instance = new(ModbusRtuReadRequestLayout.Instance);



    private readonly ModbusRtuReadRequestLayout _layout;
    private ModbusRtuReadRequestSerializer(in ModbusRtuReadRequestLayout layout) => _layout = layout;


    /*------------------- 共用 -------------------*/

    private bool TryPack(
        Span<byte> destination,
        byte slaveId,
        ModbusFunctionCode function,
        ushort address,
        ushort quantity,
        out int bytesWritten
        )
    {
        bytesWritten = 0;

        // 缓冲区长度不足
        if (destination.Length < _layout.TotalLength) return false;
        if (!function.IsRead) return false;

        // 从站
        destination[_layout.SlaveIdIndex] = slaveId;
        // 功能码
        destination[_layout.FunctionCodeIndex] = function;
        // 起始地址
        if (!address.TryToByte(destination[_layout.AddressRange], Endianness.BigEndian)) return false;
        if (!quantity.TryToByte(destination[_layout.QuantityRange], Endianness.BigEndian)) return false;
        // 验证
        var crc = CrcCalculator.Calculate(destination[_layout.PayloadRange]);
        if (!crc.TryToByte(destination[_layout.CrcRange], Endianness.LittleEndian)) return false;

        bytesWritten = _layout.TotalLength;
        return true;
    }
    private bool TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ModbusFunctionCode functionCode,
        out ushort address,
        out ushort quantity,
        out ushort crc
        )
    {
        slaveId = default;
        functionCode = default;
        address = default;
        quantity = default;
        crc = default;

        // 缓冲区长度不足
        if (source.Length < _layout.TotalLength) return false;

        // 从站
        var received_slaveId = source[_layout.SlaveIdIndex];
        // 功能码
        if (!ModbusFunctionCode.TryFromCode(source[_layout.FunctionCodeIndex], out var received_function_code)) return false;
        if (!received_function_code.IsRead) return false;
        // 起始地址
        if (!source[_layout.AddressRange].TryToUInt16(out var received_address, Endianness.BigEndian)) return false;
        // 数量
        if (!source[_layout.QuantityRange].TryToUInt16(out var received_quantity, Endianness.BigEndian)) return false;
        // Crc
        if (!source[_layout.CrcRange].TryToUInt16(out var received_crc, Endianness.LittleEndian)) return false;
        // 验证
        if (!CrcCalculator.Validate(source[_layout.PayloadRange], received_crc)) return false;

        slaveId = received_slaveId;
        functionCode = received_function_code;
        address = received_address;
        quantity = received_quantity;
        crc = received_crc;

        return true;
    }


    /*------------------- 线圈 -------------------*/

    bool IModbusReadCoilsRequestSerializer.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, ModbusFunctionCode.ReadCoils, address, quantity, out _);
    }
    bool IModbusReadCoilsRequestSerializer.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort quantity)
    {
        return TryUnpack(source: source, out slaveId, out _, out address, out quantity, out _);
    }

    /*------------------- 离散输入 -------------------*/

    bool IModbusReadDiscreteInputsRequestSerializer.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, ModbusFunctionCode.ReadDiscreteInputs, address, quantity, out _);
    }
    bool IModbusReadDiscreteInputsRequestSerializer.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort quantity)
    {
        return TryUnpack(source: source, out slaveId, out _, out address, out quantity, out _);
    }

    /*------------------- 保持寄存器 -------------------*/

    bool IModbusReadHoldingRegistersRequestSerializer.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, ModbusFunctionCode.ReadHoldingRegisters, address, quantity, out _);
    }
    bool IModbusReadHoldingRegistersRequestSerializer.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort quantity)
    {
        return TryUnpack(source: source, out slaveId, out _, out address, out quantity, out _);
    }

    /*------------------- 输入寄存器 -------------------*/

    bool IModbusReadInputRegistersRequestSerializer.TryPack(
        Span<byte> destination,
        byte slaveId,
        ushort address,
        ushort quantity)
    {
        return TryPack(destination, slaveId, ModbusFunctionCode.ReadInputRegisters, address, quantity, out _);
    }
    bool IModbusReadInputRegistersRequestSerializer.TryUnpack(
        ReadOnlySpan<byte> source,
        out byte slaveId,
        out ushort address,
        out ushort quantity)
    {
        return TryUnpack(source: source, out slaveId, out _, out address, out quantity, out _);
    }
}