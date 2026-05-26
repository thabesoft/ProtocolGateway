using ThabeSoft.Modbus;
using ThabeSoft.ProtocolGateway.Converters;

namespace ThabeSoft.ProtocolGateway.Modbus;



public static class ModbusTag
{
    public static ITag<bool> ReadCoils(byte slaveId, ushort address)
        => new ModbusTag<bool>(
            address: ModbusAddress.ReadCoils(slaveId, address),
            length: 1,
            converter: BoolConverter.Instance);

    public static ITag<bool> ReadDiscreteInputs(byte slaveId, ushort address)
        => new ModbusTag<bool>(
            address: ModbusAddress.ReadDiscreteInputs(slaveId, address),
            length: 1,
            converter: BoolConverter.Instance);



    public static ITag<ushort> ReadHoldingRegisterWord(byte slaveId, ushort address)
        => new ModbusTag<ushort>(
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            length: 2,
            converter: WordConverter.BigEndian);

    public static ITag<uint> ReadHoldingRegisterDWord(byte slaveId, ushort address)
        => new ModbusTag<uint>(
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            length: 4,
            converter: DWordConverter.BigEndian);

    public static ITag<ulong> ReadHoldingRegisterQWord(byte slaveId, ushort address)
        => new ModbusTag<ulong>(
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            length: 8,
            converter: QWordConverter.BigEndian);



    public static ITag<ushort> ReadInputRegistersWord(byte slaveId, ushort address)
        => new ModbusTag<ushort>(
            address:  ModbusAddress.ReadInputRegisters(slaveId, address),
            length: 2,
            converter: WordConverter.BigEndian);

    public static ITag<uint> ReadInputRegistersDWord(byte slaveId, ushort address)
        => new ModbusTag<uint>(
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            length: 4,
            converter: DWordConverter.BigEndian);

    public static ITag<ulong> ReadInputRegistersQWord(byte slaveId, ushort address)
        => new ModbusTag<ulong>(
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            length: 8,
            converter: QWordConverter.BigEndian);



    public static ITag<bool> WriteCoilWord(byte slaveId, ushort address)
      => new ModbusTag<bool>(
          address: ModbusAddress.WriteSingleCoil(slaveId, address),
          length: 1,
          converter: BoolConverter.Instance);

    public static ITag<ushort> WriteRegisterWord(byte slaveId, ushort address)
       => new ModbusTag<ushort>(
           address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
           length: 2,
           converter: WordConverter.BigEndian);

    public static ITag<uint> WriteRegisterDWord(byte slaveId, ushort address)
      => new ModbusTag<uint>(
          address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
          length: 4,
          converter: DWordConverter.BigEndian);

    public static ITag<ulong> WriteRegisterQWord(byte slaveId, ushort address)
      => new ModbusTag<ulong>(
          address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
          length: 8,
          converter: QWordConverter.BigEndian);

}


internal sealed class ModbusTag<T>(
    ModbusAddress address,
    int length,
    IValueConverter<T> converter
    ) : ITag<T>
    where T : unmanaged
{
    public IAddress Address => address;
    public int Length => length;
    public IValueConverter<T> Converter => converter;
}