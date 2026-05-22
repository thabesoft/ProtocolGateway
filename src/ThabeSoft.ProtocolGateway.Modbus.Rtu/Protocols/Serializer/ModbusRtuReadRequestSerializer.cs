using System.Net;
using ThabeSoft.IndustrialHub.Modbus.Crc;
using ThabeSoft.ProtocolGateway.Conversion;
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
    IModbusReadInputRegistersRequestSerializer,

    IEncoder<ModbusReadCoilsRequest>, IDecoder<ModbusReadCoilsRequest>
{
    public static readonly ModbusRtuReadRequestSerializer Instance = new(ModbusRtuReadRequestLayout.Instance);



    private readonly ModbusRtuReadRequestLayout _layout;
    private ModbusRtuReadRequestSerializer(in ModbusRtuReadRequestLayout layout) => _layout = layout;


    /*------------------- 共用 -------------------*/

    private Result TryPack(
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
        if (!address.TryToByte(destination[_layout.AddressRange], ByteSwap.BigEndian)) return false;
        if (!quantity.TryToByte(destination[_layout.QuantityRange], ByteSwap.BigEndian)) return false;
        // 验证
        var crc = CrcCalculator.Calculate(destination[_layout.PayloadRange]);
        if (!crc.TryToByte(destination[_layout.CrcRange], ByteSwap.LittleEndian)) return false;

        bytesWritten = _layout.TotalLength;
        return true;
    }

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
        if (!address.TryToByte(destination[_layout.AddressRange], ByteSwap.BigEndian)) return false;
        if (!quantity.TryToByte(destination[_layout.QuantityRange], ByteSwap.BigEndian)) return false;
        // 验证
        var crc = CrcCalculator.Calculate(destination[_layout.PayloadRange]);
        if (!crc.TryToByte(destination[_layout.CrcRange], ByteSwap.LittleEndian)) return false;

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
        if (!source[_layout.AddressRange].TryToUInt16(out var received_address, ByteSwap.BigEndian)) return false;
        // 数量
        if (!source[_layout.QuantityRange].TryToUInt16(out var received_quantity, ByteSwap.BigEndian)) return false;
        // Crc
        if (!source[_layout.CrcRange].TryToUInt16(out var received_crc, ByteSwap.LittleEndian)) return false;
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

    Result<int> IEncoder<ModbusReadCoilsRequest>.Encode(
        in ModbusReadCoilsRequest source,
        Span<byte> destination)
    {
        return TryPack(destination, source.SlaveId, source.FunctionCode, source.Address, source.Quantity, out bytesWritten);
    }
    bool IDecoder<ModbusReadCoilsRequest>.Decode(
        ReadOnlySpan<byte> source,
        out ModbusReadCoilsRequest destination)
    {
        destination = default;

        if (!TryUnpack(source: source, out var slaveId, out var function_code, out var address, out var quantity, out _)) return false;
        if (function_code != ModbusFunctionCode.ReadCoils) return false;

        return ModbusReadCoilsRequest.TryCreateCoils(slaveId, address, quantity, out destination);
    }


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