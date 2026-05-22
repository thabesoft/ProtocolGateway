namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 字
/// </summary>
public readonly struct Word : IEquatable<Word>
{
    public static readonly Word Zero = default;


    private readonly ushort _value;
    private Word(ushort value) => _value = value;


    public static Word FromUInt16(ushort value)
    {
        return new Word(value);
    }
    public static Word FromInt16(short value)
    {
        return FromUInt16((ushort)value);
    }
    public static Word FromBytes(byte high, byte low)
    {
        return new Word((ushort)((high << 8) | low));
    }



    public static implicit operator ushort(Word word) => word._value;
    public static implicit operator short(Word word) => (short)word._value;
    public static implicit operator char(Word word) => (char)word._value;

    public static bool operator ==(Word left, Word right) => left._value == right._value;
    public static bool operator !=(Word left, Word right) => left._value != right._value;


    /// <summary>
    /// 调换高低字节, 返回新的Word
    /// </summary>
    public Word ByteSwap() => new((ushort)((_value >> 8) | ((_value & 0xFF) << 8)));

    public ushort ToUInt16() => _value;
    public short ToInt16() => (short)_value;
    public char ToChar() => (char)_value;


    public override bool Equals(object? obj) => obj is Word word && Equals(word);
    public bool Equals(Word other) => other._value == _value;
    public override int GetHashCode() => _value.GetHashCode();
    public override string ToString() => _value.ToString();
}