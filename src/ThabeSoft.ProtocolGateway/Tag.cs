using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Converters;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 预设Tag
/// </summary>
public static class Tag
{
    #region --地址标签--

    public static ITag<bool> Bool(IAddress address)
      => new Tag<bool>(address, 1, BoolConverter.Instance);
    public static ITag<byte> Byte(IAddress address, BitOrder bitOrder = BitOrder.MSB0)
        => new Tag<byte>(address, 1, ByteConverter.From(bitOrder));


    public static ITag<short> Int16(IAddress address, Endianness endianness = Endianness.BigEndian)
        => new Tag<short>(address, 2, WordConverter.From(endianness));
    public static ITag<ushort> UInt16(IAddress address, Endianness endianness = Endianness.BigEndian)
        => new Tag<ushort>(address, 2, WordConverter.From(endianness));
    public static ITag<char> Char(IAddress address, Endianness endianness = Endianness.BigEndian)
        => new Tag<char>(address, 2, WordConverter.From(endianness));


    public static ITag<int> Int32(IAddress address, DWordLayout layout = default)
        => new Tag<int>(address, 4, new DWordConverter(layout));
    public static ITag<uint> UInt32(IAddress address, DWordLayout layout = default)
        => new Tag<uint>(address, 4, new DWordConverter(layout));
    public static ITag<float> Float(IAddress address, DWordLayout layout = default)
        => new Tag<float>(address, 4, new DWordConverter(layout));


    public static ITag<long> Int64(IAddress address, QWordLayout layout = default)
        => new Tag<long>(address, 8, new QWordConverter(layout));
    public static ITag<ulong> UInt64(IAddress address, QWordLayout layout = default)
        => new Tag<ulong>(address, 8, new QWordConverter(layout));
    public static ITag<double> Double(IAddress address, QWordLayout layout = default)
        => new Tag<double>(address, 8, new QWordConverter(layout));

    #endregion

    #region --路由标签--

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

    #endregion
}



/// <summary>
/// 地址标签
/// </summary>
internal sealed class Tag<TValue> : IRoutableTag<TValue>
    where TValue : unmanaged
{
    public ChannelName ChannelName { get; }
    public IAddress Address { get; }
    public int Length { get; }
    public IValueConverter<TValue> Converter { get; }


    internal Tag(IAddress address, int length, IValueConverter<TValue> converter)
    {
        Address = address;
        Length = length;
        Converter = converter;
    }
}
/// <summary>
/// 路由地址标签
/// </summary>
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