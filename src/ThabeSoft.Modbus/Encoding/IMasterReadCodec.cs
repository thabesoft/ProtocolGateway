using ThabeSoft.Modbus.Headers;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Encoding;

/// <summary>
/// 读操作编码器
/// </summary>
public interface IMasterReadCodec
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


/// <summary>
/// 从站读请求编码器
/// </summary>
public interface ISlaveReadCodec
{
    /// <summary>
    /// 解码请求
    /// </summary>
    Result<ReadRequestHeader> DecodeRequest(ReadOnlySpan<byte> source);

    /// <summary>
    /// 编码线圈
    /// </summary>
    Result<int> EncodeCoilsResponse(Span<byte> destination, in ReadResponseHeader header, Span<bool> values);
    /// <summary>
    /// 编码寄存器
    /// </summary>
    Result<int> EncodeRegistersResponse(Span<byte> destination, in ReadResponseHeader header, Span<ushort> values);
}
