using ThabeSoft.Modbus.Headers;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Encoding;


/// <summary>
/// 写单值编码器
/// </summary>
public interface IWriteSingleCodec
{
    /// <summary>
    /// 写线圈值
    /// </summary>
    Result<int> EncodeCoilRequest(Span<byte> destination, in WriteSingleCoilHeader header);
    /// <summary>
    /// 编码写单寄存器请求
    /// </summary>
    Result<int> EncodeRegisterRequest(Span<byte> destination, in WriteSingleRegisterHeader header);

    /// <summary>
    /// 解码写单线圈响应
    /// </summary>
    Result<WriteSingleCoilHeader> DecodeCoilResponse(ReadOnlySpan<byte> source);
    /// <summary>
    /// 解码写单寄存器响应
    /// </summary>
    Result<WriteSingleRegisterHeader> DecodeRegisterResponse(ReadOnlySpan<byte> source);
}