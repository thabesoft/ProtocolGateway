using ThabeSoft.Modbus;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// Modbus Tag
/// </summary>
public static class ModbusTagFactory
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

    public static Result<ITag> CreateRoutableTag(ChannelName name, byte slaveId, FunctionCode functionCode, ushort address, TagValueType valueType)
    {
        var data_length_result = valueType.GetByteLength();
        if (!data_length_result.IsSuccess) return data_length_result.Cast<ITag>();

        var modbus_address = ModbusAddress.Create(slaveId, functionCode, address);

        // 8
        if (valueType == TagValueType.Bool)
        {
            if(!functionCode.IsCoil)
            {
                return Result.Error<ITag>($"非线圈功能码 [{functionCode}], 无法使用该数据类型 [{valueType}]");
            }

            return Result.Success<ITag>(Create<bool>(BoolConverter.Instance));
        }

        // 功能码不匹配
        if (!functionCode.IsRegister)
        {
            return Result.Error<ITag>($"非寄存器功能码 [{functionCode}], 无法使用该数据类型 [{valueType}]");
        }

        if (valueType == TagValueType.Byte)
        {
            return Result.Success<ITag>(Create<byte>(ByteConverter.LSB0));
        }
        if (valueType == TagValueType.SByte)
        {
            return Result.Success<ITag>(Create<sbyte>(ByteConverter.LSB0));
        }

        // 16
        if (valueType == TagValueType.Int16)
        {
            return Result.Success<ITag>(Create<short>(WordConverter.BigEndian));
        }
        if (valueType == TagValueType.UInt16)
        {
            return Result.Success<ITag>(Create<ushort>(WordConverter.BigEndian));
        }
        if (valueType == TagValueType.Char)
        {
            return Result.Success<ITag>(Create<char>(WordConverter.BigEndian));
        }

        // 32
        if (valueType == TagValueType.Int32)
        {
            return Result.Success<ITag>(Create<int>(DWordConverter.BigEndian));
        }
        if (valueType == TagValueType.UInt32)
        {
            return Result.Success<ITag>(Create<uint>(DWordConverter.BigEndian));
        }
        if (valueType == TagValueType.Float)
        {
            return Result.Success<ITag>(Create<float>(DWordConverter.BigEndian));
        }

        // 64
        if (valueType == TagValueType.Int64)
        {
            return Result.Success<ITag>(Create<long>(QWordConverter.BigEndian));
        }
        if (valueType == TagValueType.UInt64)
        {
            return Result.Success<ITag>(Create<ulong>(QWordConverter.BigEndian));
        }
        if (valueType == TagValueType.Double)
        {
            return Result.Success<ITag>(Create<double>(QWordConverter.BigEndian));
        }

        return Result.Error<ITag>($"Modbus标签创建失败, 不支持的数据类型:{valueType}");


        IRoutableTag<T> Create<T>(IBinarySerializer<T> converter) where T : unmanaged
        {
            return new ModbusRoutableTag<T>(
               channelName: name,
               address: modbus_address,
               dataType: valueType,
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
    IBinarySerializer<T> converter
    ) : ITag<T>
    where T : unmanaged
{
    public IAddress Address => address;
    public TagValueType ValueType => dataType;
    public int Length => length;
    public IBinarySerializer<T> Converter => converter;
}

/// <summary>
/// 路由标签
/// </summary>
internal sealed class ModbusRoutableTag<T>(
    ChannelName channelName,
    ModbusAddress address,
    TagValueType dataType,
    int length,
    IBinarySerializer<T> converter
    ) : IRoutableTag<T>
    where T : unmanaged
{
    public ChannelName ChannelName => channelName;
    public IAddress Address => address;
    public TagValueType ValueType => dataType;
    public int Length => length;
    public IBinarySerializer<T> Converter => converter;
}