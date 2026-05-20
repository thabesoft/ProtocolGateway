namespace IndustrialHub.Modbus.Protocol.Rtu;

/// <summary>
/// Rtu协议
/// </summary>
public static class RtuProtocol
{
    public static IRtuReadCoilsFrameLayout ReadCoils() => RtuReadFrameLayouts.Instance;
    public static IRtuReadHoldingRegistersFrameLayout ReadHoldingRegisters() => RtuReadFrameLayouts.Instance;
    public static IRtuReadInputRegistersFrameLayout ReadInputRegisters() => RtuReadFrameLayouts.Instance;
    public static IRtuReadDiscreteInputsFrameLayout ReadDiscreteInputs() => RtuReadFrameLayouts.Instance;


    /// <summary>
    /// 创建一个拥有指定数量寄存器的帧布局
    /// </summary>
    /// <param name="quantity">寄存器数量</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IRtuWriteMultipleRegistersFrameLayout WriteMultipleRegisters(byte quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "数量必须大于0");
        }
        if (quantity > 125)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "寄存器数量不能超过125");
        }

        // Data
        var data_byte_length = ModbusFrameLayout.GetRegistersToByteLength(quantity);
        const int data_start = 7;
        int data_end = data_start + data_byte_length;
        var data_range = new Range(data_start, data_end);

        // Content
        var content = new Range(0, data_end);

        // Crc
        var crc_start = data_end;
        var crc_end = data_end + 2;
        var crc_range = new Range(crc_start, crc_end);

        return new RtuWriteMultipleFrameLayout(
            dataRange: data_range,
            contentRange: content,
            crcRange: crc_range,
            dataByteLength: data_byte_length,
            fullByteLength: crc_end,
            dataMaxQuantity: quantity);
    }
    /// <summary>
    /// 创建一个拥有指定数量线圈的帧布局
    /// </summary>
    /// <param name="quantity">线圈数量</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IRtuWriteMultipleCoilsFrameLayout WriteMultipleCoils(ushort quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "数量必须大于0");
        }
        if (quantity > 2000)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "线圈数量不能超过2000");
        }

        // Data
        var data_byte_length = ModbusFrameLayout.GetColisToByteLength(quantity);
        const int data_start = 7;
        int data_end = data_start + data_byte_length;
        var data_range = new Range(data_start, data_end);

        // Content
        var content = new Range(0, data_end);

        // Crc
        var crc_start = data_end;
        var crc_end = data_end + 2;
        var crc_range = new Range(crc_start, crc_end);

        return new RtuWriteMultipleFrameLayout(
            dataRange: data_range,
            contentRange: content,
            crcRange: crc_range,
            dataByteLength: data_byte_length,
            fullByteLength: crc_end,
            dataMaxQuantity: quantity);
    }
    /// <summary>
    /// 写单个线圈帧布局
    /// </summary>
    public static IRtuWriteSingleCoilFrameLayout WriteSingleCoil() => RtuWriteSingleFrameLayout.Instance;
    /// <summary>
    /// 写单个寄存器帧布局
    /// </summary>
    public static IRtuWriteSingleRegisterFrameLayout WriteSingleRegister() => RtuWriteSingleFrameLayout.Instance;


    //    #region --错误包--

    //    public static bool TryUnpackError(
    //        ReadOnlySpan<byte> bytes,
    //        out ErrorFrame result)
    //    {
    //        const int PackageLength = 5;

    //        result = default;

    //        if (bytes.Length < PackageLength) return false;
    //        if (!ErrorFunctionCode.TryFromCode(bytes[1], out _)) return false;

    //        var slaveId = bytes[0];
    //        var error_function_code = bytes[1];
    //        var error_code = bytes[2];

    //        var content = bytes[..^2];
    //        var crc = (ushort)(bytes[^2] | bytes[^1] << 8);

    //        if (!CrcCalculator.Compute(content, crc)) return false;

    //#pragma warning disable CS0618 // 类型或成员已过时
    //        result = new ErrorFrame(slaveId, error_function_code, error_code);
    //#pragma warning restore CS0618 // 类型或成员已过时
    //        return true;
    //    }
    //    public static bool TryPackError(
    //        Span<byte> buffer,
    //        byte slaveId,
    //        ErrorFunctionCode errorFunctionCode,
    //        byte errorCode,
    //        out int written
    //        )
    //    {
    //        const int PackageLength = 5;
    //        written = 0;

    //        if (buffer.Length < PackageLength) return false;

    //        buffer[0] = slaveId;
    //        buffer[1] = errorFunctionCode;
    //        buffer[2] = errorCode;

    //        var crc = CrcCalculator.Compute(buffer[0..3]);
    //        buffer[3] = (byte)crc;
    //        buffer[4] = (byte)(crc >> 8);

    //        written = 5;
    //        return true;
    //    }
    //    public static bool TryPackError(
    //        Span<byte> buffer,
    //        ErrorFrame frame,
    //        out int written
    //        )
    //    {
    //        return TryPackError(buffer, frame.SlaveId, frame.FunctionCode, frame.ErrorCode, out written);
    //    }

    //    #endregion

    //    #region --读取包--

    //    public static bool TryUnpackRead(
    //        ReadOnlySpan<byte> bytes,
    //        out ReadFrame result)
    //    {
    //        const int PackageLength = 8;
    //        result = default;

    //        if (bytes.Length < PackageLength) return false;


    //        var slave_id = bytes[0];
    //        var function_code = bytes[1];
    //        var address = (ushort)(bytes[2] | (bytes[3] << 8));
    //        var quantity = (ushort)(bytes[4] | (bytes[5] << 8));

    //        var content = bytes[..6];
    //        var crc = (ushort)(bytes[6] << 8 | bytes[7]);

    //        if (CrcCalculator.Compute(content) != crc) return false;

    //#pragma warning disable CS0618 // 类型或成员已过时
    //        result = new ReadFrame(slave_id, function_code, address, quantity);
    //#pragma warning restore CS0618 // 类型或成员已过时
    //        return true;
    //    }
    //    public static bool TryPackRead(
    //        Span<byte> buffer,
    //        byte slaveId,
    //        FunctionCode functionCode,
    //        ushort address,
    //        ushort quantity,
    //        out int written
    //        )
    //    {
    //        const byte BitsPerByte = 8;
    //        const byte PackageLength = 8;

    //        const int ReadCoilsMaxQuantity = 2000;
    //        const int ReadDiscreteInputsMaxQuantity = 2000;
    //        const int ReadHoldingRegistersQuantity = 2000;
    //        const int ReadInputRegistersQuantity = 125;

    //        written = 0;
    //        // 包长度验证
    //        if (buffer.Length < PackageLength) return false;

    //        // 数量验证
    //        if (functionCode == FunctionCode.ReadCoils && quantity > ReadCoilsMaxQuantity) return false;
    //        if (functionCode == FunctionCode.ReadDiscreteInputs && quantity > ReadDiscreteInputsMaxQuantity) return false;
    //        if (functionCode == FunctionCode.ReadHoldingRegisters && quantity > ReadHoldingRegistersQuantity) return false;
    //        if (functionCode == FunctionCode.ReadInputRegisters && quantity > ReadInputRegistersQuantity) return false;


    //        buffer[0] = slaveId;
    //        buffer[1] = functionCode;

    //        buffer[2] = (byte)(address >> BitsPerByte);
    //        buffer[3] = (byte)address;

    //        buffer[4] = (byte)(quantity >> BitsPerByte);
    //        buffer[5] = (byte)quantity;

    //        // Crc验证
    //        var crc = CrcCalculator.Compute(buffer[..6]);
    //        buffer[6] = (byte)crc;
    //        buffer[7] = (byte)(crc >> 8);

    //        written = PackageLength;
    //        return true;
    //    }
    //    public static bool TryPackRead(
    //        Span<byte> buffer,
    //        ReadFrame frame,
    //        out int written
    //        )
    //    {
    //        return TryPackRead(
    //            buffer: buffer,
    //            slaveId: frame.SlaveId,
    //            functionCode: frame.FunctionCode,
    //            address: frame.Address,
    //            quantity: frame.Quantity,
    //            written: out written);
    //    }

    //    public static bool TryPackReadCoils(
    //        Span<bool> buffer,
    //        byte slaveId,
    //        ushort address,
    //        ushort quantity,
    //        out int written
    //        )
    //    {
    //        return TryPackRead(
    //            buffer: buffer,
    //            slaveId: slaveId,
    //            functionCode: FunctionCode.ReadCoils,
    //            address: address,
    //            quantity: quantity,
    //            written: out written);
    //    }
    //    public static bool TryPackReadDiscreteInputs(
    //        Span<byte> buffer,
    //        byte slaveId,
    //        ushort address,
    //        ushort quantity,
    //        out int written
    //        )
    //    {
    //        return TryPackRead(
    //           buffer: buffer,
    //           slaveId: slaveId,
    //           functionCode: FunctionCode.ReadDiscreteInputs,
    //           address: address,
    //           quantity: quantity,
    //           written: out written);
    //    }
    //    public static bool TryPackReadHoldingRegisters(
    //        Span<byte> buffer,
    //        byte slaveId,
    //        ushort address,
    //        ushort quantity,
    //        out int written
    //        )
    //    {
    //        return TryPackRead(
    //            buffer: buffer,
    //            slaveId: slaveId,
    //            functionCode: FunctionCode.ReadHoldingRegisters,
    //            address: address,
    //            quantity: quantity,
    //            written: out written);
    //    }
    //    public static bool TryPackReadInputRegisters(
    //        Span<byte> buffer,
    //        byte slaveId,
    //        ushort address,
    //        ushort quantity,
    //        out int written
    //        )
    //    {
    //        return TryPackRead(
    //           buffer: buffer,
    //           slaveId: slaveId,
    //           functionCode: FunctionCode.ReadInputRegisters,
    //           address: address,
    //           quantity: quantity,
    //           written: out written);
    //    }

    //    #endregion

    //    #region --写单值包--

    //    public static bool TryUnpackWriteSingleRegister(
    //        ReadOnlySpan<byte> bytes,
    //        out WriteSingleRegisterFrame result)
    //    {
    //        const int PackageLength = 8;
    //        result = default;

    //        // 校验包长度
    //        if (bytes.Length < PackageLength) return false;
    //        // 校验功能码
    //        if (bytes[1] != FunctionCode.WriteSingleRegister) return false;
    //        // 校验Crc
    //        var content = bytes[..6];
    //        var crc = (ushort)(bytes[6] << 8 | bytes[7]);
    //        if (CrcCalculator.Compute(content) != crc) return false;


    //        var slave_id = bytes[0];
    //        var address = (ushort)(bytes[2] | (bytes[3] << 8));
    //        var value = (ushort)(bytes[4] | (bytes[5] << 8));

    //        if (CrcCalculator.Compute(content) != crc) return false;

    //#pragma warning disable CS0618 // 类型或成员已过时
    //        result = new WriteSingleRegisterFrame(slave_id, address, value);
    //#pragma warning restore CS0618 // 类型或成员已过时
    //        return true;
    //    }
    //    public static bool TryPackWriteSingleRegister(
    //        Span<byte> buffer,
    //        byte slaveId,
    //        ushort address,
    //        ushort value,
    //        out int written
    //        )
    //    {
    //        const byte BitsPerByte = 8;
    //        const byte PackageLength = 8;

    //        written = 0;
    //        if (buffer.Length < PackageLength) return false;

    //        buffer[0] = slaveId;
    //        buffer[1] = FunctionCode.WriteSingleRegister;

    //        buffer[2] = (byte)(address >> BitsPerByte);
    //        buffer[3] = (byte)address;

    //        buffer[4] = (byte)(value >> BitsPerByte);
    //        buffer[5] = (byte)value;

    //        var crc = CrcCalculator.Compute(buffer[..6]);
    //        buffer[6] = (byte)crc;
    //        buffer[7] = (byte)(crc >> 8);

    //        written = PackageLength;
    //        return true;
    //    }
    //    public static bool TryPackWriteSingleRegister(
    //        Span<byte> buffer,
    //        WriteSingleRegisterFrame frame,
    //        out int written
    //        )
    //    {
    //        return TryPackWriteSingleRegister(
    //            buffer: buffer,
    //            slaveId: frame.SlaveId,
    //            address: frame.Address,
    //            value: frame.Value,
    //            written: out written);
    //    }


    //    public static bool TryUnpackWriteSingleCoil(
    //    ReadOnlySpan<byte> bytes,
    //    out WriteSingleCoilFrame result)
    //    {
    //        const int PackageLength = 8;
    //        result = default;

    //        // 校验包长度
    //        if (bytes.Length < PackageLength) return false;
    //        // 校验功能码
    //        if (bytes[1] != FunctionCode.WriteSingleCoil) return false;
    //        // 校验线圈值
    //        var coil_value = (ushort)(bytes[4] | bytes[5] << 8);
    //        if (coil_value is not (0xFF00 or 0x0000)) return false;
    //        // 校验Crc
    //        var content = bytes[..6];
    //        var crc = (ushort)(bytes[6] << 8 | bytes[7]);
    //        if (CrcCalculator.Compute(content) != crc) return false;


    //        var slave_id = bytes[0];
    //        var address = (ushort)(bytes[2] | (bytes[3] << 8));
    //        var value = coil_value is 0xFF00;

    //#pragma warning disable CS0618 // 类型或成员已过时
    //        result = new WriteSingleCoilFrame(slave_id, address, value);
    //#pragma warning restore CS0618 // 类型或成员已过时
    //        return true;
    //    }
    //    public static bool TryPackWriteSingleCoil(
    //        Span<byte> buffer,
    //        byte slaveId,
    //        ushort address,
    //        bool value,
    //        out int written
    //        )
    //    {
    //        const byte BitsPerByte = 8;
    //        const byte PackageLength = 8;

    //        written = 0;
    //        if (buffer.Length < PackageLength) return false;

    //        buffer[0] = slaveId;
    //        buffer[1] = FunctionCode.WriteSingleRegister;

    //        buffer[2] = (byte)(address >> BitsPerByte);
    //        buffer[3] = (byte)address;

    //        ushort uint_16_value = (ushort)(value ? 0xFF00 : 0x0000);
    //        buffer[4] = (byte)(uint_16_value >> BitsPerByte);
    //        buffer[5] = (byte)uint_16_value;

    //        var crc = CrcCalculator.Compute(buffer[..6]);
    //        buffer[6] = (byte)crc;
    //        buffer[7] = (byte)(crc >> 8);

    //        written = PackageLength;
    //        return true;
    //    }
    //    public static bool TryPackRead(
    //        Span<byte> buffer,
    //        WriteSingleCoilFrame frame,
    //        out int written
    //        )
    //    {
    //        return TryPackWriteSingleCoil(
    //            buffer: buffer,
    //            slaveId: frame.SlaveId,
    //            address: frame.Address,
    //            value: frame.Value,
    //            written: out written);
    //    }

    //    #endregion

    //    #region --写多值包--

    //    public static bool TryUnpackWriteMultipleRegisters(
    //        ReadOnlySpan<byte> raw,
    //        Span<ushort> body,
    //        out RequestHeader header)
    //    {
    //        var layout = WriteMultipleFrameLayouts.CreateByRegisterQuantity(body.Length);
    //        return layout.TryUnpackRegisters(raw, body, out header);
    //    }

    //    public static bool TryPackWriteMultipleRegisters(
    //        Span<byte> buffer,
    //        byte slaveId,
    //        ushort address,
    //        ReadOnlyMemory<ushort> values,
    //        out int written
    //        )
    //    {
    //        var layout = WriteMultipleFrameLayouts.CreateByRegisterQuantity(values.Length);
    //        layout.TryPackRegisters(buffer, slaveId, address, values, out written);
    //    }

    //    #endregion
}