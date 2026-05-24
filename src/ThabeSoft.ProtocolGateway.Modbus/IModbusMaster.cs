using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Modbus;

/// <summary>
/// Modbus 主站
/// </summary>
public interface IModbusMaster
{
    ValueTask<Result<int>> ReadColisAsync(Memory<bool> destination, byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken = default);
    ValueTask<Result<int>> ReadDiscreteInputsAsync(Memory<bool> destination, byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken = default);
    ValueTask<Result<int>> ReadHoldingRegistersAsync(Memory<byte> destination, byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken = default);
    ValueTask<Result<int>> ReadInputRegistersAsync(Memory<byte> destination, byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken = default);


    ValueTask<Result> WriteMultipleCoilsAsync(byte slaveId, ushort address, ReadOnlyMemory<bool> values, CancellationToken cancellationToken = default);
    ValueTask<Result> WriteMultipleRegistersAsync(byte slaveId, ushort address, ReadOnlyMemory<ushort> values, CancellationToken cancellationToken = default);
    ValueTask<Result> WriteSingleCoilsAsync(byte slaveId, ushort address, bool value, CancellationToken cancellationToken = default);
    ValueTask<Result> WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value, CancellationToken cancellationToken = default);
}