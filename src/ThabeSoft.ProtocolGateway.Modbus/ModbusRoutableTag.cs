using System.Xml.Linq;
using ThabeSoft.Modbus;
using ThabeSoft.ProtocolGateway.Converters;

namespace ThabeSoft.ProtocolGateway.Modbus;



public static class ModbusRoutableTag
{
    public static IRoutableTag<bool> ReadCoils(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<bool>(
            channelName: name,
            address: ModbusAddress.ReadCoils(slaveId, address),
            length: 1,
            converter: BoolConverter.Instance);

    public static IRoutableTag<bool> ReadDiscreteInputs(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<bool>(
            channelName: name,
            address: ModbusAddress.ReadDiscreteInputs(slaveId, address),
            length: 1,
            converter: BoolConverter.Instance);



    public static IRoutableTag<ushort> ReadHoldingRegisterWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<ushort>(
            channelName: name,
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            length: 2,
            converter: WordConverter.BigEndian);

    public static IRoutableTag<uint> ReadHoldingRegisterDWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<uint>(
            channelName: name,
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            length: 4,
            converter: DWordConverter.BigEndian);

    public static IRoutableTag<ulong> ReadHoldingRegisterQWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<ulong>(
            channelName: name,
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            length: 8,
            converter: QWordConverter.BigEndian);



    public static IRoutableTag<ushort> ReadInputRegistersWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<ushort>(
            channelName: name,
            address:  ModbusAddress.ReadInputRegisters(slaveId, address),
            length: 2,
            converter: WordConverter.BigEndian);

    public static IRoutableTag<uint> ReadInputRegistersDWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<uint>(
            channelName: name,
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            length: 4,
            converter: DWordConverter.BigEndian);

    public static IRoutableTag<ulong> ReadInputRegistersQWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<ulong>(
            channelName: name,
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            length: 8,
            converter: QWordConverter.BigEndian);



    public static IRoutableTag<bool> WriteCoilWord(ChannelName name, byte slaveId, ushort address)
      => new ModbusRoutableTag<bool>(
          channelName: name,
          address: ModbusAddress.WriteSingleCoil(slaveId, address),
          length: 1,
          converter: BoolConverter.Instance);

    public static IRoutableTag<ushort> WriteRegisterWord(ChannelName name, byte slaveId, ushort address)
       => new ModbusRoutableTag<ushort>(
           channelName: name,
           address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
           length: 2,
           converter: WordConverter.BigEndian);

    public static IRoutableTag<uint> WriteRegisterDWord(ChannelName name, byte slaveId, ushort address)
      => new ModbusRoutableTag<uint>(
          channelName: name,
          address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
          length: 4,
          converter: DWordConverter.BigEndian);

    public static IRoutableTag<ulong> WriteRegisterQWord(ChannelName name, byte slaveId, ushort address)
      => new ModbusRoutableTag<ulong>(
          channelName: name,
          address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
          length: 8,
          converter: QWordConverter.BigEndian);

}


internal sealed class ModbusRoutableTag<T>(
    ChannelName channelName,
    ModbusAddress address,
    int length,
    IValueConverter<T> converter
    ) : IRoutableTag<T>
    where T : unmanaged
{
    public ChannelName ChannelName => channelName;
    public IAddress Address => address;
    public int Length => length;
    public IValueConverter<T> Converter => converter;
}