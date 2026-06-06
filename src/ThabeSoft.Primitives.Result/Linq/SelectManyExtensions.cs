namespace ThabeSoft.Primitives.Linq;


/// <summary>
/// 选择多个
/// </summary>
public static class SelectManyExtensions
{
    public readonly struct Unit;
    public static Result<Unit> ToUint(this Result result) => result.Cast<Unit>();
}
