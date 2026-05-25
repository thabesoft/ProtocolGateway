using ThabeSoft.Modbus.Headers.Requests;
using ThabeSoft.Modbus.Headers.Response;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Encoding;

/// <summary>
/// 读操作编码器
/// </summary>
public interface IReadCodec
{
    /// <summary>
    /// 编码
    /// </summary>
    Result<int> EncodeRequest(Span<byte> destination, in ReadRequestHeader header);

    /// <summary>
    /// 解码线圈
    /// </summary>
    Result<ReadResponseHeader> DecodeCoilsResponse(ReadOnlySpan<byte> source, Span<bool> values);
    /// <summary>
    /// 解码寄存器
    /// </summary>
    Result<ReadResponseHeader> DecodeRegistersResponse(ReadOnlySpan<byte> source, Span<ushort> values);
}
