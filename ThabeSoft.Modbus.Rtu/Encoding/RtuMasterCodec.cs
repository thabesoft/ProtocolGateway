using ThabeSoft.Modbus.Headers;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Encoding;


/// <summary>
/// Rtu主站解码器
/// </summary>
public sealed class RtuMasterCodec : IMasterCodec
{
    private readonly IReadCodec _readCodec = RtuReadCodec.Instance;
    private readonly IWriteSingleCodec _writeSingle = RtuWriteSingleCodec.Instance;
    private readonly IWriteMultipleCodec _writeMultiple = RtuWriteMultipleCodec.Instance;


    private RtuMasterCodec() { }
    public static RtuMasterCodec Instance { get; } = new();



    Result<ReadResponseHeader> IReadCodec.DecodeCoilsResponse(ReadOnlySpan<byte> source, Span<bool> values)
        => _readCodec.DecodeCoilsResponse(source, values);
    Result<ReadResponseHeader> IReadCodec.DecodeRegistersResponse(ReadOnlySpan<byte> source, Span<ushort> values)
        => _readCodec.DecodeRegistersResponse(source, values);
    Result<int> IReadCodec.EncodeRequest(Span<byte> destination, in ReadRequestHeader header)
        => _readCodec.EncodeRequest(destination, header);


    Result<WriteSingleCoilHeader> IWriteSingleCodec.DecodeCoilResponse(ReadOnlySpan<byte> source)
        => _writeSingle.DecodeCoilResponse(source);
    Result<WriteSingleRegisterHeader> IWriteSingleCodec.DecodeRegisterResponse(ReadOnlySpan<byte> source)
        => _writeSingle.DecodeRegisterResponse(source);
    Result<int> IWriteSingleCodec.EncodeCoilRequest(Span<byte> destination, in WriteSingleCoilHeader header)
        => _writeSingle.EncodeCoilRequest(destination, header);
    Result<int> IWriteSingleCodec.EncodeRegisterRequest(Span<byte> destination, in WriteSingleRegisterHeader header)
        => _writeSingle.EncodeRegisterRequest(destination, header);


    Result<WriteMultipleResponseHeader> IWriteMultipleCodec.DecodeCoilsResponse(ReadOnlySpan<byte> source)
        => _writeMultiple.DecodeCoilsResponse(source);
    Result<WriteMultipleResponseHeader> IWriteMultipleCodec.DecodeRegistersResponse(ReadOnlySpan<byte> source)
        => _writeMultiple.DecodeRegistersResponse(source);
    Result<int> IWriteMultipleCodec.EncodeCoilsRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<bool> values)
        => _writeMultiple.EncodeCoilsRequest(destination, header, values);
    Result<int> IWriteMultipleCodec.EncodeRegistersRequest(Span<byte> destination, in WriteMultipleRequestHeader header, ReadOnlySpan<ushort> values)
        => _writeMultiple.EncodeRegistersRequest(destination, header, values);
}
