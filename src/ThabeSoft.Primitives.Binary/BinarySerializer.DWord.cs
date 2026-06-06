namespace ThabeSoft.Primitives.Binary;


/// <summary>
/// 32 位数据转换
/// </summary>
public sealed class DWordSerializer(DWordLayout layout) :
    IBinarySerializer<uint>,
    IBinarySerializer<int>,
    IBinarySerializer<float>
{
    public static DWordSerializer BigEndian { get; } = new(DWordLayout.BigEndian);
    public static DWordSerializer LittleEndian { get; } = new(DWordLayout.LittleEndian);


    private Result<uint> From(ReadOnlySpan<byte> source)
    {
        if (source.Length < 4) return Result.Error<uint>( "双字至少需要4字节");

        Span<byte> destination = stackalloc byte[4];
        source.Swap(destination, layout);
        return destination.ToDWord(layout);
    }
    private Result To(uint value, Span<byte> destination)
    {
        return value.ToBytes(destination, layout);
    }



    Result<uint> IBinarySerializer<uint>.Deserialize(ReadOnlySpan<byte> source) => From(source);
    Result<int> IBinarySerializer<int>.Deserialize(ReadOnlySpan<byte> source) => From(source).Select(x => (int)x);
    Result<float> IBinarySerializer<float>.Deserialize(ReadOnlySpan<byte> source) => From(source).Select(x => (float)x);

    

    Result IBinarySerializer<uint>.Serialize(uint source, Span<byte> destination) => To(source, destination);
    Result IBinarySerializer<int>.Serialize(int source, Span<byte> destination) => To((ushort)source, destination);
    Result IBinarySerializer<float>.Serialize(float source, Span<byte> destination) => To((ushort)source, destination);
}
