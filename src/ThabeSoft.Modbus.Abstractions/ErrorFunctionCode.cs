using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus;


/// <summary>
/// 错误功能码
/// </summary>
public readonly record struct ErrorFunctionCode : IEquatable<ErrorFunctionCode>
{
    public static readonly FunctionCode Empty = default;


    public FunctionCode FunctionCode { get; }
    private ErrorFunctionCode(FunctionCode functionCode) => FunctionCode = functionCode;


    public static ErrorFunctionCode FromFunctionCode(FunctionCode code)
    {
        return new ErrorFunctionCode(code);
    }
    public static Result<ErrorFunctionCode> FromCode(byte code)
    {
        if ((code & 0x80) == 0)
        {
            return Result.Error<ErrorFunctionCode>($"不是有效的异常功能码: 0x{code:X2}（缺少 0x80 标志位）");
        }

        return FunctionCode.FromCode((byte)(code & 0x7F))
            .Select(x => new ErrorFunctionCode(x));
    }
    public static bool TryFromCode(byte code, out ErrorFunctionCode errorFunctionCode)
    {
        errorFunctionCode = default;
        if ((code & 0x80) == 0) return false;

        if (!FunctionCode.TryFromCode((byte)(code & 0x7F), out var function_code)) return false;

        errorFunctionCode = new ErrorFunctionCode(function_code);
        return true;
    }



    public static implicit operator byte(ErrorFunctionCode code) => (byte)(code | 0x80);
    public override string ToString() => $"(0x{FunctionCode | 0x80:X2})";
}