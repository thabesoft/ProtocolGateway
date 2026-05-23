namespace ThabeSoft.ProtocolGateway.Bitwise;


public interface IBits<T> : IReadonlyBits
    where T : IBits<T>
{
    T Bit(int index, bool value);
}
