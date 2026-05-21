namespace ThabeSoft.ProtocolGateway.Protocols;

/// <summary>
/// 协议异常
/// </summary>
public class ProtocolException : Exception
{
    public ProtocolException()
    {
    }

    public ProtocolException(string message) : base(message)
    {
    }

    public ProtocolException(string message, Exception innerException) : base(message, innerException)
    {
    }
}



public interface IReadValue<T> where T : struct
{
    string Address { get; }
    T Value { get; }
}