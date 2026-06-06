using System.Buffers;
using System.ComponentModel;
using ThabeSoft.Lifecycle;
using ThabeSoft.Modbus;
using ThabeSoft.Primitives;
using ThabeSoft.Primitives.Binary;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// Modbus 通道
/// </summary>
/// <param name="master">Modbus主站</param>
public sealed class ModbusChannel(IModbusMaster master) : IReadWriteChannel
{

    public event PropertyChangedEventHandler? PropertyChanged { add => master.PropertyChanged += value; remove => master.PropertyChanged -= value; }
    public LifecycleState State => master.State;
    public ValueTask<Result> StartAsync(CancellationToken cancellationToken = default) => master.StartAsync(cancellationToken);
    public ValueTask<Result> StopAsync(CancellationToken cancellationToken = default) => master.StopAsync(cancellationToken);
    public ValueTask DisposeAsync() => master.DisposeAsync();



    /// <summary>
    /// 读取
    /// </summary>
    public async ValueTask<Result<TValue>> ReadAsync<TValue>(ITag<TValue> tag, CancellationToken cancellationToken = default)
        where TValue : unmanaged
    {
        if (tag.Address is not ModbusAddress address) return Result.Error<TValue>( "无效地址");
        if (!address.FunctionCode.IsRead) return Result.Error<TValue>( "不是有效的 Modbus 读值地址");

        var byte_count = address.FunctionCode.IsReadCoils ? 1 : tag.DataLength;
        var buffer = ArrayPool<byte>.Shared.Rent(byte_count);

        try
        {
            var data_mem = buffer.AsMemory(0, byte_count);

            // 读线圈
            if (address.FunctionCode == FunctionCode.ReadCoils)
            {
                await ReadCoilsToByteAsync(data_mem, address.SlaveId, address.Start, master.ReadCoilsAsync, ct: cancellationToken);
                return tag.Serializer.Deserialize(data_mem.Span);
            }
            // 读离散输入
            if (address.FunctionCode == FunctionCode.ReadDiscreteInputs)
            {
                await ReadCoilsToByteAsync(data_mem, address.SlaveId, address.Start, master.ReadDiscreteInputsAsync, ct: cancellationToken);
                return tag.Serializer.Deserialize(data_mem.Span);
            }

            // 读保持寄存器
            if (address.FunctionCode == FunctionCode.ReadHoldingRegisters)
            {
                await ReadRegistersToByteAsync(data_mem, address.SlaveId, address.Start, master.ReadHoldingRegistersAsync, ct: cancellationToken);
                return tag.Serializer.Deserialize(data_mem.Span);
            }
            // 读输入寄存器
            if (address.FunctionCode == FunctionCode.ReadInputRegisters)
            {
                await ReadRegistersToByteAsync(data_mem, address.SlaveId, address.Start, master.ReadInputRegistersAsync, ct: cancellationToken);
                return tag.Serializer.Deserialize(data_mem.Span);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }

        return Result.Error<TValue>("Modbus 无法识别的读取操作");
    }
    /// <summary>
    /// 写入
    /// </summary>
    public async ValueTask<Result> WriteAsync<TValue>(ITag<TValue> tag, TValue value, CancellationToken cancellationToken = default) where TValue : unmanaged
    {
        if (tag.Address is not ModbusAddress address) return Result.Error("无效地址");
        if (!address.FunctionCode.IsWrite) return Result.Error<TValue>( "不是有效的 Modbus 写值地址");


        if (address.FunctionCode == FunctionCode.WriteMultipleCoils)
        {
            bool[] buffer = new bool[1];
            //await master.WriteMultipleCoilsAsync(address.SlaveId, address.Start,  cancellationToken);
            Span<byte> c = stackalloc byte[1];

            return tag.Serializer.Deserialize(c);
        }

        if (address.FunctionCode == FunctionCode.WriteMultipleRegisters)
        {
            return Result.Success();
        }

        if (address.FunctionCode == FunctionCode.WriteSingleCoil)
        {
            //await master.ReadInputRegistersAsync(buffer, address.SlaveId, address.Start, 1, cancellationToken);
            Span<byte> c = stackalloc byte[1];
            return tag.Serializer.Deserialize(c);
        }

        if (address.FunctionCode == FunctionCode.WriteSingleRegister)
        {
            return Result.Success();
        }


        return Result.Error<TValue>("Modbus 无法识别的写取操作");
    }


    /// <summary>
    /// 读取n个字节的线圈, 1个字节=8个线圈
    /// </summary>
    /// <param name="destination">需要多少个字节的线圈</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="readHandler">读取请求委托</param>
    /// <param name="bitOrder">位序</param>
    private static async ValueTask<Result> ReadCoilsToByteAsync(Memory<byte> destination, byte slaveId, ushort address, ReadCoilsHandler readHandler, BitOrder bitOrder = BitOrder.LSB0, CancellationToken ct = default)
    {
        var bits_count = destination.Length * 8;
        var bits_buffer = ArrayPool<bool>.Shared.Rent(bits_count);

        try
        {
            // 读取所有bit
            var bits_mem = bits_buffer.AsMemory(0, bits_count);
            var reaer_result = await readHandler(bits_mem, slaveId, address, ct);
            if (!reaer_result.IsSuccess) return reaer_result;

            // 将每8个bit解析为一个byte
            return bits_mem.Span.ToBytes(destination.Span, bitOrder);
        }
        finally
        {
            ArrayPool<bool>.Shared.Return(bits_buffer);
        }
    }

    /// <summary>
    /// 读取n个字节的寄存器, 2个字节=1个寄存器
    /// </summary>
    /// <param name="destination">需要多少个字节的寄存器</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="readHandler">读取请求委托</param>
    /// <param name="endianness">端序</param>
    private static async ValueTask<Result> ReadRegistersToByteAsync(Memory<byte> destination, byte slaveId, ushort address, ReadRegistersHandler readHandler, Endianness endianness = Endianness.BigEndian, CancellationToken ct = default)
    {
        var byte_count = destination.Length;
        if (byte_count % 2 != 0) return Result.Error($"读取寄存器失败,需要偶数字节,实际:{byte_count}");

        var word_count = destination.Length / 2;
        var word_buffer = ArrayPool<ushort>.Shared.Rent(word_count);

        try
        {
            // 读取所有word
            var word_mem = word_buffer.AsMemory(0, word_count);
            var reaer_result = await readHandler(word_mem, slaveId, address, ct);
            if (!reaer_result.IsSuccess) return reaer_result;

            // 将每个word解析为2个byte
            return word_mem.Span.ToBytes(destination.Span, endianness);
        }
        finally
        {
            ArrayPool<ushort>.Shared.Return(word_buffer);
        }
    }

    



    /// <summary>
    /// 读取线圈委托
    /// </summary>
    /// <param name="destination">需要多少个线圈</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    private delegate ValueTask<Result> ReadCoilsHandler(Memory<bool> destination, byte slaveId, ushort address, CancellationToken ct);

    /// <summary>
    /// 读取寄存器委托
    /// </summary>
    /// <param name="destination">需要多少个寄存器值</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    private delegate ValueTask<Result> ReadRegistersHandler(Memory<ushort> destination, byte slaveId, ushort address, CancellationToken ct);
}