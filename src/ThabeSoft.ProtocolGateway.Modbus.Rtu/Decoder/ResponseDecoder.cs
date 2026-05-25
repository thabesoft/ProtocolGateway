using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Protocols;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Headers;
using ThabeSoft.ProtocolGateway.Modbus.Rtu.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Primitives;


namespace ThabeSoft.ProtocolGateway.Modbus.Rtu.Decoder;


/// <summary>
/// Rtu 响应解码器
/// </summary>
public static class ResponseDecoder
{
    public static Result<ReadResponseHeader> ReadCoils(ReadOnlySpan<byte> source, Span<bool> values, in ReadResponseLayout layout)
    {
        static Result FunctionCodeVerify(FunctionCode code) => code == FunctionCode.ReadCoils ? Result.Success :
            Result.InvalidParameter($"解码失败, 功能码比匹配, 预期:{FunctionCode.ReadCoils}, 实际: {code}");

        return ReadCoils(
            source: source,
            functionCodeVerifyHandler: FunctionCodeVerify,
            values: values,
            layout: layout);
    }
    public static Result<ReadResponseHeader> ReadDiscreteInputs(ReadOnlySpan<byte> source, Span<bool> values, in ReadResponseLayout layout)
    {
        static Result FunctionCodeVerify(FunctionCode code) => code == FunctionCode.ReadDiscreteInputs ? Result.Success :
            Result.InvalidParameter($"解码失败, 功能码比匹配, 预期:{FunctionCode.ReadDiscreteInputs}, 实际: {code}");

        return ReadCoils(
            source: source,
            functionCodeVerifyHandler: FunctionCodeVerify,
            values: values,
            layout: layout);
    }
    public static Result<ReadResponseHeader> ReadHoldingRegisters(ReadOnlySpan<byte> source, Span<ushort> values, in ReadResponseLayout layout)
    {
        static Result FunctionCodeVerify(FunctionCode code) => code == FunctionCode.ReadHoldingRegisters ? Result.Success :
            Result.InvalidParameter($"解码失败, 功能码比匹配, 预期:{FunctionCode.ReadHoldingRegisters}, 实际: {code}");

        return ReadRegister(
            source: source,
            functionCodeVerifyHandler: FunctionCodeVerify,
            values: values,
            layout: layout);
    }
    public static Result<ReadResponseHeader> ReadInputRegisters(ReadOnlySpan<byte> source, Span<ushort> values, in ReadResponseLayout layout)
    {
        static Result FunctionCodeVerify(FunctionCode code) => code == FunctionCode.ReadInputRegisters ? Result.Success :
            Result.InvalidParameter($"解码失败, 功能码比匹配, 预期:{FunctionCode.ReadInputRegisters}, 实际: {code}");

        return ReadRegister(
            source: source,
            functionCodeVerifyHandler: FunctionCodeVerify,
            values: values,
            layout: layout);
    }

    public static Result<WriteSingleCoilHeader> WriteSingleCoils(ReadOnlySpan<byte> source, in WriteSingleLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteSingleCoil == x);
        if (!function_code_result) return function_code_result.PropagateError<WriteSingleCoilHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<WriteSingleCoilHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<WriteSingleCoilHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<WriteSingleCoilHeader>();
        // 数据
        var value_result = source[layout.ValueRange].ToWord()
            .Bind(ProtocolExtensions.GetSingleCoilValue);
        if (!value_result) return value_result.PropagateError<WriteSingleCoilHeader>();

        // 拷贝数据
        return new WriteSingleCoilHeader(
            slaveId: slave_id,
            address: address_result.Value,
            value: value_result.Value,
            crc: crc_result.Value);
    }
    public static Result<RtuWriteSingleRegisterHeader> WriteSingleRegister(ReadOnlySpan<byte> source, in WriteSingleLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteSingleCoil == x);
        if (!function_code_result) return function_code_result.PropagateError<RtuWriteSingleRegisterHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<RtuWriteSingleRegisterHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<RtuWriteSingleRegisterHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddressRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<RtuWriteSingleRegisterHeader>();
        // 数据
        var value_result = source[layout.ValueRange]
            .ToWord(Endianness.BigEndian);
        if (!value_result) return value_result.PropagateError<RtuWriteSingleRegisterHeader>();

        // 拷贝数据
        return new RtuWriteSingleRegisterHeader(
            slaveId: slave_id,
            address: address_result.Value,
            value: value_result.Value,
            crc: crc_result.Value);
    }
    public static Result<WriteMultipleResponseHeader> WriteMultipleCoils(ReadOnlySpan<byte> source, in WriteMultipleResponseLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteMultipleCoils == x);
        if (!function_code_result) return function_code_result.PropagateError<WriteMultipleResponseHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<WriteMultipleResponseHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<WriteMultipleResponseHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddrassRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<WriteMultipleResponseHeader>();
        // 数据数量
        var quantity_result = source[layout.QuantityRange]
            .ToWord(Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<WriteMultipleResponseHeader>();

        // 拷贝数据
        return new WriteMultipleResponseHeader(
            slaveId: slave_id,
            functionCode: function_code_result.Value,
            address: address_result.Value,
            quantity: quantity_result.Value,
            crc: crc_result.Value);
    }
    public static Result<WriteMultipleResponseHeader> WriteMultipleRegisters(ReadOnlySpan<byte> source, in WriteMultipleResponseLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex])
            .Where(x => FunctionCode.WriteMultipleRegisters == x);
        if (!function_code_result) return function_code_result.PropagateError<WriteMultipleResponseHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<WriteMultipleResponseHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<WriteMultipleResponseHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 地址
        var address_result = source[layout.AddrassRange]
            .ToWord(Endianness.BigEndian);
        if (!address_result) return address_result.PropagateError<WriteMultipleResponseHeader>();
        // 数据数量
        var quantity_result = source[layout.QuantityRange]
            .ToWord(Endianness.BigEndian);
        if (!quantity_result) return quantity_result.PropagateError<WriteMultipleResponseHeader>();

        // 拷贝数据
        return new WriteMultipleResponseHeader(
            slaveId: slave_id,
            functionCode: function_code_result.Value,
            address: address_result.Value,
            quantity: quantity_result.Value,
            crc: crc_result.Value);
    }





    /// <summary>
    /// 读线圈
    /// </summary>
    /// <param name="source">源码</param>
    /// <param name="functionCodeVerifyHandler">功能码验证</param>
    /// <param name="values">读到的寄存器值</param>
    /// <param name="layout">读响应布局</param>
    private static Result<ReadResponseHeader> ReadCoils(
        ReadOnlySpan<byte> source,
        Func<FunctionCode, Result> functionCodeVerifyHandler,
        Span<bool> values,
        in ReadResponseLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex]);
        if (!function_code_result) return function_code_result.PropagateError<ReadResponseHeader>();

        // 功能码验证结果
        var function_verify_result = functionCodeVerifyHandler(function_code_result.Value);
        if (!function_verify_result) return function_verify_result.PropagateError<ReadResponseHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<ReadResponseHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<ReadResponseHeader>();

        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 数据长度
        var data_length = source[layout.DataLengthIndex];
        // 数据
        Span<bool> buffer = stackalloc bool[layout.DataQuantity];
        var data_result = source[layout.DataRange].ToBits(buffer, BitOrder.LSB0);
        if (!data_result) return data_result.PropagateError<ReadResponseHeader>();

        // 拷贝数据
        buffer.CopyTo(values);

        return new ReadResponseHeader(
            slaveId: slave_id,
            functionCode: function_code_result.Value,
            dataLength: data_length,
            crc: crc_result.Value);
    }
    /// <summary>
    /// 读寄存器
    /// </summary>
    /// <param name="source">源码</param>
    /// <param name="functionCodeVerifyHandler">功能码验证</param>
    /// <param name="values">读到的寄存器值</param>
    /// <param name="layout">读响应布局</param>
    private static Result<ReadResponseHeader> ReadRegister(
        ReadOnlySpan<byte> source,
        Func<FunctionCode, Result> functionCodeVerifyHandler,
        Span<ushort> values,
        in ReadResponseLayout layout)
    {
        // 功能码创建结果
        var function_code_result = FunctionCode
            .FromCode(source[layout.FunctionCodeIndex]);
        if (!function_code_result) return function_code_result.PropagateError<ReadResponseHeader>();

        // 功能码验证结果
        var function_verify_result = functionCodeVerifyHandler(function_code_result.Value);
        if (!function_verify_result) return function_verify_result.PropagateError<ReadResponseHeader>();

        // Crc
        var crc_result = source[layout.CrcRange]
            .ToWord(Endianness.LittleEndian);
        if (!crc_result) return crc_result.PropagateError<ReadResponseHeader>();

        // 验证
        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
        if (!validate_result) return validate_result.PropagateError<ReadResponseHeader>();


        // 从站
        var slave_id = source[layout.SlaveIdIndex];
        // 数据长度
        var data_length = source[layout.DataLengthIndex];
        // 数据
        Span<ushort> buffer = stackalloc ushort[layout.DataQuantity];
        var data_result = source[layout.DataRange].ToWords(buffer, Endianness.BigEndian);
        if (!data_result) return data_result.PropagateError<ReadResponseHeader>();

        // 拷贝数据
        buffer.CopyTo(values);

        return new ReadResponseHeader(
            slaveId: slave_id,
            functionCode: function_code_result.Value,
            dataLength: data_length,
            crc: crc_result.Value);
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