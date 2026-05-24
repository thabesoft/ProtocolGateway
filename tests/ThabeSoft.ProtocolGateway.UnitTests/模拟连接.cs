using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus;
using ThabeSoft.ProtocolGateway.Primitives;
using ThabeSoft.ProtocolGateway.SerialPort;
using ThabeSoft.ProtocolGateway.SerialPort.Options;

namespace ThabeSoft.ProtocolGateway;


[TestClass]
public class 模拟连接
{
    public TestContext TestContext { get; set; }


    [TestMethod(DisplayName = "模拟全流程")]
    public async Task Connect()
    {
        SerialPortTransport transport = new();
        var options = SerialOptions.Create("COM2");

        var connect_result=  await transport.ConnectAsync(options, TestContext.CancellationToken);
        Assert.IsTrue(connect_result, connect_result.Message);

        ModbusRtuMaster master = new(transport);
        ModbusChannel channel = new(master);

        var read_tag = Tag.Bool(ModbusAddress.ReadCoils(01, 100));
        var write_tag = Tag.Bool(ModbusAddress.WriteSingleCoil(01, 100));

        var test_tag = Tag.Int64(ModbusAddress.ReadInputRegisters(01, 100), QWordLayout.BADCFEHG);
        var test_tag2 = Tag.Int32(ModbusAddress.ReadInputRegisters(01, 100), DWordLayout.CDAB);
        var test_tag3 = QWordLayout.FromBigEndian(ByteSwap.SwapDWord | ByteSwap.SwapByte)
            .Bind(x => Tag.Int64(ModbusAddress.ReadInputRegisters(01, 100), x));

        // 写
        var write_result = await channel.WriteAsync(write_tag, true, TestContext.CancellationToken);
        Assert.IsTrue(write_result, write_result.Message);

        // 读
        var read_result = await channel.ReadAsync(read_tag, TestContext.CancellationToken);
        Assert.IsTrue(read_result, read_result.Message);

        Console.WriteLine(read_result.Value);
    }
}
