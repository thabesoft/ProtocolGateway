using ThabeSoft.ProtocolGateway.Builders;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 标签
/// </summary>
public sealed class Tag<TValue> : ITag<TValue>
    where TValue : unmanaged
{
    public IAddress Address { get; }
    public int Length { get; }
    public IByteConverter<TValue> Converter { get; }

    internal Tag(IAddress address, int length, IByteConverter<TValue> converter)
    {
        Address = address;
        Length = length;
        Converter = converter;
    }
}

/// <summary>
/// 预设Tag
/// </summary>
public static class Tag
{
    public static Tag<bool> Bool(IAddress address)
        => new(address, 1, BoolConverter.Instance);
    public static Tag<byte> Byte(IAddress address)
        => new(address, 1, PassthroughConverter.Instance);

    public static Tag<short> Int16(IAddress address, WordLayout wordLayout = default)
        => new(address, 2, new WordConverter(wordLayout));
    public static Tag<ushort> UInt16(IAddress address, WordLayout wordLayout = default)
        => new(address, 2, new WordConverter(wordLayout));
    public static Tag<char> Char(IAddress address, WordLayout wordLayout = default)
        => new(address, 2, new WordConverter(wordLayout));


    public static Tag<int> Int32(IAddress address, DWordLayout layout = default)
        => new(address, 4, new DWordConverter(layout));
    public static Tag<uint> UInt32(IAddress address, DWordLayout layout = default)
        => new(address, 4, new DWordConverter(layout));
    public static Tag<float> Float(IAddress address, DWordLayout layout = default)
        => new(address, 4, new DWordConverter(layout));

    public static Tag<long> Int64(IAddress address, QWordLayout layout = default)
        => new(address, 8, new QWordConverter(layout));
    public static Tag<ulong> UInt64(IAddress address, QWordLayout layout = default)
        => new(address, 8, new QWordConverter(layout));
    public static Tag<double> Double(IAddress address, QWordLayout layout = default)
        => new(address, 8, new QWordConverter(layout));
}