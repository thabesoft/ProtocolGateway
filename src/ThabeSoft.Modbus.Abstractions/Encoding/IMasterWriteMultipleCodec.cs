using ThabeSoft.Modbus.Headers;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Encoding;


/// <summary>
/// 主站编解码器（主站请求编码 + 从站响应解码）
/// </summary>
public interface IMasterWriteMultipleCodec
{
    /// <summary>
    /// 编码写多线圈请求
    /// </summary>
    Result<int> EncodeCoilsRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<bool> values);
    /// <summary>
    /// 解码写多线圈响应
    /// </summary>
    Result<WriteMultipleResponseHeader> DecodeCoilsResponse(ReadOnlySpan<byte> source);


    /// <summary>
    /// 编码写多寄存器请求
    /// </summary>
    Result<int> EncodeRegistersRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<ushort> values);
    /// <summary>
    /// 解码写多寄存器响应
    /// </summary>
    Result<WriteMultipleResponseHeader> DecodeRegistersResponse(ReadOnlySpan<byte> source);
}