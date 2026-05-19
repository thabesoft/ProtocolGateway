namespace IndustrialHub.Modbus.Bits;


/// <summary>
/// 8位 Bit
/// </summary>
public readonly struct Bits8 : IBits<Bits8>
{
    public const int MaxIndex = 7;
    public const int MaxLength = 8;


    public static Bits8 Empty = default;



    private readonly byte _value;
    private Bits8(byte value) => _value = value;


    public static Bits8 FromBits(ReadOnlySpan<bool> bits, bool bigEndian = false)
    {
        if (bits.Length < 0 || bits.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(bits.Length), $"位长度范围在必须在 [0, {MaxLength}] 之间");
        }

        byte result = 0;

        for (int i = 0; i < bits.Length; i++)
        {
            if (!bits[i]) continue;

            int bitIndex = bigEndian ? MaxLength - 1 - i : i;
            result |= (byte)(1 << bitIndex);
        }

        return new Bits8(result);
    }

    public static implicit operator Bits8(byte value) => new(value);
    public static implicit operator byte(Bits8 value) => value._value;


    public static Bits8 operator ~(Bits8 value) => (byte)~value._value;
    public static Bits8 operator |(Bits8 left, Bits8 right) => (byte)(left._value | right._value);
    public static Bits8 operator &(Bits8 left, Bits8 right) => (byte)(left._value & right._value);
    public static Bits8 operator ^(Bits8 left, Bits8 right) => (byte)(left._value ^ right._value);
    public static Bits8 operator <<(Bits8 value, int shift) => (byte)(value._value << shift);
    public static Bits8 operator >>(Bits8 value, int shift) => (byte)(value._value >> shift);

    public static bool operator ==(Bits8 left, Bits8 right) => left._value == right._value;
    public static bool operator !=(Bits8 left, Bits8 right) => left._value != right._value;


    public readonly Bits8 Bit(int index, bool value)
    {
        if (index < 0 || index > MaxIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"位索引范围在必须在 [0, {MaxLength}] 之间");
        }

        byte new_value;

        if (value)
        {
            new_value = (byte)(_value | (1 << index));
        }
        else
        {
            new_value = (byte)(_value & ~(1 << index));
        }

        return new Bits8(new_value);
    }
    public readonly bool Bit(int index)
    {
        return (_value & (1 << index)) != 0;
    }
    public readonly byte ToUInt8()
    {
        return _value;
    }

    public readonly void CopyTo(Span<bool> destination, bool bigEndian = false)
    {
        if (destination.Length == 0) return;

        int bits_to_copy = destination.Length > MaxLength ? MaxLength : destination.Length;

        for (int i = 0; i < bits_to_copy; i++)
        {
            var cur_index = bigEndian ? i : bits_to_copy - 1 - i;
            destination[cur_index] = (_value & (1 << cur_index)) != 0;
        }
    }

    public override int GetHashCode() => _value.GetHashCode();
    public override bool Equals(object obj) => obj is Bits8 other && _value.Equals(other._value);
    public override string ToString() => $"0b{Convert.ToString(_value, 2).PadLeft(MaxLength, '0')}";
}