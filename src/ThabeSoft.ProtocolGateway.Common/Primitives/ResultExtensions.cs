//namespace ThabeSoft.ProtocolGateway.Primitives;


//public static class ResultExtensions
//{
//    /// <summary>
//    /// 无值结果方法
//    /// </summary>
//    extension(Result result)
//    {
//        /// <summary>
//        /// 将无值结果转换为有值结果。
//        /// </summary>
//        /// <remarks>
//        /// 当原结果成功时，使用指定的值作为新结果的值；
//        /// 当原结果失败时，保留错误信息并转换为新的有值结果类型。
//        /// </remarks>
//        /// <typeparam name="T">目标结果的值类型</typeparam>
//        /// <param name="valueOnSuccess">成功时使用的值</param>
//        /// <returns>
//        /// - 原结果成功：Result&lt;T&gt;.Ok(valueOnSuccess)，保留原消息
//        /// - 原结果失败：Result&lt;T&gt;.Error(ErrorType, Message)
//        /// </returns>
//        public Result<T> ToResult<T>(T valueOnSuccess)
//        {
//            if (result.IsSuccess)
//            {
//                return Result.Ok(valueOnSuccess, result.Message);
//            }

//            return Result.Error<T>(result.ErrorType, result.Message);
//        }

//        public  Result Bind(Action action)
//        {
//            if (result.IsSuccess) action();
//            return result;
//        }
//        public ValueTask<Result> BindAsync(Func<CancellationToken, ValueTask<Result>> action, CancellationToken cancellationToken = default)
//        {
//            if (result.IsSuccess) return action(cancellationToken);
//            return new ValueTask<Result>(result);
//        }


//        public Result Then(Func<Result> next)
//        {
//            return result.IsSuccess ? next() : result;
//        }



//        public static bool All(params Result[] results)
//        {
//            foreach (var r in results)
//            {
//                if (!r.IsSuccess) return r;
//            }

//            return Result.Success;
//        }
//        public static Result All<TValue>(Span<TValue> destination, params Result<TValue>[] results) where TValue : unmanaged
//        {
//            if (destination.Length < results.Length)
//            {
//                return Result.Error(ErrorType.InvalidOperation, "Destination too small");
//            }

//            for (int i = 0; i < results.Length; i++)
//            {
//                if (!results[i].IsSuccess)
//                {
//                    return Result.Error(ErrorType.Internal);
//                }
//            }

//            for (int i = 0; i < results.Length; i++)
//            {
//                destination[i] = results[i].Value;
//            }

//            return Result.Success;
//        }
//    }

//    /// <summary>
//    /// 含值结果扩展
//    /// </summary>
//    /// <typeparam name="T">值类型</typeparam>
//    extension<T>(Result<T> result)
//    {
//        #region --ToResult--

//        /// <summary>
//        /// 将当前结果转换为另一种值类型的结果，丢弃原值。
//        /// </summary>
//        /// <remarks>
//        /// 当需要改变结果的值类型，但不需要保留原值时使用。
//        /// 例如：<c>Result&lt;int&gt; → Result&lt;Guid&gt;</c>
//        /// </remarks>
//        /// <typeparam name="U">目标结果的值类型</typeparam>
//        /// <returns>
//        /// - 如果原结果成功：返回错误（因为无法将 T 转换为 U，调用方应使用带 value 参数的重载）
//        /// - 如果原结果失败：返回相同错误类型和消息的 Result&lt;U&gt;
//        /// </returns>
//        public Result<U> ToResult<U>()
//        {
//            return Result.Error<U>(result.ErrorType, result.Message);
//        }
//        /// <summary>
//        /// 将当前结果转换为另一种值类型的结果，并指定成功时的值。
//        /// </summary>
//        /// <remarks>
//        /// 当原结果成功时，用指定的 value 作为新结果的值；
//        /// 当原结果失败时，保留错误信息并转换为新结果类型。
//        /// </remarks>
//        /// <typeparam name="U">目标结果的值类型</typeparam>
//        /// <param name="value">成功时使用的新值</param>
//        /// <returns>
//        /// - 如果原结果成功：返回 Result&lt;U&gt;.Ok(value)，并保留原消息
//        /// - 如果原结果失败：返回相同错误类型和消息的 Result&lt;U&gt;
//        /// </returns>
//        public Result<U> ToResult<U>(U value)
//        {
//            if (result.IsSuccess)
//            {
//                return Result.Ok(value, result.Message);
//            }

//            return Result.Error<U>(result.ErrorType, result.Message);
//        }
//        /// <summary>
//        /// 将当前结果转换为另一种值类型的结果，通过转换器生成新值。
//        /// </summary>
//        /// <remarks>
//        /// 原结果成功时，使用 valueConverter 将 T 转换为 U 作为新结果的值；
//        /// 原结果失败时，保留错误信息并转换为新结果类型。
//        /// </remarks>
//        /// <typeparam name="U">目标结果的值类型</typeparam>
//        /// <param name="valueConverter">成功时将 T 转换为 U 的委托</param>
//        /// <returns>
//        /// - 如果原结果成功：返回 Result&lt;U&gt;.Ok(valueConverter(result.Value))，并保留原消息
//        /// - 如果原结果失败：返回相同错误类型和消息的 Result&lt;U&gt;
//        /// </returns>
//        public Result<U> ToResult<U>(Func<T, U> valueConverter)
//        {
//            if (result.IsSuccess)
//            {
//                return Result.Ok(valueConverter(result.Value), result.Message);
//            }

//            return Result.Error<U>(result.ErrorType, result.Message);
//        }


//        #endregion


//        public Result<T> Bind(Action<T> action)
//        {
//            if (result.IsSuccess) action(result.Value);
//            return result;
//        }
//        public ValueTask<Result<T>> BindAsync(Func<CancellationToken, ValueTask<Result<T>>> action, CancellationToken cancellationToken = default)
//        {
//            if (result.IsSuccess) return action(cancellationToken);
//            return new ValueTask<Result<T>>(result);
//        }


//        /// <summary>
//        /// 将成功结果的值通过选择器转换为新值，并返回新的 Result。
//        /// </summary>
//        /// <remarks>
//        /// 当前结果成功时，应用选择器将 T 转换为 U，并保留原消息返回成功的 Result；
//        /// 当前结果失败时，保留错误类型和消息，转换为 Result&lt;U&gt;。
//        /// </remarks>
//        /// <typeparam name="U">目标结果的值类型</typeparam>
//        /// <param name="selector">值转换函数（仅当结果成功时调用）</param>
//        /// <returns>
//        /// - 成功：Result&lt;U&gt;.Ok(selector(value))
//        /// - 失败：Result&lt;U&gt;.Error(ErrorType, Message)
//        /// </returns>
//        public Result<U> Map<U>(Func<T, U> selector)
//        {
//            if(result.IsSuccess)
//            {
//                return Result.Ok(selector(result.Value), result.Message);
//            }

//            return Result.Error<U>(result.ErrorType, result.Message);
//        }
//        public Result<U> OrElse<U>(Func<U> defaultValue)
//        {
//            if(result.IsSuccess)
//            {
//                return Result.Ok(defaultValue());
//            }

//            return Result.Error<U>(result.ErrorType, result.Message);
//        }
//    }
//}