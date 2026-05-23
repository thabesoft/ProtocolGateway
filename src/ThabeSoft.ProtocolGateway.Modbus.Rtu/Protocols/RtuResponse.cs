using System.Diagnostics.CodeAnalysis;
using ThabeSoft.IndustrialHub.Modbus.Crc;
using ThabeSoft.IndustrialHub.Modbus.Exceptions;
using ThabeSoft.ProtocolGateway.Modbus.Crc;

namespace ThabeSoft.ProtocolGateway.Protocols;


/// <summary>
/// Rtu 响应
/// </summary>
public readonly struct RtuResponse
{
    private readonly byte[] _bytes;

    public readonly byte SlaveId => _bytes[0];
    public readonly byte FunctionCode => _bytes[1];
    public readonly byte DataLength => _bytes[2];
    public readonly ReadOnlySpan<byte> Data => _bytes.AsSpan(3, DataLength);
    public readonly ushort Crc => (ushort)(_bytes[DataLength + 3] | (_bytes[DataLength + 4] << 8));


    private RtuResponse(byte[] responseBytes)
    {
        _bytes = responseBytes;
    }
    public static RtuResponse Create(byte[] responseBytes)
    {
        if (responseBytes.Length < 2)
        {
            throw new InvalidDataException("响应长度不足，至少需要3字节");
        }

        // 功能码
        var function_code = responseBytes[1];
        // 错误
        if(IsErrorFunctionCode(function_code)) ErrorData(responseBytes);

        var data_length = responseBytes[2];   // 数据长度
        var content_length = data_length + 3; // 内容长度: SalveId(1) + FuncCode(1) + DataLength(1)
        var total_length = content_length + 2;// 总长度 : 内容长度 + Crc(2)

        if (responseBytes.Length < total_length)
        {
            throw new InvalidDataException($"响应长度不足，[{responseBytes.Length} / {total_length}]");
        }

        var content = responseBytes.AsSpan(0, content_length);
        var crc = (ushort)(responseBytes[total_length - 2] | (responseBytes[total_length - 1] << 8));

        if (CrcCalculator.Calculate(content) != crc)
        {
            throw new CrcException("CRC校验失败");
        }

        return new RtuResponse(responseBytes);
    }

    /// <summary>
    /// 错误数据
    /// </summary>
    /// <exception cref="InvalidDataException"></exception>
    /// <exception cref="ModbusException"></exception>
    [DoesNotReturn]
    public static void ErrorData(byte[] bytes)
    {
        if (bytes.Length < 5)
        {
            throw new InvalidDataException("响应长度不足，至少需要5字节");
        }

        const int content_length = 3; // SlaveId(1) + FunctionCode(1) + ErrorCode(1)

        var slave_id = bytes[0];
        var function_code = bytes[1];
        var error_code = bytes[2];

        var data = bytes.AsSpan(0, content_length);
        var crc = (ushort)(bytes[3] | (bytes[4] << 8));

        if (CrcCalculator.Calculate(data) == crc)
        {
            throw new InvalidDataException("CRC校验失败");
        }

        throw new ModbusException();
    }


    public static bool IsErrorFunctionCode(byte functionCode)
    {
        return (functionCode & 0x80) != 0;
    }
}

public static class RtuResponseExtensions
{
    extension(byte[] bytes)
    {
        public RtuResponse AsRtuResponse()
        {
            return RtuResponse.Create(bytes);
        }
    }
}