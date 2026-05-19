using IndustrialHub.Modbus;
using IndustrialHub.Modbus.Options;
using IndustrialHub.Modbus.Transporters;

namespace IndustrialHub.UnitTests;


[TestClass]
public sealed class ModbusTests
{
    public TestContext TestContext { get; set; }


    [TestMethod(DisplayName = "并发连接")]
    public async Task ConnectAsync()
    {
        await using var transporter = new SerialPortTransporter();
        var options = ModbusSerialOptions.Create("COM2")
            .SetBaudRate(9600);

        var tasks = Enumerable.Range(0, 1000).Select(_ => transporter.ConnectAsync(options, TestContext.CancellationToken).AsTask());

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await Task.WhenAll(tasks));
    }

    [TestMethod(DisplayName = "未连接断开")]
    public async Task DisconnectAsync()
    {
        await using var transporter = new SerialPortTransporter();
        await Assert.ThrowsAsync<InvalidOperationException>(() => transporter.DisconnectAsync(TestContext.CancellationToken).AsTask());
    }

    [TestMethod(DisplayName = "未连接释放")]
    public async Task DisposeAsync()
    {
        await using var transporter = new SerialPortTransporter();
        await transporter.DisposeAsync();
    }

    [TestMethod(DisplayName = "读取数据")]
    public async Task ReadExactAsync()
    {
        await using var transporter = new SerialPortTransporter();
        var options = ModbusSerialOptions.Create("COM2")
            .SetBaudRate(9600);

        await transporter.ConnectAsync(options, TestContext.CancellationToken);

        byte[] buffer = new byte[20];
        await transporter.ReadExactAsync(buffer, new CancellationTokenSource(1000).Token);

        await transporter.DisconnectAsync(TestContext.CancellationToken);
    }

    [TestMethod(DisplayName = "读取数据")]
    public async Task ReadAAAAsync()
    {
        await using var transporter = new SerialPortTransporter();
        var options = ModbusSerialOptions.Create("COM2")
            .SetBaudRate(9600);
        ModbusMaster master = new(transporter);

        var colis = new bool[10];
        await master.ReadColisAsync(colis, 0x01, 40002, TestContext.CancellationToken);
    }
}
