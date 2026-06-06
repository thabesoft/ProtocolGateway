namespace ThabeSoft.Primitives;


/// <summary>
/// 结果扩展
/// </summary>
public static partial class ResultExtensions
{
    // 通用结果
    extension<T>(T result) where T : IResult
    {
        /// <summary>
        /// 成功回调
        /// </summary>
        public T OnSuccess(Action handler)
        {
            if (result.IsSuccess) handler();
            return result;
        }

        /// <summary>
        /// 有消息时候回调
        /// </summary>
        public T OnMessage(Action<string> handler)
        {
            if (result.HasMessage) handler(result.Message!);
            return result;
        }
    }

    // 有值结果
    extension<TValue>(Result<TValue> result) where TValue : notnull
    {
        /// <summary>
        /// 有值回调
        /// </summary>
        public Result<TValue> OnValue(Action<TValue> handler)
        {
            if (result.HasValue) handler(result.Value);
            return result;
        }

        /// <summary>
        /// 有值回调, 并携带一个参数
        /// </summary>
        /// <typeparam name="TState">参数类型</typeparam>
        /// <param name="state">参数值</param>
        public Result<TValue> OnValue<TState>(TState state, Action<TValue, TState> handler)
        {
            if (result.HasValue) handler(result.Value, state);
            return result;
        }
    }
}
