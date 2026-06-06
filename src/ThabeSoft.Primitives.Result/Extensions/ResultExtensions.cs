namespace ThabeSoft.Primitives;


/// <summary>
/// 结果扩展
/// </summary>
public static class ResultExtensions
{
    //工厂方法
    extension(Result)
    {
        public static Result Success() => new(ResultLevel.Success, null);
        public static Result Info(string message) => new(ResultLevel.Info, message);
        public static Result Warning(string message) => new(ResultLevel.Warning, message);
        public static Result Error(string message) => new(ResultLevel.Error, message);



        public static Result<TValue> Success<TValue>(TValue value) where TValue : notnull
            => new(ResultLevel.Success, null, value);

        public static Result<TValue> Info<TValue>(string message) where TValue : notnull
            => new(ResultLevel.Info, message);
        public static Result<TValue> Info<TValue>(string message, TValue value) where TValue : notnull
            => new(ResultLevel.Info, message, value);

        public static Result<TValue> Warning<TValue>(string message) where TValue : notnull
            => new(ResultLevel.Warning, message);
        public static Result<TValue> Warning<TValue>(string message, TValue value) where TValue : notnull
            => new(ResultLevel.Warning, message, value);

        public static Result<TValue> Error<TValue>(string message) where TValue : notnull
            => new(ResultLevel.Error, message);
        public static Result<TValue> Error<TValue>(string message, TValue value) where TValue : notnull
            => new(ResultLevel.Error, message, value);
    }

    // 无值结果
    extension(Result result)
    {
        /// <summary>
        /// 成功回调
        /// </summary>
        public Result OnSuccess(Action handler)
        {
            if (result.IsSuccess) handler();
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
        /// 基于上一个结果的值, 返回一个新结果
        /// </summary>
        /// <typeparam name="U">新结果值类型</typeparam>
        public Result<U> Select<U>(Func<TValue, Result<U>> handler) where U : notnull
        {
            if (result.IsSuccess) return handler(result.Value);

            return result.Cast<U>();
        }
        /// <summary>
        /// 基于上一个结果的值, 返回一个新结果, 可携带自定义参数
        /// </summary>
        /// <typeparam name="U">新结果类型</typeparam>
        /// <typeparam name="TState">自定义参数类型</typeparam>
        /// <param name="state">自定义参数值</param>
        public Result<U> Select<U, TState>(TState state, Func<TValue, TState, Result<U>> handler) where U : notnull
        {
            if (result.IsSuccess) return handler(result.Value, state);

            return result.Cast<U>();
        }


        /// <summary>
        /// 将上一个结果的值转换为新结果
        /// </summary>
        /// <typeparam name="U">新结果值类型</typeparam>
        /// <param name="handler">值转换器</param>
        public Result<U> Select<U>(Func<TValue, U> handler) where U : notnull
        {
            if (result.HasValue) return Result.Success(handler(result.Value));
            return result.Cast<U>();
        }


        public Result<TValue> Where(Func<TValue, bool> matcher, string? errorMessage = null)
        {
            if (result.IsSuccess && matcher(result.Value))
            {
                return Result.Error<TValue>(errorMessage ?? "条件比较失败");
            }

            return result;
        }



        /// <summary>
        /// 获取值, 如果没有返回 默认值
        /// </summary>
        public TValue? GetValueOrDefault()
        {
            return result.HasValue ? result.Value : default;
        }

        /// <summary>
        /// 获取值, 如果没有则使用默认值
        /// </summary>
        public TValue GetValueOrDefault(TValue defaultValue)
        {
            return result.HasValue ? result.Value : defaultValue;
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        public bool TryGetValue(out TValue value)
        {
            if (result.HasValue)
            {
                value = result.Value;
                return true;
            }

            value = default!;
            return false;
        }
    }
}
