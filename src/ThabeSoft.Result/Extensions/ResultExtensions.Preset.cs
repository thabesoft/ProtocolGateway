namespace ThabeSoft.Primitives;


/// <summary>
/// 错误预设
/// </summary>
public static partial class ResultExtensions
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
    }
}