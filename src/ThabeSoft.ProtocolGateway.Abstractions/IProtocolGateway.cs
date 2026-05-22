using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway;


public interface IProtocolGateway
{
    void AddDeviceContext<TDevice>(Action<IDeviceOptionsBuilders<TDevice>> optionActinos);

    IReader GetReader<TDevice>();
    IWriter<TDevice> GetWriter<TDevice>();

    ISubscriber GetSubscriber();
    ISubscriber<TDevice> GetSubscriber<TDevice>();
}





internal static class Test
{
    public static async Task Process()
    {
        // 构建标签
        ITagInfoBuilder<int> builder = null!;
        var tag = builder
            .Name("Test")
            .Address("40000")
            .Converter(DWordConverter.FromLittleEndian(ByteSwap.SwapByte | ByteSwap.SwapQWord))
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

    

    public sealed class ModbusChannel : IReader, IWriter
    {
        public ValueTask<Result<TValue>> ReadAsync<TValue>(ITag<TValue> tagInfo, CancellationToken cancellationToken = default) where TValue : unmanaged
        {
            tagInfo.
        }

        public ValueTask<Result> WriteAsync<TValue>(ITag<TValue> tagInfo, TValue value, CancellationToken cancellationToken = default) where TValue : unmanaged
        {
            throw new NotImplementedException();
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