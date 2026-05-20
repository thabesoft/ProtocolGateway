namespace ThabeSoft.IndustriaHub;

/// <summary>
/// 结果
/// </summary>
public readonly struct Result<TValue> where TValue : struct
{
    private readonly TValue _value;

    public readonly bool IsSuccess { get; }
    public readonly ResultErrorType ErrorType { get; }
    public readonly string? Message { get; }


    public Result(TValue value)
    {
        _value = value;
        IsSuccess = true;
        ErrorType = ResultErrorType.None;
    }
    public Result(ResultErrorType error, string? message)
    {
        IsSuccess = false;
        ErrorType = error;
        Message = message;
    }


    public readonly bool TryGetValue(out TValue value)
    {
        if (IsSuccess)
        {
            value = _value;
            return true;
        }

        value = default;
        return false;
    }
}