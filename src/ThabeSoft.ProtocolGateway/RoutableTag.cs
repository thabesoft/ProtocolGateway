using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Converters;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 标签
/// </summary>


/// <summary>
/// 预设Tag
/// </summary>
public static class RoutableTag
{
    public static IRoutableTag<bool> Bool(ChannelName name, IAddress address)
        => new RoutableTag<bool>(name, address, 1, BoolConverter.Instance);
    public static IRoutableTag<byte> Byte(ChannelName name, IAddress address, BitOrder bitOrder = BitOrder.MSB0)
        => new RoutableTag<byte>(name, address, 1, ByteConverter.From(bitOrder));

    public static IRoutableTag<short> Int16(ChannelName name, IAddress address, Endianness endianness = Endianness.BigEndian)
        => new RoutableTag<short>(name, address, 2, WordConverter.From(endianness));
    public static IRoutableTag<ushort> UInt16(ChannelName name, IAddress address, Endianness endianness = Endianness.BigEndian)
        => new RoutableTag<ushort>(name, address, 2, WordConverter.From(endianness));
    public static IRoutableTag<char> Char(ChannelName name, IAddress address, Endianness endianness = Endianness.BigEndian)
        => new RoutableTag<char>(name, address, 2, WordConverter.From(endianness));


    public static IRoutableTag<int> Int32(ChannelName name, IAddress address, DWordLayout layout = default)
        => new RoutableTag<int>(name, address, 4, new DWordConverter(layout));
    public static IRoutableTag<uint> UInt32(ChannelName name, IAddress address, DWordLayout layout = default)
        => new RoutableTag<uint>(name, address, 4, new DWordConverter(layout));
    public static IRoutableTag<float> Float(ChannelName name, IAddress address, DWordLayout layout = default)
        => new RoutableTag<float>(name, address, 4, new DWordConverter(layout));


    public static IRoutableTag<long> Int64(ChannelName name, IAddress address, QWordLayout layout = default)
        => new RoutableTag<long>(name, address, 8, new QWordConverter(layout));
    public static IRoutableTag<ulong> UInt64(ChannelName name, IAddress address, QWordLayout layout = default)
        => new RoutableTag<ulong>(name, address, 8, new QWordConverter(layout));
    public static IRoutableTag<double> Double(ChannelName name, IAddress address, QWordLayout layout = default)
        => new RoutableTag<double>(name, address, 8, new QWordConverter(layout));
}



internal sealed class RoutableTag<TValue> : IRoutableTag<TValue>
    where TValue : unmanaged
{
    public ChannelName ChannelName { get; }
    public IAddress Address { get; }
    public int Length { get; }
    public IValueConverter<TValue> Converter { get; }


    internal RoutableTag(ChannelName name, IAddress address, int length, IValueConverter<TValue> converter)
    {
        ChannelName = name;
        Address = address;
        Length = length;
        Converter = converter;
    }
}