namespace ThabeSoft.Primitives;


/// <summary>
/// 结果
/// </summary>
public interface IResult
{
    /// <summary>
    /// 分级
    /// </summary>
    ResultLevel Level { get; }

    /// <summary>
    /// 消息
    /// </summary>
    string? Message { get; }


    /// <summary>
    /// 是否有消息
    /// </summary>
    bool HasMessage{ get; }

    /// <summary>
    /// 信息 || 成功
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// 警告 || 错误
    /// </summary>
    bool IsProblem { get; }

    /// <summary>
    /// 错误
    /// </summary>
    bool IsFailure { get; }



    /// <summary>
    /// 转换错误到指定类型 (必须是没有值的结果)
    /// </summary>
    Result<UValue> Cast<UValue>() where UValue : notnull;
}

/// <summary>
/// 结果
/// </summary>
/// <typeparam name="TValue">包含值的类型</typeparam> 
public interface IResult<out TValue> : IResult
    where TValue : notnull
{
    /// <summary>
    /// 是否有值
    /// </summary>
    bool HasValue { get; }

    /// <summary> 
    /// 值
    /// </summary>
    TValue Value { get; }
}