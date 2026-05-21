namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 结果
/// </summary>
public readonly struct Result<TValue> where TValue : unmanaged
{
    public static readonly Result<TValue> Empty = default;


    public TValue Value { get; }
    public ResponseStatus Status { get; }
    public bool IsSuccess => Status == ResponseStatus.OK;


    private Result(TValue value)
    {
        Value = value;
        Status = ResponseStatus.OK;
    }
    private Result(ResponseStatus status)
    {
        Status = status;
    }


    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Failure(ResponseStatus status) => new(status);
    public static Result<TValue> Timeout() => new(ResponseStatus.Timeout);
    public static Result<TValue> InternalError() => new(ResponseStatus.InternalError);
}

public sealed class Result
{
    public static Result<TValue> Success<TValue>(TValue value) where TValue : unmanaged
        => Result<TValue>.Success(value);
    public static Result<TValue> Failure<TValue>(ResponseStatus status) where TValue : unmanaged
        => Result<TValue>.Failure(status);
    public static Result<TValue> Timeout<TValue>() where TValue : unmanaged
        => Result<TValue>.Timeout();
    public static Result<TValue> InternalError<TValue>() where TValue : unmanaged
        => Result<TValue>.InternalError();
}