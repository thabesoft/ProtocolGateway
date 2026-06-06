namespace ThabeSoft.Primitives.Linq;


/// <summary>
/// 选择多个
/// </summary>
public static class SelectManyExtensions
{
    public readonly struct Unit;
    public static Result<Unit> ToUint(this Result result) => result.Cast<Unit>();



    public static Result<TOut> SelectMany<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> bind)
        where TIn : notnull
        where TOut : notnull
    {
        if (result.IsFailure) return result.Cast<TOut>();

        return bind(result.Value);
    }

    public static Result<TSelect> SelectMany<TIn, TOut, TSelect>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> bind,
        Func<TIn, TOut, TSelect> project)
        where TIn : notnull
        where TOut : notnull
        where TSelect : notnull
    {
        if (!result.HasValue) return result.Cast<TSelect>();

        var next = bind(result.Value);
        if (next.IsFailure) return next.Cast<TSelect>();

        return Result.Success(project(result.Value, next.Value));
    }

    public static Result<TSelect> Select<TIn, TSelect>(
        this Result<TIn> result,
        Func<TIn, TSelect> selector)
        where TIn : notnull
        where TSelect : notnull
    {
        if (result.IsFailure) return result.Cast<TSelect>();

        return Result.Success(selector(result.Value));
    }
}
