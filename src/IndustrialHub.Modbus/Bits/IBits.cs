namespace IndustrialHub.Modbus.Bits;


public interface IBits<T> : IReadonlyBits
    where T : IBits<T>
{
    T Bit(int index, bool value);
}
