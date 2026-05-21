using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ThabeSoft.ProtocolGateway.Channels;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway;


public static class ChannelExtensions
{
    // 读基本数据类型的扩展方法
    extension(IReadChannel channel)
    {
        public async ValueTask<Result<T>> ReadAsync<T>(IReadRequest request, CancellationToken cancellationToken = default)
            where T : unmanaged
        {
            var length = Unsafe.SizeOf<T>();
            var buffer = ArrayPool<byte>.Shared.Rent(length);
            try
            {
                var destination = buffer.AsMemory(0, length);
                var status = await channel.ReadAsync(request, destination, cancellationToken);
                if (status != ErrorType.Success) return Result.Failure<T>(status);

                var value = MemoryMarshal.Cast<byte, T>(destination.Span)[0];
                return Result.Success(value);
            }
            catch (OperationCanceledException)
            {
                return Result.Timeout<T>();
            }
            catch (Exception ex)
            {
                Debug.Fail($"数据读取失败: {ex.Message}");
                return Result.InternalError<T>();
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        /*------------------- 8bit -------------------*/

        public ValueTask<Result<byte>> ReadByteAsync(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<byte>(request, cancellationToken);
        }

        /*------------------- 16bit -------------------*/
        public ValueTask<Result<ushort>> ReadUInt16Async(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<ushort>(request, cancellationToken);
        }
        public ValueTask<Result<short>> ReadInt16Async(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<short>(request, cancellationToken);
        }
        public ValueTask<Result<char>> ReadCharAsync(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<char>(request, cancellationToken);
        }

        /*------------------- 32bit -------------------*/
        public ValueTask<Result<uint>> ReadUInt32Async(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<uint>(request, cancellationToken);
        }
        public ValueTask<Result<int>> ReadInt32Async(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<int>(request, cancellationToken);
        }

        public ValueTask<Result<float>> ReadSingleAsync(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<float>(request, cancellationToken);
        }

        /*------------------- 64bit -------------------*/

        public ValueTask<Result<ulong>> ReadUInt64Async(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<ulong>(request, cancellationToken);
        }
        public ValueTask<Result<long>> ReadInt64Async(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<long>(request, cancellationToken);
        }

        public ValueTask<Result<double>> ReadDoubleAsync(IReadRequest request, CancellationToken cancellationToken = default)
        {
            return channel.ReadAsync<double>(request, cancellationToken);
        }
    }

    // 写基本数据类型的扩展方法
    extension(IWriteChannel channel)
    {
        public async ValueTask<ErrorType> WriteAsync<T>(IWriteRequest request, T value, CancellationToken cancellationToken = default)
            where T : unmanaged
        {
            var length = Unsafe.SizeOf<T>();
            var buffer = ArrayPool<byte>.Shared.Rent(length);
            try
            {
                var destination = buffer.AsMemory(0, length);
                MemoryMarshal.Write(destination.Span, ref value);

                return await channel.WriteAsync(request, destination, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return ErrorType.Timeout;
            }
            catch (Exception ex)
            {
                Debug.Fail($"数据写入失败: {ex.Message}");
                return ErrorType.InternalError;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        /*------------------- 8bit -------------------*/

        public ValueTask<ErrorType> WriteByteAsync(IWriteRequest request, byte value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }


        /*------------------- 16bit -------------------*/

        public ValueTask<ErrorType> WriteUInt16Async(IWriteRequest request, ushort value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }

        public ValueTask<ErrorType> WriteInt16Async(IWriteRequest request, short value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }

        public ValueTask<ErrorType> WriteCharAsync(IWriteRequest request, char value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }

        /*------------------- 32bit -------------------*/

        public ValueTask<ErrorType> WriteUInt32Async(IWriteRequest request, uint value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }

        public ValueTask<ErrorType> WriteInt32Async(IWriteRequest request, int value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }

        public ValueTask<ErrorType> WriteSingleAsync(IWriteRequest request, float value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }

        /*------------------- 64bit -------------------*/

        public ValueTask<ErrorType> WriteUInt64Async(IWriteRequest request, ulong value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }

        public ValueTask<ErrorType> WriteInt64Async(IWriteRequest request, long value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }

        public ValueTask<ErrorType> WriteDoubleAsync(IWriteRequest request, double value, CancellationToken cancellationToken = default)
        {
            return channel.WriteAsync(request, value, cancellationToken);
        }
    }

    // 订阅基本数据类型的扩展方法
    extension(ISubscribeChannel channel)
    {
        public ISubscription Subscribe<T>(ISubscribeSource source, Action<T> callback)
            where T : unmanaged
        {
            return channel.Subscribe(source, bytes =>
            {
                var value = MemoryMarshal.Cast<byte, T>(bytes.Span)[0];
                callback.Invoke(value);
            });
        }

        /*------------------- 8bit -------------------*/

        public ISubscription SubscribeByte(ISubscribeSource source, Action<byte> callback)
        {
            return channel.Subscribe(source, callback);
        }

        /*------------------- 16bit -------------------*/

        public ISubscription SubscribeUInt16(ISubscribeSource source, Action<ushort> callback)
        {
            return channel.Subscribe(source, callback);
        }

        public ISubscription SubscribeInt16(ISubscribeSource source, Action<short> callback)
        {
            return channel.Subscribe(source, callback);
        }

        public ISubscription SubscribeChar(ISubscribeSource source, Action<char> callback)
        {
            return channel.Subscribe(source, callback);
        }

        /*------------------- 32bit -------------------*/

        public ISubscription SubscribeUInt32(ISubscribeSource source, Action<uint> callback)
        {
            return channel.Subscribe(source, callback);
        }

        public ISubscription SubscribeInt32(ISubscribeSource source, Action<int> callback)
        {
            return channel.Subscribe(source, callback);
        }

        public ISubscription SubscribeSingle(ISubscribeSource source, Action<float> callback)
        {
            return channel.Subscribe(source, callback);
        }

        /*------------------- 64bit -------------------*/

        public ISubscription SubscribeUInt64(ISubscribeSource source, Action<ulong> callback)
        {
            return channel.Subscribe(source, callback);
        }

        public ISubscription SubscribeInt64(ISubscribeSource source, Action<long> callback)
        {
            return channel.Subscribe(source, callback);
        }

        public ISubscription SubscribeDouble(ISubscribeSource source, Action<double> callback)
        {
            return channel.Subscribe(source, callback);
        }
    }
}