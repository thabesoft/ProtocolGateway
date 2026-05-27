using ThabeSoft.ProtocolGateway.Converters;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// Modbus Tag
/// </summary>
public static class ModbusTag
{
    #region --常规标签--

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
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
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

    #endregion

    #region --路由标签--

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
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
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
           address: ModbusAddress.WriteSingleRegister(slaveId, address),
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

    #endregion
}


/// <summary>
/// 常规标签
/// </summary>
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

/// <summary>
/// 路由标签
/// </summary>
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