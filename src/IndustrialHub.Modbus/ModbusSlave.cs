using IndustrialHub.Modbus.Protocol;
using System.IO.Ports;

namespace IndustrialHub.Modbus;

/// <summary>
/// Modbus 从站
/// </summary>
public sealed class ModbusSlave(SerialPort port)
{
    private readonly bool[] _coils = new bool[2000];
    private readonly bool[] _discreteInputs = new bool[2000];
    private readonly ushort[] _holdingRegisters = new ushort[2000];
    private readonly ushort[] _inputRegisters = new ushort[100];

    private readonly byte[] _receiveBuffer = new byte[512];

    public async Task Fuck()
    {
        var length = await ReadFullFrame();
        var span = _receiveBuffer.AsSpan(0, length);


        if (function_code == FunctionCode.ReadCoils)
        {
            var head = _receiveBuffer.AsMemory().Slice(5, 4);
            await port.BaseStream.ReadExactAsync(head);
        }
    }

    private async Task<int> ReadFullFrame()
    {
        // SlaveId(1) + FunctionCode(1) + Address(2)
        var head = _receiveBuffer.AsMemory()[..4];
        await port.BaseStream.ReadExactAsync(head);

        var slave_id = head.Span[0];
        var function_code = (FunctionCode)head.Span[1];
        var address = head.Span[2] << 8 | head.Span[3];

        if(function_code.IsRead || function_code == FunctionCode.WriteSingleCoil || function_code == FunctionCode.WriteSingleRegister)
        {
            // value(2) + crc(2)
            var content = _receiveBuffer.AsMemory().Slice(5, 4);
            await port.BaseStream.ReadExactAsync(content);
            
            return 8;
        }

        // 数量(2) + 字节数(1)
        var byte_count_mem = _receiveBuffer.AsMemory().Slice(5, 3);
        await port.BaseStream.ReadExactAsync(byte_count_mem);
        
        var quantity = head.Span[4] << 8 | head.Span[5];
        var byte_count = head.Span[6];

        // 内容(n) + Crc(2)
        var content_mem = _receiveBuffer.AsMemory().Slice(7, byte_count + 2);
        await port.BaseStream.ReadExactAsync(content_mem);

        return 7 + byte_count + 2;
    }


    private void ReadCoils(Memory<bool> buffer, ushort address, ushort quantity)
    {
        var internal_address = GetInternalColisAddress(address);
        if (internal_address + quantity > _coils.Length) throw new ArgumentException("超出线圈范围");

        _coils.AsMemory(internal_address, quantity).CopyTo(buffer);
    }
    private void ReadDiscreteInputs(Memory<bool> buffer, ushort address, ushort quantity)
    {
        var internal_address = GetInternalDiscreteInputsAddress(address);
        if (internal_address + quantity > _coils.Length) throw new ArgumentException("超出离散输入范围");

        _discreteInputs.AsMemory(internal_address, quantity).CopyTo(buffer);
    }
    private void ReadHoldingRegisters(Memory<ushort> buffer, ushort address, ushort quantity)
    {
        var internal_address = GetInternalHoldingRegistersAddress(address);
        if (internal_address + quantity > _holdingRegisters.Length) throw new ArgumentException("超出保持寄存器范围");

        _holdingRegisters.AsMemory(internal_address, quantity).CopyTo(buffer);
    }
    private void ReadInputRegisters(Memory<ushort> buffer, ushort address, ushort quantity)
    {
        var internal_address = GetInternalInputRegistersAddress(address);
        if (internal_address + quantity > _holdingRegisters.Length) throw new ArgumentException("超出保持寄存器范围");

        _holdingRegisters.AsMemory(internal_address, quantity).CopyTo(buffer);
    }


    private void WriteSingleCoils(ushort address, bool value)
    {
        var internal_address = GetInternalDiscreteInputsAddress(address);
        if (internal_address > _coils.Length) throw new ArgumentException("超出线圈范围");

        _coils[internal_address] = value;
    }
    private void WriteMultipleCoils(ushort address, ReadOnlySpan<bool> value)
    {
        var internal_address = GetInternalColisAddress(address);
        if (internal_address + value.Length > _coils.Length) throw new ArgumentException("超出线圈范围");

        var span = _coils.AsSpan().Slice(internal_address, value.Length);
        value.CopyTo(span);
    }
    private void WriteHoldingRegister(ushort address, ushort value)
    {
        var internal_address = GetInternalHoldingRegistersAddress(address);
        if (internal_address > _coils.Length) throw new ArgumentException("超出保持寄存器范围");

        _holdingRegisters[internal_address] = value;
    }
    private void WriteMultipleRegisters(ushort address, ReadOnlySpan<ushort> value)
    {
        var internal_address = GetInternalHoldingRegistersAddress(address);
        if (internal_address + value.Length > _coils.Length) throw new ArgumentException("超出输入寄存器范围");

        var span = _inputRegisters.AsSpan().Slice(internal_address, value.Length);
        value.CopyTo(span);
    }


    private int GetInternalColisAddress(ushort address) => address - 40001;
    private int GetInternalDiscreteInputsAddress(ushort address) => address - 40001;
    private int GetInternalHoldingRegistersAddress(ushort address) => address - 50001;
    private int GetInternalInputRegistersAddress(ushort address) => address - 50001;
}
