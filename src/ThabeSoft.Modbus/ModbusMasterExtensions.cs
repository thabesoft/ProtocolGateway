using System.Buffers;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus;


/// <summary>
/// Modbus 主站扩展
/// </summary>
public static class ModbusMasterExtensions
{
    extension(IModbusMaster master)
    {
        public async ValueTask<Result<ushort>> ReadHoldingRegistersWordAsync(byte slaveId, ushort address, CancellationToken cancellationToken = default)
        {
            var buffer = ArrayPool<ushort>.Shared.Rent(1);
            try
            {
                var mem = buffer.AsMemory(0, 1);
                var result = await master.ReadHoldingRegistersAsync(mem, slaveId, address, cancellationToken);
                if (!result.IsSuccess) return result.PropagateError<ushort>();

                return Result.Ok(mem.Span[0]);
            }
            finally
            {
                ArrayPool<ushort>.Shared.Return(buffer);
            }
        }

        public async ValueTask<Result<uint>> ReadHoldingRegistersDWordAsync(byte slaveId, ushort address, CancellationToken cancellationToken= default)
        {
            var buffer = ArrayPool<ushort>.Shared.Rent(2);
            try
            {
                var mem = buffer.AsMemory(0, 2);
                var result = await master.ReadHoldingRegistersAsync(mem, slaveId, address, cancellationToken);
                if (!result.IsSuccess) return result.PropagateError<uint>();

                return mem.Span.ToDWord();
            }
            finally
            {
                ArrayPool<ushort>.Shared.Return(buffer);
            }
        }

        //public async ValueTask<Result<ulong>> ReadHoldingRegistersQWordAsync(byte slaveId, ushort address, CancellationToken cancellationToken = default)
        //{
        //    var buffer = ArrayPool<ushort>.Shared.Rent(4);
        //    try
        //    {
        //        var mem = buffer.AsMemory(0, 4);
        //        var result = await master.ReadHoldingRegistersAsync(mem, slaveId, address, cancellationToken);
        //        if (!result.IsSuccess) return result.PropagateError<ulong>();

                
        //        return mem.Span.ToQWord(DWordLayout.BigEndian);
        //    }
        //    finally
        //    {
        //        ArrayPool<ushort>.Shared.Return(buffer);
        //    }
        //}
    }
}
