namespace ThabeSoft.Primitives;


/// <summary>
/// 结果扩展
/// </summary>
public static partial class ResultExtensions
{
    // 有值结果
    extension<TValue>(Result<TValue> result) where TValue : notnull
    {
        /// <summary>
        /// 将上一个结果的值转换为新结果
        /// </summary>
        /// <typeparam name="UValue">新结果值类型</typeparam>
        /// <param name="handler">值转换器</param>
        public Result<UValue> Select<UValue>(Func<TValue, UValue> handler) where UValue : notnull
        {
            if (result.HasValue) return Result.Success(handler(result.Value));
            return result.Cast<UValue>();
        }
        /// <summary>
        /// 基于上一个结果的值, 返回一个新结果
        /// </summary>
        /// <typeparam name="UValue">新结果值类型</typeparam>
        public Result<UValue> Select<UValue>(Func<TValue, Result<UValue>> handler) where UValue : notnull
        {
            if (result.IsSuccess) return handler(result.Value);

            return result.Cast<UValue>();
        }
        /// <summary>
        /// 基于上一个结果的值, 携带参数返回一个新结果
        /// </summary>
        /// <typeparam name="UValue">新结果类型</typeparam>
        /// <typeparam name="TState">自定义参数类型</typeparam>
        /// <param name="state">自定义参数值</param>
        public Result<UValue> Select<UValue, TState>(TState state, Func<TValue, TState, Result<UValue>> handler) where UValue : notnull
        {
            if (result.IsSuccess) return handler(result.Value, state);

            return result.Cast<UValue>();
        }
        /// <summary>
        /// 基于上一个结果, 携带参数返回一个新结果
        /// </summary>
        public Result Select<TState>(TState state, Func<TValue, TState, Result> handler)
        {
            if (result.IsSuccess) return handler(result.Value, state);

            return result;
        }


        /// <summary>
        /// 实现 LINQ 查询语法的 SelectMany，支持 from...from...select 连续绑定
        /// </summary>
        /// <param name="bind">用第一个结果的值获取第二个结果</param>
        /// <param name="project">将两个结果的值组合成最终值</param>
        /// <returns>两个结果都成功时返回组合后的成功结果，否则返回第一个失败结果</returns>
        public Result<TSelect> SelectMany<UValue, TSelect>(Func<TValue, Result<UValue>> bind, Func<TValue, UValue, TSelect> project)
            where UValue : notnull
            where TSelect : notnull
        {
            if (!result.HasValue) return result.Cast<TSelect>();

            var next = bind(result.Value);
            if (!next.HasValue) return next.Cast<TSelect>();

            return Result.Success(project(result.Value, next.Value));
        }
    }
}
