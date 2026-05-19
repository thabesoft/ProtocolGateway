namespace IndustrialHub.Modbus.Bits;


/// <summary>
/// 8位 Bit
/// </summary>
public readonly struct Bits16 : IBits<Bits16>
{
    private const int MaxIndex = 15;
    private const int MaxLength = 16;


    public static Bits16 Empty = default;



    private readonly ushort _value;
    private Bits16(ushort value) => _value = value;


    public static Bits16 FromBits(Bits8 first, Bits8 second, bool isBigEndian = false)
    {
        if (isBigEndian)
        {
            return new Bits16((ushort)((first << Bits8.MaxLength) | second));
        }
        else
        {
            return new Bits16((ushort)((second << Bits8.MaxLength) | first));
        }
    }

    public static Bits16 FromBits(ReadOnlySpan<bool> bits, bool bigEndian = false)
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

        return new Bits16(result);
    }


    public static implicit operator Bits16(ushort value) => new(value);
    public static implicit operator ushort(Bits16 value) => value._value;


    public static Bits16 operator ~(Bits16 value) => (ushort)~value._value;
    public static Bits16 operator |(Bits16 left, Bits16 right) => (ushort)(left._value | right._value);
    public static Bits16 operator &(Bits16 left, Bits16 right) => (ushort)(left._value & right._value);
    public static Bits16 operator ^(Bits16 left, Bits16 right) => (ushort)(left._value ^ right._value);
    public static Bits16 operator <<(Bits16 value, int shift) => (ushort)(value._value << shift);
    public static Bits16 operator >>(Bits16 value, int shift) => (ushort)(value._value >> shift);

    public static bool operator ==(Bits16 left, Bits16 right) => left._value == right._value;
    public static bool operator !=(Bits16 left, Bits16 right) => left._value != right._value;


    public readonly Bits16 Bit(int index, bool value)
    {
        if (index < 0 || index > MaxIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"位索引范围在必须在 [0, {MaxIndex}] 之间");
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

        return new Bits16(new_value);
    }
    public readonly bool Bit(int index)
    {
        return (_value & (1 << index)) != 0;
    }
    public readonly ushort ToUInt16()
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
    public override bool Equals(object obj) => obj is Bits16 other && _value.Equals(other._value);
    public override string ToString() => $"0b{Convert.ToString(_value, 2).PadLeft(MaxLength, '0').Insert(8, "_")}";
}