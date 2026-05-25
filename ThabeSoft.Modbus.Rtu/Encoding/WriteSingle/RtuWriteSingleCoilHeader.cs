using ThabeSoft.Modbus.Encoding.WriteMultiple;
using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Headers.Response;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Encoding.WriteSingle;


/// <summary>
/// Rtu 写单值头
/// </summary>
public readonly record struct RtuWriteSingleCoilHeader : IWriteSingleCoilHeader, ICrcable
{
    public static readonly RtuWriteSingleCoilHeader Empty = default;


    public byte SlaveId { get; }
    public FunctionCode FunctionCode => FunctionCode.WriteSingleCoil;
    public ushort Address { get; }
    public bool Value { get; }
    public ushort Crc { get; }



    [Obsolete("禁止调用构造, 请使用工厂方法")]
    public RtuWriteSingleCoilHeader() { }

    public RtuWriteSingleCoilHeader(byte slaveId, ushort address, bool value, ushort crc)
    {
        SlaveId = slaveId;
        Address = address;
        Value = value;
        Crc = crc;
    }


    public static implicit operator WriteSingleCoilHeader(RtuWriteSingleCoilHeader header)
    {
        return new WriteSingleCoilHeader(header.SlaveId, header.Address, header.Value);
    }


    public override string ToString()
    {
        return $"从站={SlaveId}, 功能码={FunctionCode}, 地址={Address}, 值={Value}, Crc={Crc}";
    }
}