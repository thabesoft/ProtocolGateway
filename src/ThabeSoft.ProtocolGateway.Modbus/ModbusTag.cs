using ThabeSoft.Modbus;
using ThabeSoft.Primitives;

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
            TagValueType.Bool,
            length: 1,
            converter: BoolConverter.Instance);

    public static ITag<bool> ReadDiscreteInputs(byte slaveId, ushort address)
        => new ModbusTag<bool>(
            address: ModbusAddress.ReadDiscreteInputs(slaveId, address),
            TagValueType.Bool,
            length: 1,
            converter: BoolConverter.Instance);



    public static ITag<ushort> ReadHoldingRegisterWord(byte slaveId, ushort address)
        => new ModbusTag<ushort>(
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            TagValueType.UInt16,
            length: 2,
            converter: WordConverter.BigEndian);

    public static ITag<uint> ReadHoldingRegisterDWord(byte slaveId, ushort address)
        => new ModbusTag<uint>(
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            TagValueType.UInt32,
            length: 4,
            converter: DWordConverter.BigEndian);

    public static ITag<ulong> ReadHoldingRegisterQWord(byte slaveId, ushort address)
        => new ModbusTag<ulong>(
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            TagValueType.UInt64,
            length: 8,
            converter: QWordConverter.BigEndian);



    public static ITag<ushort> ReadInputRegistersWord(byte slaveId, ushort address)
        => new ModbusTag<ushort>(
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            TagValueType.UInt16,
            length: 2,
            converter: WordConverter.BigEndian);

    public static ITag<uint> ReadInputRegistersDWord(byte slaveId, ushort address)
        => new ModbusTag<uint>(
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            TagValueType.UInt32,
            length: 4,
            converter: DWordConverter.BigEndian);

    public static ITag<ulong> ReadInputRegistersQWord(byte slaveId, ushort address)
        => new ModbusTag<ulong>(
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            TagValueType.UInt64,
            length: 8,
            converter: QWordConverter.BigEndian);



    public static ITag<bool> WriteCoilWord(byte slaveId, ushort address)
      => new ModbusTag<bool>(
          address: ModbusAddress.WriteSingleCoil(slaveId, address),
          TagValueType.Bool,
          length: 1,
          converter: BoolConverter.Instance);

    public static ITag<ushort> WriteRegisterWord(byte slaveId, ushort address)
       => new ModbusTag<ushort>(
           address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
           TagValueType.UInt16,
           length: 2,
           converter: WordConverter.BigEndian);

    public static ITag<uint> WriteRegisterDWord(byte slaveId, ushort address)
      => new ModbusTag<uint>(
          address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
          TagValueType.UInt32,
          length: 4,
          converter: DWordConverter.BigEndian);

    public static ITag<ulong> WriteRegisterQWord(byte slaveId, ushort address)
      => new ModbusTag<ulong>(
          address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
          TagValueType.UInt64,
          length: 8,
          converter: QWordConverter.BigEndian);

    #endregion

    #region --路由标签--

    public static Result<ITag> CreateRoutableTag(ChannelName name, byte slaveId, FunctionCode functionCode, ushort address, TagValueType dataType)
    {
        var data_length_result = dataType.GetByteLength();
        if (!data_length_result.IsSuccess) return data_length_result.PropagateError<ITag>();

        var modbus_address = ModbusAddress.Create(slaveId, functionCode, address);

        // 8
        if (dataType == TagValueType.Bool)
        {
            return Result.Ok<ITag>(Create<bool>(BoolConverter.Instance));
        }
        if (dataType == TagValueType.Byte)
        {
            return Result.Ok<ITag>(Create<byte>(ByteConverter.LSB0));
        }
        if (dataType == TagValueType.SByte)
        {
            return Result.Ok<ITag>(Create<sbyte>(ByteConverter.LSB0));
        }

        // 16
        if (dataType == TagValueType.Int16)
        {
            return Result.Ok<ITag>(Create<short>(WordConverter.BigEndian));
        }
        if (dataType == TagValueType.UInt16)
        {
            return Result.Ok<ITag>(Create<ushort>(WordConverter.BigEndian));
        }
        if (dataType == TagValueType.Char)
        {
            return Result.Ok<ITag>(Create<char>(WordConverter.BigEndian));
        }

        // 32
        if (dataType == TagValueType.Int32)
        {
            return Result.Ok<ITag>(Create<int>(DWordConverter.BigEndian));
        }
        if (dataType == TagValueType.UInt32)
        {
            return Result.Ok<ITag>(Create<uint>(DWordConverter.BigEndian));
        }
        if (dataType == TagValueType.Float)
        {
            return Result.Ok<ITag>(Create<float>(DWordConverter.BigEndian));
        }

        // 64
        if (dataType == TagValueType.Int64)
        {
            return Result.Ok<ITag>(Create<long>(QWordConverter.BigEndian));
        }
        if (dataType == TagValueType.UInt64)
        {
            return Result.Ok<ITag>(Create<ulong>(QWordConverter.BigEndian));
        }
        if (dataType == TagValueType.Double)
        {
            return Result.Ok<ITag>(Create<double>(QWordConverter.BigEndian));
        }

        return Result.NotSupported<ITag>($"Modbus标签创建失败, 不支持的数据类型:{dataType}");


        IRoutableTag<T> Create<T>(IValueConverter<T> converter) where T : unmanaged
        {
            return new ModbusRoutableTag<T>(
               channelName: name,
               address: modbus_address,
               dataType: dataType,
               length: data_length_result.Value,
               converter: converter);
        }
    }

    public static IRoutableTag<bool> ReadCoils(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<bool>(
            channelName: name,
            address: ModbusAddress.ReadCoils(slaveId, address),
            TagValueType.Bool,
            length: 1,
            converter: BoolConverter.Instance);

    public static IRoutableTag<bool> ReadDiscreteInputs(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<bool>(
            channelName: name,
            address: ModbusAddress.ReadDiscreteInputs(slaveId, address),
            TagValueType.Bool,
            length: 1,
            converter: BoolConverter.Instance);



    public static IRoutableTag<ushort> ReadHoldingRegisterWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<ushort>(
            channelName: name,
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            TagValueType.UInt16,
            length: 2,
            converter: WordConverter.BigEndian);

    public static IRoutableTag<uint> ReadHoldingRegisterDWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<uint>(
            channelName: name,
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            TagValueType.UInt32,
            length: 4,
            converter: DWordConverter.BigEndian);

    public static IRoutableTag<ulong> ReadHoldingRegisterQWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<ulong>(
            channelName: name,
            address: ModbusAddress.ReadHoldingRegisters(slaveId, address),
            TagValueType.UInt64,
            length: 8,
            converter: QWordConverter.BigEndian);



    public static IRoutableTag<ushort> ReadInputRegistersWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<ushort>(
            channelName: name,
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            TagValueType.UInt16,
            length: 2,
            converter: WordConverter.BigEndian);

    public static IRoutableTag<uint> ReadInputRegistersDWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<uint>(
            channelName: name,
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            TagValueType.UInt32,
            length: 4,
            converter: DWordConverter.BigEndian);

    public static IRoutableTag<ulong> ReadInputRegistersQWord(ChannelName name, byte slaveId, ushort address)
        => new ModbusRoutableTag<ulong>(
            channelName: name,
            address: ModbusAddress.ReadInputRegisters(slaveId, address),
            TagValueType.UInt64,
            length: 8,
            converter: QWordConverter.BigEndian);



    public static IRoutableTag<bool> WriteCoilWord(ChannelName name, byte slaveId, ushort address)
      => new ModbusRoutableTag<bool>(
          channelName: name,
          address: ModbusAddress.WriteSingleCoil(slaveId, address),
          TagValueType.Bool,
          length: 1,
          converter: BoolConverter.Instance);

    public static IRoutableTag<ushort> WriteRegisterWord(ChannelName name, byte slaveId, ushort address)
       => new ModbusRoutableTag<ushort>(
           channelName: name,
           address: ModbusAddress.WriteSingleRegister(slaveId, address),
           TagValueType.UInt16,
           length: 2,
           converter: WordConverter.BigEndian);

    public static IRoutableTag<uint> WriteRegisterDWord(ChannelName name, byte slaveId, ushort address)
      => new ModbusRoutableTag<uint>(
          channelName: name,
          address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
          TagValueType.UInt32,
          length: 4,
          converter: DWordConverter.BigEndian);

    public static IRoutableTag<ulong> WriteRegisterQWord(ChannelName name, byte slaveId, ushort address)
      => new ModbusRoutableTag<ulong>(
          channelName: name,
          address: ModbusAddress.WriteMultipleRegisters(slaveId, address),
          TagValueType.UInt64,
          length: 8,
          converter: QWordConverter.BigEndian);

    #endregion
}


/// <summary>
/// 常规标签
/// </summary>
internal sealed class ModbusTag<T>(
    ModbusAddress address,
    TagValueType dataType,
    int length,
    IValueConverter<T> converter
    ) : ITag<T>
    where T : unmanaged
{
    public IAddress Address => address;
    public TagValueType ValueType => dataType;
    public int Length => length;
    public IValueConverter<T> Converter => converter;

}

/// <summary>
/// 路由标签
/// </summary>
internal sealed class ModbusRoutableTag<T>(
    ChannelName channelName,
    ModbusAddress address,
    TagValueType dataType,
    int length,
    IValueConverter<T> converter
    ) : IRoutableTag<T>
    where T : unmanaged
{
    public ChannelName ChannelName => channelName;
    public IAddress Address => address;
    public TagValueType ValueType => dataType;
    public int Length => length;
    public IValueConverter<T> Converter => converter;
}