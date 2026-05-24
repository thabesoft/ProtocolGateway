using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus;


/// <summary>
/// Modbus 主站
/// </summary>
public interface IModbusMaster
{
    /// <summary>
    /// 读取n个线圈值
    /// </summary>
    /// <param name="destination">缓冲区多大就是读取几个</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    ValueTask<Result> ReadCoilsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default);
    /// <summary>
    /// 读取n个离散输出值
    /// </summary>
    /// <param name="destination">缓冲区多大就是读取几个</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    ValueTask<Result> ReadDiscreteInputsAsync(Memory<bool> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default);
    /// <summary>
    /// 读取n个保持寄存器值
    /// </summary>
    /// <param name="destination">缓冲区多大就是读取几个</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    ValueTask<Result> ReadHoldingRegistersAsync(Memory<ushort> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default);
    /// <summary>
    /// 读取n个输入寄存器值
    /// </summary>
    /// <param name="destination">缓冲区多大就是读取几个</param>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    ValueTask<Result> ReadInputRegistersAsync(Memory<ushort> destination, byte slaveId, ushort address, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置一个线圈值
    /// </summary>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="value">开(true) 或 关(false)</param>
    ValueTask<Result> WriteSingleCoilsAsync(byte slaveId, ushort address, bool value, CancellationToken cancellationToken = default);
    /// <summary>
    /// 设置一个寄存器值
    /// </summary>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="value">数值</param>
    ValueTask<Result> WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value, CancellationToken cancellationToken = default);
    /// <summary>
    /// 写入n个线圈值
    /// </summary>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="values">写入全部元素</param>
    ValueTask<Result> WriteMultipleCoilsAsync(byte slaveId, ushort address, ReadOnlyMemory<bool> values, CancellationToken cancellationToken = default);
    /// <summary>
    /// 写入n个寄存器值
    /// </summary>
    /// <param name="slaveId">从站Id</param>
    /// <param name="address">起始地址</param>
    /// <param name="values">写入全部元素</param>
    ValueTask<Result> WriteMultipleRegistersAsync(byte slaveId, ushort address, ReadOnlyMemory<ushort> values, CancellationToken cancellationToken = default);
}