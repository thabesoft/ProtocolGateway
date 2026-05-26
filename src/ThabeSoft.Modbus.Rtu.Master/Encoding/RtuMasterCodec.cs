using ThabeSoft.Modbus.Headers;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Encoding;


/// <summary>
/// Rtu主站解码器
/// </summary>
public sealed class RtuMasterCodec : IMasterCodec
{
    private readonly IMasterReadCodec _readCodec = RtuMasterReadCodec.Instance;
    private readonly IMasterWriteSingleCodec _writeSingle = RtuMasterWriteSingleCodec.Instance;
    private readonly IMasterWriteMultipleCodec _writeMultiple = RtuMasterWriteMultipleCodec.Instance;


    private RtuMasterCodec() { }
    public static RtuMasterCodec Instance { get; } = new();



    Result<ReadResponseHeader> IMasterReadCodec.DecodeCoilsResponse(ReadOnlySpan<byte> source, Span<bool> values)
        => _readCodec.DecodeCoilsResponse(source, values);
    Result<ReadResponseHeader> IMasterReadCodec.DecodeRegistersResponse(ReadOnlySpan<byte> source, Span<ushort> values)
        => _readCodec.DecodeRegistersResponse(source, values);
    Result<int> IMasterReadCodec.EncodeRequest(Span<byte> destination, in ReadRequestHeader header)
        => _readCodec.EncodeRequest(destination, header);


    Result<WriteSingleCoilHeader> IMasterWriteSingleCodec.DecodeCoilResponse(ReadOnlySpan<byte> source)
        => _writeSingle.DecodeCoilResponse(source);
    Result<WriteSingleRegisterHeader> IMasterWriteSingleCodec.DecodeRegisterResponse(ReadOnlySpan<byte> source)
        => _writeSingle.DecodeRegisterResponse(source);
    Result<int> IMasterWriteSingleCodec.EncodeCoilRequest(Span<byte> destination, in WriteSingleCoilHeader header)
        => _writeSingle.EncodeCoilRequest(destination, header);
    Result<int> IMasterWriteSingleCodec.EncodeRegisterRequest(Span<byte> destination, in WriteSingleRegisterHeader header)
        => _writeSingle.EncodeRegisterRequest(destination, header);


    Result<WriteMultipleResponseHeader> IMasterWriteMultipleCodec.DecodeCoilsResponse(ReadOnlySpan<byte> source)
        => _writeMultiple.DecodeCoilsResponse(source);
    Result<WriteMultipleResponseHeader> IMasterWriteMultipleCodec.DecodeRegistersResponse(ReadOnlySpan<byte> source)
        => _writeMultiple.DecodeRegistersResponse(source);
    Result<int> IMasterWriteMultipleCodec.EncodeCoilsRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<bool> values)
        => _writeMultiple.EncodeCoilsRequest(destination, header, values);
    Result<int> IMasterWriteMultipleCodec.EncodeRegistersRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<ushort> values)
        => _writeMultiple.EncodeRegistersRequest(destination, header, values);
}
