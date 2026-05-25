using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 写多值响应头
/// </summary>
public readonly record struct RtuWriteMultipleResponseHeader
{
    public static readonly RtuWriteMultipleResponseHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode { get; }
    public ushort Address { get; }
    public ushort Quantity { get; }
    public ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuWriteMultipleResponseHeader() { }
    private RtuWriteMultipleResponseHeader(byte slaveId, FunctionCode functionCode, ushort address, ushort quantity, ushort crc)
    {
        SlaveId = slaveId;
        FunctionCode = functionCode;
        Address = address;
        Quantity = quantity;
        Crc = crc;
    }


    public static implicit operator WriteMultipleResponseHeader(RtuWriteMultipleResponseHeader header)
    {
        if(header.FunctionCode == FunctionCode.WriteMultipleCoils)
        {
            return WriteMultipleResponseHeader.Coils(header.SlaveId, header.Address, header.Quantity).Value;
        }

        return WriteMultipleResponseHeader.Registers(header.SlaveId, header.Address, header.Quantity).Value;
    }


    public static Result<RtuWriteMultipleResponseHeader> Coils(byte slaveId, ushort address, int quantity, ushort crc)
    {
        var quantity_result = ReadCoilsQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<RtuWriteMultipleResponseHeader>();

        return new RtuWriteMultipleResponseHeader(slaveId, FunctionCode.WriteMultipleCoils, address, quantity_result.Value, crc);
    }
    public static RtuWriteMultipleResponseHeader Coils(byte slaveId, ushort address, ReadCoilsQuantity quantity, ushort crc)
    {
        return new RtuWriteMultipleResponseHeader(slaveId, FunctionCode.WriteMultipleCoils, address, quantity, crc);
    }


    public static Result<RtuWriteMultipleResponseHeader> Registers(byte slaveId, ushort address, int quantity, ushort crc)
    {
        var quantity_result = ReadRegistersQuantity.Create(quantity);
        if (!quantity_result) return quantity_result.PropagateError<RtuWriteMultipleResponseHeader>();

        return new RtuWriteMultipleResponseHeader(slaveId, FunctionCode.WriteMultipleRegisters, address, quantity_result.Value, crc);
    }
    public static RtuWriteMultipleResponseHeader Registers(byte slaveId, ushort address, ReadRegistersQuantity quantity, ushort crc)
    {
        return new RtuWriteMultipleResponseHeader(slaveId, FunctionCode.WriteMultipleRegisters, address, quantity, crc);
    }


    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address},数量={Quantity} Crc={Crc}";
    }
}