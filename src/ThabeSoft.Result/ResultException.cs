using System.Diagnostics;

namespace ThabeSoft.Primitives;

/// <summary>
/// 结果异常
/// </summary>
public sealed class ResultException : Exception
{
    public ErrorType ErrorType { get; } = ErrorType.None;


    public ResultException()
    {
    }
    public ResultException(ErrorType errorType, string? message = null) : base(message)
    {
        ErrorType = errorType;
    }
    public ResultException(string? message) : base(message)
    {
    }

    public ResultException(string? message, Exception? innerException) : base(message, innerException)
    {
    }



    internal static void ThrowIfDebugger(ErrorType errorType, string? message = null)
    {
#if DEBUG
        Debug.WriteLine($"[RESULT_ERROR] {errorType}: {message}");

        // 调试状态下直接异常
        if (!Debugger.IsAttached) return;
        throw new ResultException($"[{errorType}]: {message}");
#endif
    }
}