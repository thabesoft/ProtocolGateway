//using ThabeSoft.Modbus.Headers;
//using ThabeSoft.Primitives;


//namespace ThabeSoft.Modbus.Encoding;


///// <summary>
///// Rtu 请求解码器 (从站用)
///// </summary>
//public static class RequestDecoder
//{
//    public static Result<ReadRequesteHeader> ReadCoils(ReadOnlySpan<byte> source)
//    {
//        return Read(source, FunctionCode.ReadCoils, ReadRequestLayout.Instance);
//    }
//    public static Result<ReadRequesteHeader> ReadDiscreteInputs(ReadOnlySpan<byte> source)
//    {
//        return Read(source, FunctionCode.ReadDiscreteInputs, ReadRequestLayout.Instance);
//    }
//    public static Result<ReadRequesteHeader> ReadHoldingRegisters(ReadOnlySpan<byte> source)
//    {
//        return Read(source, FunctionCode.ReadHoldingRegisters, ReadRequestLayout.Instance);
//    }
//    public static Result<ReadRequesteHeader> ReadInputRegisters(ReadOnlySpan<byte> source)
//    {
//        return Read(source, FunctionCode.ReadInputRegisters, ReadRequestLayout.Instance);
//    }


//    public static Result<WriteSingleCoilHeader> WriteSingleCoil(ReadOnlySpan<byte> source)
//    {
//        return WriteSingleCoil(source, WriteSingleLayout.Instance);
//    }
//    public static Result<WriteSingleCoilHeader> WriteSingleCoil(ReadOnlySpan<byte> source, WriteSingleLayout layout)
//    {
//        // 缓冲区不足
//        if (source.Length < layout.TotalLength)
//            return BufferTooSmall<WriteSingleCoilHeader>(nameof(WriteSingleCoil), layout.TotalLength, source.Length);

//        // 从站
//        var slaveId = source[layout.SlaveIdIndex];
//        // 功能码
//        var function_code = FunctionCode.WriteSingleCoil;
//        // 起始地址
//        var address_result = source[layout.AddressRange]
//            .ToWord(Endianness.BigEndian);
//        if (!address_result) return address_result.PropagateError<WriteSingleCoilHeader>();

//        // 值
//        var value_result = source[layout.ValueRange]
//            .ToWord(Endianness.BigEndian)
//            .Bind(ProtocolExtensions.GetSingleCoilValue);
//        if (!value_result) return value_result.PropagateError<WriteSingleCoilHeader>();

//        // Crc
//        var crc_result = source[layout.CrcRange]
//            .ToWord(Endianness.LittleEndian);
//        if (!crc_result) return crc_result.PropagateError<WriteSingleCoilHeader>();

//        // 验证
//        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
//        if (!validate_result) return validate_result.PropagateError<WriteSingleCoilHeader>();


//        return new WriteSingleCoilHeader(
//            slaveId: slaveId,
//            address: address_result.Value,
//            value: value_result,
//            crc: crc_result.Value);
//    }

//    public static Result<RtuWriteSingleRegisterHeader> WriteSingleRegister(ReadOnlySpan<byte> source)
//    {
//        return WriteSingleRegister(source, WriteSingleLayout.Instance);
//    }
//    public static Result<RtuWriteSingleRegisterHeader> WriteSingleRegister(ReadOnlySpan<byte> source, in WriteSingleLayout layout)
//    {
//        // 缓冲区不足
//        if (source.Length < layout.TotalLength)
//            return BufferTooSmall<RtuWriteSingleRegisterHeader>(nameof(WriteSingleRegister), layout.TotalLength, source.Length);

//        // 从站
//        var slaveId = source[layout.SlaveIdIndex];
//        // 功能码
//        var function_code = FunctionCode.WriteSingleCoil;

//        // 起始地址
//        var address_result = source[layout.AddressRange]
//            .ToWord(Endianness.BigEndian);
//        if (!address_result) return address_result.PropagateError<RtuWriteSingleRegisterHeader>();

//        // 值
//        var value_result = source[layout.ValueRange]
//            .ToWord(Endianness.BigEndian);
//        if (!value_result) return value_result.PropagateError<RtuWriteSingleRegisterHeader>();

//        // Crc
//        var crc_result = source[layout.CrcRange]
//            .ToWord(Endianness.LittleEndian);
//        if (!crc_result) return crc_result.PropagateError<RtuWriteSingleRegisterHeader>();

//        // 验证
//        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
//        if (!validate_result) return validate_result.PropagateError<RtuWriteSingleRegisterHeader>();


//        return new RtuWriteSingleRegisterHeader(
//            slaveId: slaveId,
//            address: address_result.Value,
//            value: value_result.Value,
//            crc: crc_result.Value);
//    }


//    public static Result<WriteMultipleResponseHeader> WriteMultipleCoils(ReadOnlySpan<byte> source, Span<bool> destination)
//    {
//        // 根据数量创建布局
//        var layout_result = WriteCoilsQuantity.Create(destination.Length)
//            .Bind(WriteMultipleRequestLayout.FromCoilsQuantity);
//        if (!layout_result) return layout_result.PropagateError<WriteMultipleResponseHeader>();

//        return WriteMultipleCoils(source, destination, layout_result.Value);
//    }
//    public static Result<WriteMultipleResponseHeader> WriteMultipleCoils(ReadOnlySpan<byte> source, Span<bool> destination, in WriteMultipleRequestLayout layout)
//    {
//        // 缺少请求头
//        if (layout == WriteMultipleRequestLayout.Empty)
//            return MissingRequestLayout<WriteMultipleResponseHeader>(nameof(WriteMultipleCoils), nameof(WriteMultipleRequestLayout));

//        // 校验包长度
//        if (source.Length < layout.TotalLength)
//            return BufferTooSmall<WriteMultipleResponseHeader>(nameof(WriteMultipleCoils), layout.TotalLength, source.Length);

//        // 缓冲区不足
//        if (destination.Length < layout.DataQuantity)
//            return ValueBufferTooSmall<WriteMultipleResponseHeader>(nameof(WriteMultipleCoils), layout.TotalLength, source.Length);

//        // 从站Id
//        var slave_id = source[layout.SlaveIdIndex];
//        // 功能码
//        var function_code = FunctionCode.WriteMultipleCoils;
//        // 起始地址
//        var address_result = source[layout.AddressRange].ToWord(Endianness.BigEndian);
//        if (!address_result) return address_result.PropagateError<WriteMultipleResponseHeader>();
//        // 数量
//        var quantity_result = source[layout.QuantityRange].ToWord(Endianness.BigEndian);
//        if (!quantity_result) return quantity_result.PropagateError<WriteMultipleResponseHeader>();
//        // 数据长度
//        var data_length = source[layout.DataLengthIndex];
//        if (data_length != layout.DataLength)
//            return Result.Error<WriteMultipleResponseHeader>(ErrorType.InvalidData, "数据长度不匹配");
//        // Crc
//        var crc_result = source[layout.CrcRange].ToWord(Endianness.LittleEndian);
//        if (!crc_result) return crc_result.PropagateError<WriteMultipleResponseHeader>();
//        // 验证
//        if (!CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value))
//            return Result.Error<WriteMultipleResponseHeader>(ErrorType.InvalidData, "Crc校验失败");
//        // 数据
//        Span<bool> buffer = stackalloc bool[layout.DataQuantity];
//        var value_result = source[layout.DataRange].ToBits(buffer, BitOrder.LSB0);
//        if (!value_result) return value_result.PropagateError<WriteMultipleResponseHeader>();

//        buffer.CopyTo(destination);
//        return new WriteMultipleResponseHeader(
//            slaveId: slave_id,
//            functionCode: FunctionCode.WriteMultipleCoils,
//            address: address_result.Value,
//            quantity: quantity_result.Value,
//            crc: crc_result.Value);
//    }


//    public static Result<WriteMultipleResponseHeader> WriteMultipleRegisters(ReadOnlySpan<byte> source, Span<ushort> destination)
//    {
//        // 根据数量创建布局
//        var layout_result = WriteRegistersQuantity.Create(destination.Length)
//            .Bind(WriteMultipleRequestLayout.FromRegistersQuantity);
//        if (!layout_result) return layout_result.PropagateError<WriteMultipleResponseHeader>();

//        return WriteMultipleRegisters(source, destination, layout_result.Value);
//    }
//    public static Result<WriteMultipleResponseHeader> WriteMultipleRegisters(ReadOnlySpan<byte> source, Span<ushort> destination, in WriteMultipleRequestLayout layout)
//    {
//        // 缺少请求头
//        if (layout == WriteMultipleRequestLayout.Empty)
//            return MissingRequestLayout<WriteMultipleResponseHeader>(nameof(WriteMultipleRegisters), nameof(WriteMultipleRequestLayout));

//        // 校验包长度
//        if (source.Length < layout.TotalLength)
//            return BufferTooSmall<WriteMultipleResponseHeader>(nameof(WriteMultipleRegisters), layout.TotalLength, source.Length);

//        // 缓冲区不足
//        if (destination.Length < layout.DataQuantity)
//            return ValueBufferTooSmall<WriteMultipleResponseHeader>(nameof(WriteMultipleRegisters), layout.TotalLength, source.Length);

//        // 从站Id
//        var slave_id = source[layout.SlaveIdIndex];
//        // 功能码
//        var function_code = FunctionCode.WriteMultipleRegisters;
//        // 起始地址
//        var address_result = source[layout.AddressRange].ToWord(Endianness.BigEndian);
//        if (!address_result) return address_result.PropagateError<WriteMultipleResponseHeader>();
//        // 数量
//        var quantity_result = source[layout.QuantityRange].ToWord(Endianness.BigEndian);
//        if (!quantity_result) return quantity_result.PropagateError<WriteMultipleResponseHeader>();
//        // 数据长度
//        var data_length = source[layout.DataLengthIndex];
//        if (data_length != layout.DataLength)
//            return Result.InvalidData<WriteMultipleResponseHeader>("数据长度不匹配");
//        // Crc
//        var crc_result = source[layout.CrcRange].ToWord(Endianness.LittleEndian);
//        if (!crc_result) return crc_result.PropagateError<WriteMultipleResponseHeader>();
//        // 验证
//        if (!CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value))
//            return Result.InvalidData<WriteMultipleResponseHeader>("Crc校验失败");
//        // 数据
//        Span<ushort> buffer = stackalloc ushort[layout.DataQuantity];
//        var value_result = source[layout.DataRange].ToWords(buffer, Endianness.BigEndian);
//        if (!value_result) return value_result.PropagateError<WriteMultipleResponseHeader>();

//        buffer.CopyTo(destination);
//        return new WriteMultipleResponseHeader(
//            slaveId: slave_id,
//            functionCode: FunctionCode.WriteMultipleRegisters,
//            address: address_result.Value,
//            quantity: quantity_result.Value,
//            crc: crc_result.Value);
//    }


//    /// <summary>
//    /// 读取请求解码
//    /// </summary>
//    /// <param name="source">源码</param>
//    /// <param name="functionCode">是什么功能</param>
//    /// <param name="layout">请求布局</param>
//    /// <returns></returns>
//    private static Result<ReadRequesteHeader> Read(ReadOnlySpan<byte> source, FunctionCode functionCode, in ReadRequestLayout layout)
//    {
//        // 数据不足
//        if (source.Length < layout.TotalLength)
//            return BufferTooSmall<ReadRequesteHeader>($"Read ({functionCode})", layout.TotalLength, source.Length);

//        // 从站
//        var slave_id = source[layout.SlaveIdIndex];
//        // 功能码
//        var function_code_result = FunctionCode
//            .FromCode(source[layout.FunctionCodeIndex])
//            .Where(x => x == functionCode);
//        if (!function_code_result) return function_code_result.PropagateError<ReadRequesteHeader>();

//        // 起始地址
//        var address_result = source[layout.AddressRange]
//            .ToWord(Endianness.BigEndian);
//        if (!address_result) return address_result.PropagateError<ReadRequesteHeader>();

//        // 数量
//        var quantity_result = source[layout.QuantityRange]
//            .ToWord(Endianness.BigEndian);
//        if (!quantity_result) return quantity_result.PropagateError<ReadRequesteHeader>();

//        // Crc
//        var crc_result = source[layout.CrcRange]
//            .ToWord(Endianness.LittleEndian);
//        if (!crc_result) return crc_result.PropagateError<ReadRequesteHeader>();

//        // 验证
//        var validate_result = CrcCalculator.Validate(source[layout.PayloadRange], crc_result.Value);
//        if (!validate_result) return validate_result.PropagateError<ReadRequesteHeader>();


//        return new ReadRequesteHeader(
//           slaveId: slave_id,
//           functionCode: function_code_result.Value,
//           address: address_result.Value,
//           quantity: quantity_result.Value,
//           crc: crc_result.Value);
//    }



//    private static Result MissingRequestHeader(string actionName) => Result.InvalidParameter(
//        $"[{actionName}] 请求头不可为空");
//    public static Result<T> MissingRequestLayout<T>(string actionName, string layoutName) => Result.InvalidParameter<T>(
//       $"[{actionName}] 缺少布局信息: {layoutName}");
//    private static Result<T> BufferTooSmall<T>(string actionName, int required, int actual)
//        => Result.InvalidParameter<T>($"[{actionName}] 解码所需建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
//    private static Result<T> ValueBufferTooSmall<T>(string actionName, int required, int actual)
//        => Result.InvalidParameter<T>($"[{actionName}] 解码所需值建缓冲区不足，需要 {required} 字节，实际 {actual} 字节");
//}