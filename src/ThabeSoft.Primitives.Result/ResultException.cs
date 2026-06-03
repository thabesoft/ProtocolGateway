using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ThabeSoft.Primitives;

/// <summary>
/// 结果异常
/// </summary>
public sealed class ResultException : Exception
{
    public ResultException()
    {
    }
    public ResultException(string? message) : base(message)
    {
    }

    public ResultException(string? message, Exception? innerException) : base(message, innerException)
    {
    }



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfDebugger(string? message = null)
    {
#if DEBUG
        Debug.WriteLine($"[RESULT_ERROR]: {message}");

        // 调试状态下直接异常
        if (!Debugger.IsAttached) return;
        throw new ResultException(message);
#endif
    }
}