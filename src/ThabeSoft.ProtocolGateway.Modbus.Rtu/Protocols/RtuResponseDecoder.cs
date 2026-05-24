using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Primitives;


namespace ThabeSoft.ProtocolGateway.Modbus.Protocols;


/// <summary>
/// Rtu 响应解码器
/// </summary>
public static class RtuResponseDecoder
{
    public static Result ReadCoils(ReadOnlySpan<byte> source, Span<bool> values, in RtuReadResponseLayout layout)
    {
        // 功能码
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => x == FunctionCode.ReadCoils);
        if (!function_code_result) return function_code_result;

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result;

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result;
         
        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 数据长度
        var data_length = source[layout.DataLengthIndex];
        // 数据
        Span<bool> buffer = stackalloc bool[layout.DataQuantity];
        var data_result = source[layout.DataRange].ToBits(buffer);
        if (!data_result) return data_result;

        // 拷贝数据
        buffer.CopyTo(values);
        return true;
    }




    private static Result MissingRequestHeader(string actionName) => Result.InvalidParameter(
        $"[{actionName}] 请求头不可为空");
    public static Result<T> MissingRequestLayout<T>(string actionName, string layoutName) => Result.InvalidParameter<T>(
       $"[{actionName}] 缺少布局信息: {layoutName}");
    private static Result<T> BufferTooSmall<T>(string actionName, int required, int actual)
        => Result.InvalidParameter<T>($"[{actionName}] 解码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
    private static Result<T> ValueBufferTooSmall<T>(string actionName, int required, int actual)
        => Result.InvalidParameter<T>($"[{actionName}] 解码所需值建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
}