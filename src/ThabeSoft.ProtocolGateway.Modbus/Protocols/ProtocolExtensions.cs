using System.Runtime.CompilerServices;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Modbus 协议扩展
/// </summary>
public static class ProtocolExtensions
{
    private const ushort CoilOpen = 0xFF00;
    private const ushort CoilClose = 0x0000;

    /// <summary>
    /// 获取线圈数量转为字节后的长度
    /// </summary>
    /// <param name="quantity">线圈数量</param>
    /// <returns>锁占用的字节</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetColisToByteLength(ushort quantity) => (quantity + 7) / 8;

    /// <summary>
    /// 获取寄存器转为字节后的长度
    /// </summary>
    /// <param name="quantity">寄存器数量</param>
    /// <returns>所占用的字节</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRegistersToByteLength(ushort quantity) => quantity * 2;

    /// <summary>
    /// 获取单线圈的值 (Modbus写入值最低要求 两个字节)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetCoilWordValue(bool value) => value ? CoilOpen : CoilClose;

    /// <summary>
    /// 获取单线圈的值
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result GetSingleCoilValue(ushort value)
    {
        return value switch
        {
            CoilOpen => true,
            CoilClose => false,
            _ => Result.Error(ErrorType.InvalidParameter, $"无效的 Modbus 线圈值: 0x{value:X4}，有效值为 0xFF00 (ON) 或 0x0000 (OFF)")
        };
    }


    extension(Result)
    {
        /// <summary>
        /// 缺少请求头
        /// </summary>
        public static Result MissingRequestHeader()
            => Result.Error(ErrorType.InvalidParameter, "请求头不可为空");
        /// <summary>
        /// 缺少请求头
        /// </summary>
        public static Result<T> MissingRequestHeader<T>()
            => Result.Error<T>(ErrorType.InvalidParameter, "请求头不可为空");


        /// <summary>
        /// 缺少请求布局
        /// </summary>
        public static Result MissingRequestLayout()
            => Result.Error(ErrorType.InvalidParameter, "请求布局不可为空");
        /// <summary>
        /// 缺少请求布局
        /// </summary>
        public static Result<T> MissingRequestLayout<T>()
            => Result.Error<T>(ErrorType.InvalidParameter, "请求布局不可为空");



        /// <summary>
        /// 缓冲区不足错误
        /// </summary>
        public static Result BufferInsufficient(int required, int actual)
            => Result.Error(ErrorType.InvalidParameter,
                $"缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
        /// <summary>
        /// 缓冲区不足错误
        /// </summary>
        public static Result<T> BufferInsufficient<T>(int required, int actual) 
            => Result<T>.Error(ErrorType.InvalidParameter,
                $"缓冲区不足，需要 {required} 字节，实际 {actual} 字节");



        /// <summary>
        /// 数据长度不足错误
        /// </summary>
        public static Result DataTooShort(int required, int actual) 
            => Result.Error(ErrorType.InvalidData,
                $"数据长度不足，需要 {required} 字节，实际 {actual} 字节");
        /// <summary>
        /// 数据长度不足错误
        /// </summary>
        public static Result<T> DataTooShort<T>(int required, int actual) 
            => Result<T>.Error(ErrorType.InvalidData,
                $"数据长度不足，需要 {required} 字节，实际 {actual} 字节");
    }
}