using ThabeSoft.Modbus;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway;

namespace ThabeSoft.Benchmark;

internal static class ModbusTest
{
    public static async Task Fuck(CancellationToken cancellationToken = default)
    {
        // 端口
        SerialPortTransport transport = new();
        await transport.ConnectAsync(SerialPortOptions.Create("Com2"), cancellationToken);

        // 网关
         Gateway gateway = new();
        var channel_name = ChannelName.Create("Pcl1").Value;

        // 通道
        ModbusRtuMaster master = new(transport);
        gateway.AddModbusChannel(channel_name, master);

        // 轮询
        var tag = ModbusTagFactory.ReadHoldingRegisterDWord(channel_name, 1, 100);
        gateway.Poll(tag)
            .Subscribe(result =>
            {
                if (result.IsSuccess)
                {
                    Console.WriteLine(result.Value);
                }
                else
                {
                    Console.WriteLine(result.Message);
                }

            },
            ex => Console.WriteLine(ex.Message),
            cancellationToken);

        gateway.Poll(tag)
            .Subscribe(result =>
            {
                if (result.IsSuccess)
                {
                    Console.WriteLine(result.Value);
                }
                else
                {
                    Console.WriteLine(result.Message);
                }

            },
            ex => Console.WriteLine(ex.Message),
            cancellationToken);


        // 等待
        await Task.Run(() =>
        {
            while (Console.ReadLine() != "exit") { }

        }, cancellationToken);
    }
}