using ThabeSoft.ProtocolGateway.Modbus.Crc;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Protocols;
using ThabeSoft.ProtocolGateway.Protocols.Layouts;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;

/// <summary>
/// Modbus Rtu 读请求解码器
/// </summary>
public sealed class RtuReadRequestDecoder :
    IDecoder<ModbusReadCoilsRequest>,
    IDecoder<ModbusReadDiscreteInputsRequest>,
    IDecoder<ModbusReadHoldingRegistersRequest>,
    IDecoder<ModbusReadInputRegistersRequest>
{
    public static readonly RtuReadRequestDecoder Instance = new(ModbusRtuReadRequestLayout.Instance);


    private readonly ModbusRtuReadRequestLayout _layout;
    private RtuReadRequestDecoder(in ModbusRtuReadRequestLayout layout) => _layout = layout;


    Result<ModbusReadCoilsRequest> IDecoder<ModbusReadCoilsRequest>.Decode(ReadOnlySpan<byte> source)
    {
        return TryUnpack(source, ModbusFunctionCode.ReadCoils)
            .Then(x => ModbusReadCoilsRequest.Create(x.SlaveId, x.Address, x.Quantity));
    }
    Result<ModbusReadInputRegistersRequest> IDecoder<ModbusReadInputRegistersRequest>.Decode(ReadOnlySpan<byte> source)
    {
        return TryUnpack(source, ModbusFunctionCode.ReadDiscreteInputs)
            .Then(x => ModbusReadInputRegistersRequest.Create(x.SlaveId, x.Address, x.Quantity));
    }

    Result<ModbusReadHoldingRegistersRequest> IDecoder<ModbusReadHoldingRegistersRequest>.Decode(ReadOnlySpan<byte> source)
    {
        return TryUnpack(source, ModbusFunctionCode.ReadHoldingRegisters)
            .Then(x => ModbusReadHoldingRegistersRequest.Create(x.SlaveId, x.Address, x.Quantity));
    }

    Result<ModbusReadDiscreteInputsRequest> IDecoder<ModbusReadDiscreteInputsRequest>.Decode(ReadOnlySpan<byte> source)
    {
        return TryUnpack(source, ModbusFunctionCode.ReadInputRegisters)
            .Then(x => ModbusReadDiscreteInputsRequest.Create(x.SlaveId, x.Address, x.Quantity));
    }


    private Result<UnpackData> TryUnpack(ReadOnlySpan<byte> source, ModbusFunctionCode functionCode)
    {
        if (source.Length < _layout.TotalLength)
        {
            return Result<UnpackData>.Error(ErrorType.InvalidData,
                $"响应数据长度不足，需要 {_layout.TotalLength} 字节，实际 {source.Length} 字节");
        }

        // 从站
        var slaveId = source[_layout.SlaveIdIndex];
        // 功能码
        var function_code_result = ModbusFunctionCode
            .FromCode(source[_layout.FunctionCodeIndex])
            .Where(x => x == functionCode);
        if (!function_code_result) return function_code_result.PropagateError<UnpackData>();

        // 起始地址
        var address_result = source[_layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<UnpackData>();

        // 数量
        var quantity_result = source[_layout.QuantityRange]
            .ToWord(Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<UnpackData>();

        // Crc
        var crc_result = source[_layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<UnpackData>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[_layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<UnpackData>();


        return new UnpackData(slaveId, function_code_result.Value, quantity_result.Value, crc_result.Value);
    }

    private readonly struct UnpackData(byte slaveId, ushort address, ushort quantity, ushort crc)
    {
        public byte SlaveId => slaveId;
        public ushort Address => address;
        public ushort Quantity => quantity;
        public ushort Crc => crc;
    }
}