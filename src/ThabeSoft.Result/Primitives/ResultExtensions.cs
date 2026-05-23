namespace ThabeSoft.ProtocolGateway.Primitives;


/// <summary>
/// 结果扩展
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// 预设错误
    /// </summary>
    extension(Result)
    {
        /// <summary>
        /// 请求参数错误
        /// </summary>
        /// <remarks>
        /// 当调用方传入的参数无效时使用，例如：
        /// <list type="bullet">
        /// <item>缓冲区长度不足</item>
        /// <item>数量超出有效范围</item>
        /// <item>地址无效</item>
        /// <item>参数为 null 或空</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.InvalidParameter"/></para>
        /// <para>这是调用方的问题，不是设备或系统的问题。</para>
        /// </remarks>
        /// <param name="message">具体的错误描述</param>
        public static Result InvalidParameter(string message)
        {
            return Result.Error(ErrorType.InvalidParameter, message);
        }
        /// <summary>
        /// 请求参数错误
        /// </summary>
        /// <remarks>
        /// 当调用方传入的参数无效时使用，例如：
        /// <list type="bullet">
        /// <item>缓冲区长度不足</item>
        /// <item>数量超出有效范围</item>
        /// <item>地址无效</item>
        /// <item>参数为 null 或空</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.InvalidParameter"/></para>
        /// <para>这是调用方的问题，不是设备或系统的问题。</para>
        /// </remarks>
        /// <param name="message">具体的错误描述</param>
        public static Result<T> InvalidParameter<T>(string message)
        {
            return Result.Error<T>(ErrorType.InvalidParameter, message);
        }

        /// <summary>
        /// 无效数据错误
        /// </summary>
        /// <remarks>
        /// 当从设备接收到的数据格式或内容不正确时使用，例如：
        /// <list type="bullet">
        /// <item>CRC 校验失败</item>
        /// <item>数据长度不足或超出</item>
        /// <item>功能码不匹配</item>
        /// <item>响应数据解析失败</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.InvalidData"/></para>
        /// <para>这是设备或数据源的问题，不是调用方的问题。</para>
        /// </remarks>
        public static Result InvalidData(string message)
        {
            return Result.Error(ErrorType.InvalidData, message);
        }
        /// <summary>
        /// 无效数据错误
        /// </summary>
        /// <remarks>
        /// 当从设备接收到的数据格式或内容不正确时使用，例如：
        /// <list type="bullet">
        /// <item>CRC 校验失败</item>
        /// <item>数据长度不足或超出</item>
        /// <item>功能码不匹配</item>
        /// <item>响应数据解析失败</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.InvalidData"/></para>
        /// <para>这是设备或数据源的问题，不是调用方的问题。</para>
        /// </remarks>
        public static Result<T> InvalidData<T>(string message)
        {
            return Result.Error<T>(ErrorType.InvalidData, message);
        }

        /// <summary>
        /// 无效操作
        /// </summary>
        /// <remarks>
        /// 当操作在当前状态下不被允许时使用。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>未连接时尝试读写</item>
        /// <item>已断开连接时执行操作</item>
        /// <item>操作顺序错误（如未初始化就使用）</item>
        /// <item>重复执行不允许的操作</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.InvalidOperation"/></para>
        /// <para>这是调用方未正确处理状态机导致的问题。</para>
        /// </remarks>
        /// <param name="message">具体的错误描述</param>
        public static Result InvalidOperation(string message)
        {
            return Result.Error(ErrorType.InvalidOperation, message);
        }
        /// <summary>
        /// 无效操作
        /// </summary>
        /// <remarks>
        /// 当操作在当前状态下不被允许时使用。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>未连接时尝试读写</item>
        /// <item>已断开连接时执行操作</item>
        /// <item>操作顺序错误（如未初始化就使用）</item>
        /// <item>重复执行不允许的操作</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.InvalidOperation"/></para>
        /// <para>这是调用方未正确处理状态机导致的问题。</para>
        /// </remarks>
        /// <param name="message">具体的错误描述</param>
        public static Result<T> InvalidOperation<T>(string message)
        {
            return Result.Error<T>(ErrorType.InvalidOperation, message);
        }
    }



    


    /// <summary>
    /// 无值结果扩展
    /// </summary>
    extension(Result result)
    {
        /// <summary>
        /// 成功时传播当前 Result 的错误，失败时执行备用操作
        /// </summary>
        /// <remarks>
        /// 注意：此方法的行为与常规的 OrElse 相反！
        /// <para>当当前 Result 成功时，返回失败（因为类型不匹配，无法提供 U 类型的值）；</para>
        /// <para>当当前 Result 失败时，执行备用操作获取 U 类型的 Result。</para>
        /// <para>这通常用于在失败时提供备选方案。</para>
        /// </remarks>
        /// <typeparam name="U">新结果的值类型</typeparam>
        /// <param name="action">失败时执行的备用操作，返回 Result&lt;U&gt;</param>
        /// <returns>
        /// 成功时：返回包含当前错误的 Result&lt;U&gt;（传播错误）
        /// 失败时：返回备用操作的结果
        /// </returns>
        public Result Fallback(Func<Result> action)
        {
            if (!result.IsSuccess) return action();
            return result;
        }


        /// <summary>
        /// 成功时将 Result 转换为 Result&lt;U&gt;，失败时传播错误
        /// </summary>
        /// <remarks>
        /// 当当前 Result 成功时，执行指定的委托，将其返回值包装为 Result&lt;U&gt;；
        /// 当当前 Result 失败时，忽略委托，传播原始错误。
        /// <para>与 <see cref="ThenReturn{U}"/> 的区别：本方法通过委托动态生成返回值。</para>
        /// </remarks>
        /// <typeparam name="U">新结果的值类型</typeparam>
        /// <param name="handler">成功时执行的委托，返回 U 类型的值</param>
        /// <returns>
        /// 成功时：返回包含委托返回值的 Result&lt;U&gt;
        /// 失败时：返回包含原始错误的 Result&lt;U&gt;
        /// </returns>
        public Result<U> Map<U>(Func<U> handler)
        {
            if (!result.IsSuccess) return result.PropagateError<U>();
            return Result.Ok(handler());
        }

        /// <summary>
        /// 成功时执行副作用操作，并返回原始 Result
        /// </summary>
        /// <remarks>
        /// 当当前 Result 成功时，执行指定的委托；无论成功还是失败，都返回原始 Result。
        /// 常用于日志记录、通知等不影响返回值的操作。
        /// <para>这是 <c>Tap</c> 方法的无值版本，适用于 Result（非泛型）。</para>
        /// </remarks>
        /// <param name="handler">成功时执行的委托</param>
        /// <returns>返回原始 Result 实例</returns>
        public Result Tap(Action handler)
        {
            if (result.IsSuccess) handler();
            return result;
        }
        /// <summary>
        /// 成功时执行异步副作用操作，并返回原始 Result
        /// </summary>
        /// <remarks>
        /// 当当前 Result 成功时，执行指定的异步委托；无论成功还是失败，都返回原始 Result。
        /// 常用于异步日志记录、通知等不影响返回值的操作。
        /// <para>这是 <c>Tap</c> 方法的异步版本，适用于 Result（非泛型）。</para>
        /// </remarks>
        /// <param name="handler">成功时执行的异步委托</param>
        /// <returns>返回原始 Result 实例（支持链式调用）</returns>
        public async ValueTask<Result> Tap(Func<ValueTask> handler)
        {
            if (result.IsSuccess) await handler();
            return result;
        }
        /// <summary>
        /// 成功时执行异步副作用操作（支持取消），并返回原始 Result
        /// </summary>
        /// <remarks>
        /// 当当前 Result 成功时，执行指定的异步委托；无论成功还是失败，都返回原始 Result。
        /// 常用于异步日志记录、通知等不影响返回值的操作。
        /// <para>这是 <c>Tap</c> 方法的异步版本，支持取消令牌。</para>
        /// </remarks>
        /// <param name="handler">成功时执行的异步委托，接收 CancellationToken 参数</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>返回原始 Result 实例（支持链式调用）</returns>
        public async ValueTask<Result> Tap(Func<CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) await handler(cancellationToken);
            return result;
        }

        /// <summary>
        /// 成功时返回指定的值，失败时传播错误
        /// </summary>
        /// <remarks>
        /// 当当前 Result 成功时，忽略原始值，返回指定的新值；
        /// 当前 Result 失败时，传播错误到新的 Result 类型。
        /// <para>与 <see cref="Then{TResult}(Func{TValue, TResult})"/> 的区别：
        /// 本方法不执行委托，直接返回固定值，避免闭包分配。</para>
        /// </remarks>
        /// <typeparam name="U">新结果的值类型</typeparam>
        /// <param name="value">成功时要返回的值</param>
        /// <returns>
        /// 成功时返回包含指定值的 Result&lt;U&gt;；
        /// 失败时返回包含原始错误的 Result&lt;U&gt;
        /// </returns>
        public Result<U> ThenReturn<U>(U value)
        {
            if (!result.IsSuccess) return result.PropagateError<U>();
            return Result.Ok(value);
        }

        public Result<U> Then<U>(Func<Result<U>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler();
        }
        public async ValueTask<Result<U>> Then<U>(Func<ValueTask<Result<U>>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler();
        }
        public async ValueTask<Result<U>> Then<U>(Func<CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(cancellationToken);
        }
    }


    extension<T>(Result<T> result)
    {
        public Result<U> OrElse<U>(Func<T, Result<U>> action)
        {
            if (!result.IsSuccess)
            {
                return result.PropagateError<U>();
            }

            return action(result.Value);
        }
        public Result<U> Map<U>(Func<T, U> handler)
        {
            if (!result.IsSuccess)
            {
                return result.PropagateError<U>();
            }

            return Result.Ok(handler(result.Value));
        }


        public Result<T> Tap(Action<T> handler)
        {
            if (result.IsSuccess) handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> Tap(Func<T, ValueTask> handler)
        {
            if (result.IsSuccess) await handler(result.Value);
            return result;
        }
        public async ValueTask<Result<T>> Tap(Func<T, CancellationToken, ValueTask> handler, CancellationToken cancellationToken = default)
        {
            if (result.IsSuccess) await handler(result.Value, cancellationToken);
            return result;
        }


        public Result<U> Then<TArgs, U>(TArgs ags, Func<TArgs, T, Result<U>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(ags, result.Value);
        }
        public Result<U> Then<U>(Func<T, U> handler)
        {
            if (!result.IsSuccess) return result.PropagateError<U>();
            return handler(result.Value);
        }

        public Result<U> Then<U>(Func<T, Result<U>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return handler(result.Value);
        }
        public async ValueTask<Result<U>> Then<U>(Func<T, ValueTask<Result<U>>> handler)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value);
        }
        public async ValueTask<Result<U>> Then<U>(Func<T, CancellationToken, ValueTask<Result<U>>> handler, CancellationToken cancellationToken = default)
        {
            if (!result.IsSuccess) result.PropagateError<U>();
            return await handler(result.Value, cancellationToken);
        }


        public Result<T> Out(out T value)
        {
            if (result.IsSuccess)
            {
                value = result.Value;
                return result;
            }

            value = default!;
            return result;
        }
        public Result<(T, U)> Zip<U>(Result<U> other)
        {
            if (!result.IsSuccess)
                return result.PropagateError<(T, U)>();

            if (!other.IsSuccess)
                return other.PropagateError<(T, U)>();

            return (result.Value, other.Value);
        }
    }

    /// <summary>
    /// Linq
    /// </summary>

    extension<T>(Result<T> result)
    {
        public Result<U> Select<U>(Func<T, Result<U>> selector)
        {
            return result.Then(selector);
        }

        public Result<T> Where(Func<T, bool> predicate)
        {
            if (result.IsSuccess)
            {
                if (predicate(result.Value)) return result;
                return Result.Error<T>(ErrorType.InvalidOperation, "条件不成立");
            }
            return result.PropagateError<T>();
        }
    }
}