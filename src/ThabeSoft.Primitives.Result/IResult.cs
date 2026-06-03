namespace ThabeSoft.Primitives;


/// <summary>
/// 结果
/// </summary>
public interface IResult
{
    /// <summary> 等级 </summary>
    ResultLevel Level { get; }
    /// <summary> 消息 </summary>
    string? Message { get; }

    /// <summary>
    /// 传播错误到指定类型
    /// </summary>
    Result<U> Cast<U>();
}

/// <summary>
/// 结果
/// </summary>
/// <typeparam name="TValue">包含值的类型</typeparam>
public interface IResult<out TValue> : IResult
{
    /// <summary> 值 </summary>
    TValue Value { get; }
}