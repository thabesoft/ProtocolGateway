namespace ThabeSoft.Primitives;


/// <summary>
/// 错误预设
/// </summary>
public static partial class ResultExtensions
{
    extension(Result)
    {
        [Obsolete("实验性")]
        public static Result All<T>(params IEnumerable<T> results) where T : IResult
        {
            foreach (var i in results)
            {
                if (!i.IsSuccess) return Result.Error(i.ErrorType, i.Message);
            }

            return Result.Success;
        }

        [Obsolete("实验性")]
        public static Result Any<T>(params IEnumerable<T> results) where T : IResult
        {
            foreach (var i in results)
            {
                if (i.IsSuccess) return Result.Success;
            }

            return Result.Success;
        }
    }


    /// <summary>
    /// 预设错误
    /// </summary>
    extension(Result)
    {
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
        public static Result InvalidOperation(string message = "无效操作")
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
        public static Result<T> InvalidOperation<T>(string message = "无效操作")
        {
            return Result.Error<T>(ErrorType.InvalidOperation, message);
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
        public static Result InvalidParameter(string message = "无效参数")
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
        public static Result<T> InvalidParameter<T>(string message = "无效参数")
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
        public static Result InvalidData(string message = "无效数据")
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
        public static Result<T> InvalidData<T>(string message = "无效数据")
        {
            return Result.Error<T>(ErrorType.InvalidData, message);
        }



        /// <summary>
        /// 创建不支持操作的错误结果
        /// </summary>
        /// <remarks>
        /// 当调用的功能未实现或当前上下文不支持时使用。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>未配置必要的转换器</item>
        /// <item>功能尚未实现</item>
        /// <item>当前协议不支持该操作</item>
        /// <item>版本不兼容的功能</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.NotSupported"/></para>
        /// </remarks>
        /// <param name="message">具体的错误描述，默认为"不支持"</param>
        /// <typeparam name="T">结果值类型</typeparam>
        /// <returns>包含不支持错误的 Result&lt;T&gt;</returns>
        public static Result NotSupported(string message = "不支持")
        {
            return Result.Error(ErrorType.NotSupported, message);
        }
        /// <summary>
        /// 创建不支持操作的错误结果
        /// </summary>
        /// <remarks>
        /// 当调用的功能未实现或当前上下文不支持时使用。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>未配置必要的转换器</item>
        /// <item>功能尚未实现</item>
        /// <item>当前协议不支持该操作</item>
        /// <item>版本不兼容的功能</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.NotSupported"/></para>
        /// </remarks>
        /// <param name="message">具体的错误描述，默认为"不支持"</param>
        /// <typeparam name="T">结果值类型</typeparam>
        /// <returns>包含不支持错误的 Result&lt;T&gt;</returns>
        public static Result<T> NotSupported<T>(string message = "不支持")
        {
            return Result.Error<T>(ErrorType.NotSupported, message);
        }



        /// <summary>
        /// 超时错误（带返回值）
        /// </summary>
        /// <remarks>
        /// 当操作在指定时间内未完成时使用。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>设备响应超时</item>
        /// <item>连接建立超时</item>
        /// <item>读取数据超时</item>
        /// <item>写入数据超时</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.Timeout"/></para>
        /// <para>这可能是网络问题、设备负载过高或超时时间设置过短。建议重试或检查网络状况。</para>
        /// </remarks>
        /// <typeparam name="T">结果值类型</typeparam>
        /// <param name="message">具体的错误描述，默认为"超时"</param>
        /// <returns>包含超时错误的 Result&lt;T&gt;</returns>
        public static Result Timeout(string message = "超时")
        {
            return Result.Error(ErrorType.Timeout, message);
        }
        /// <summary>
        /// 超时错误（带返回值）
        /// </summary>
        /// <remarks>
        /// 当操作在指定时间内未完成时使用。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>设备响应超时</item>
        /// <item>连接建立超时</item>
        /// <item>读取数据超时</item>
        /// <item>写入数据超时</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.Timeout"/></para>
        /// <para>这可能是网络问题、设备负载过高或超时时间设置过短。建议重试或检查网络状况。</para>
        /// </remarks>
        /// <typeparam name="T">结果值类型</typeparam>
        /// <param name="message">具体的错误描述，默认为"超时"</param>
        /// <returns>包含超时错误的 Result&lt;T&gt;</returns>
        public static Result<T> Timeout<T>(string message = "超时")
        {
            return Result.Error<T>(ErrorType.Timeout, message);
        }



        /// <summary>
        /// 操作已取消错误
        /// </summary>
        /// <remarks>
        /// 当操作被外部请求取消时使用。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>通过 <see cref="CancellationToken"/> 请求取消</item>
        /// <item>用户主动停止正在进行的操作</item>
        /// <item>上层调用方不再需要结果</item>
        /// <item>超时或关闭触发的取消</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.Cancelled"/></para>
        /// <para>这不是错误，而是正常的操作中止。调用方可以根据需要重试或忽略。</para>
        /// </remarks>
        /// <param name="message">具体的错误描述，默认为"已取消"</param>
        /// <returns>包含取消错误的 Result</returns>
        public static Result Cancelled(string message = "已取消")
        {
            return Result.Error(ErrorType.Cancelled, message);
        }
        /// <summary>
        /// <summary>
        /// 操作已取消错误
        /// </summary>
        /// <remarks>
        /// 当操作被外部请求取消时使用。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>通过 <see cref="CancellationToken"/> 请求取消</item>
        /// <item>用户主动停止正在进行的操作</item>
        /// <item>上层调用方不再需要结果</item>
        /// <item>超时或关闭触发的取消</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.Cancelled"/></para>
        /// <para>这不是错误，而是正常的操作中止。调用方可以根据需要重试或忽略。</para>
        /// </remarks>
        /// <param name="message">具体的错误描述，默认为"已取消"</param>
        /// <returns>包含取消错误的 Result</returns>
        public static Result<T> Cancelled<T>(string message = "已取消")
        {
            return Result.Error<T>(ErrorType.Cancelled, message);
        }



        /// <summary>
        /// 内部错误
        /// </summary>
        /// <remarks>
        /// 当库内部发生不应发生的错误时使用，通常是代码 Bug。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>逻辑错误（如不应该到达的分支）</item>
        /// <item>未知的枚举值</item>
        /// <item>意外的空引用</item>
        /// <item>状态不一致</item>
        /// <item>断言失败</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.Internal"/></para>
        /// <para>如果遇到此错误，请报告给开发者修复，这不应该是正常流程的一部分。</para>
        /// </remarks>
        /// <param name="message">具体的错误描述，默认为"内部错误"</param>
        /// <returns>包含内部错误的 Result</returns>
        public static Result Internal(string message = "内部错误")
        {
            return Result.Error(ErrorType.Cancelled, message);
        }
        /// <summary>
        /// 内部错误
        /// </summary>
        /// <remarks>
        /// 当库内部发生不应发生的错误时使用，通常是代码 Bug。
        /// <para>适用场景：</para>
        /// <list type="bullet">
        /// <item>逻辑错误（如不应该到达的分支）</item>
        /// <item>未知的枚举值</item>
        /// <item>意外的空引用</item>
        /// <item>状态不一致</item>
        /// <item>断言失败</item>
        /// </list>
        /// <para>错误类型: <see cref="ErrorType.Internal"/></para>
        /// <para>如果遇到此错误，请报告给开发者修复，这不应该是正常流程的一部分。</para>
        /// </remarks>
        /// <param name="message">具体的错误描述，默认为"内部错误"</param>
        /// <returns>包含内部错误的 Result</returns>
        public static Result<T> Internal<T>(string message = "内部错误")
        {
            return Result.Error<T>(ErrorType.Cancelled, message);
        }
    }
}