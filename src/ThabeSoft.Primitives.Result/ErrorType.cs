namespace ThabeSoft.Primitives;

/// <summary>
/// 错误类型
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// 没有错误
    /// </summary>
    /// <remarks>
    /// 仅用于 <see cref="Result.Success"/> 等成功状态，不应作为错误返回。
    /// </remarks>
    None = 0,

    /// <summary>
    /// 未指定的错误
    /// </summary>
    /// <remarks>
    /// 当无法确定具体错误类型时使用。
    /// <para>尽量避免使用此类型，应使用更具体的错误类型。</para>
    /// </remarks>
    Unspecified,

    /// <summary>
    /// 无效操作
    /// </summary>
    /// <remarks>
    /// 操作在当前状态下不被允许。
    /// <para>例如：未连接时尝试读写、重复执行不允许的操作、操作顺序错误等。</para>
    /// <para>这是调用方未正确处理状态机导致的问题。</para>
    /// </remarks>
    InvalidOperation,

    /// <summary>
    /// 参数无效
    /// </summary>
    /// <remarks>
    /// 调用方传入的参数有问题。
    /// <para>例如：缓冲区长度不足、参数超出范围、参数为 null 等。</para>
    /// <para>这是调用方的错误，需要调用方修正。</para>
    /// </remarks>
    InvalidParameter,

    /// <summary>
    /// 无效数据
    /// </summary>
    /// <remarks>
    /// 从外部（设备、文件、网络）接收到的数据格式错误或内容无效。
    /// <para>例如：CRC校验失败、数据长度不足、功能码不匹配等。</para>
    /// <para>这是数据源的问题，需要检查外部设备或数据链路。</para>
    /// </remarks>
    InvalidData,

    /// <summary>
    /// 不支持的操作
    /// </summary>
    /// <remarks>
    /// 调用的功能未实现或当前上下文不支持。
    /// <para>例如：未配置必要的转换器、当前协议不支持该操作、版本不兼容等。</para>
    /// <para>这通常是配置或环境问题。</para>
    /// </remarks>
    NotSupported,

    /// <summary>
    /// 操作超时
    /// </summary>
    /// <remarks>
    /// 操作在指定时间内未完成。
    /// <para>例如：设备响应超时、连接超时、读取超时等。</para>
    /// <para>这可能是网络问题、设备负载过高或超时时间设置过短。</para>
    /// </remarks>
    Timeout,

    /// <summary>
    /// 操作已取消
    /// </summary>
    /// <remarks>
    /// 操作被外部请求取消。
    /// <para>例如：通过 <see cref="CancellationToken"/> 请求取消、用户主动停止操作等。</para>
    /// </remarks>
    Cancelled,

    /// <summary>
    /// 内部错误
    /// </summary>
    /// <remarks>
    /// 库内部发生的错误，通常是代码 Bug。
    /// <para>例如：逻辑错误、未知的枚举值、意外的空引用等。</para>
    /// </remarks>
    Internal,
}