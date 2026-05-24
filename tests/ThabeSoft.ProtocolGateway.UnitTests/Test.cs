using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Builders;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.Tags;

namespace ThabeSoft.ProtocolGateway;

internal static class Test
{
    public static async Task Process()
    {
        // 构建标签
        ITagBuilder<int> builder = null!;
        var tag = builder
            .Name("Test")
            .Address(new ModbusAddress() { Address = 1000 })
            //.Converter(DWordConverter.LittleEndian(ByteSwap.SwapByte | ByteSwap.SwapQWord))
            .Build();

        // 创建通讯通道
        var channel = new ModbusChannel();
        // 创建订阅器
        var subcriber = new Subcriber(channel);

        // 读取
        var result = await channel.ReadAsync(tag);
        // 写入
        await channel.WriteAsync(tag, result.Value);
        // 订阅
        subcriber.Subscribe(tag, x => Console.Write(x));
    }

    public enum ModbusAddressType
    {
        Coils,
        HodingRegister
    }

    public sealed class ModbusAddress : IAddress
    {
        public ushort Address { get; set; }
        public ModbusAddressType Type { get; set; }
    }

    public sealed class ModbusChannel : IReader, IWriter
    {
        public async ValueTask<Result<TValue>> ReadAsync<TValue>(ITag<TValue> tag, CancellationToken cancellationToken = default) where TValue : unmanaged
        {
            if (tag.Address is not ModbusAddress modbusAddress) return Result.Error<TValue>(ErrorType.InvalidOperation, "不持支的协议");

            // 读线圈
            if(modbusAddress.Type == ModbusAddressType.Coils)
            {
                Span<byte> value = stackalloc byte[1];
                return tag.Converter.From(value);
            }

            return Result.Error<TValue>(ErrorType.InvalidParameter, "无法识别的Modbus地址类型");
        }

        public async ValueTask<Result> WriteAsync<TValue>(ITag<TValue> tagInfo, TValue value, CancellationToken cancellationToken = default) where TValue : unmanaged
        {
            Span<byte> data = stackalloc byte[4];
            return tagInfo.Converter.To(value, data);
        }
    }

    public class Subcriber(IReader reader) : ISubscriber
    {
        public IDisposable Subscribe<TValue>(ITag<TValue> tag, Action<TValue> callback) where TValue : unmanaged
        {
            return Task.Run(async () =>
            {
                while(true)
                {
                    var result = await reader.ReadAsync(tag);
                    await Task.Delay(10);

                    if (result) callback(result.Value);
                }
            });
        }
    }
}