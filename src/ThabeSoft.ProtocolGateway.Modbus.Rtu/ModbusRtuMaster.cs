using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Transports;

namespace ThabeSoft.ProtocolGateway.Modbus;


/// <summary>
/// Modbus Rtu 主站
/// </summary>
public  sealed class ModbusRtuMaster(ITransport transport) : IModbusMaster
{
    public ValueTask<Result<int>> ReadColisAsync(Memory<bool> destination, byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<int>> ReadDiscreteInputsAsync(Memory<bool> destination, byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<int>> ReadHoldingRegistersAsync(Memory<byte> destination, byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<int>> ReadInputRegistersAsync(Memory<byte> destination, byte slaveId, ushort address, ushort quantity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> WriteMultipleCoilsAsync(byte slaveId, ushort address, ReadOnlyMemory<bool> values, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> WriteMultipleRegistersAsync(byte slaveId, ushort address, ReadOnlyMemory<ushort> values, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> WriteSingleCoilsAsync(byte slaveId, ushort address, bool value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
