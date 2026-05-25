using ThabeSoft.Modbus;
using ThabeSoft.Ports.Options;
using ThabeSoft.ProtocolGateway.Modbus;
using ThabeSoft.ProtocolGateway.SerialPort;

namespace ThabeSoft.ProtocolGateway.ProtocolGateway;


[TestClass]
public class 模拟连接
{
    public TestContext TestContext { get; set; }


    private IReader _reader = default!;
    private IWriter _writer = default!;


    [TestInitialize]
    public async Task Setup()
    {
        SerialPortTransport transport = new();
        var options = SerialOptions.Create("COM2");

        var connect_result = await transport.ConnectAsync(options, TestContext.CancellationToken);
        Assert.IsTrue(connect_result, connect_result.Message);

        ModbusRtuMaster master = new(transport);
        ModbusChannel channel = new(master);

        _reader = channel;
        _writer = channel;
    }


    [DataRow((byte)0x01, (ushort)0, DisplayName = "从站0x01,地址0")]
    [DataRow((byte)0x03, (ushort)100, DisplayName = "从站0x03,地址100")]
    [TestMethod(DisplayName = "读取线圈")]
    public async Task ReadCoils(byte slaveId, ushort address)
    {
        var read_tag = Tag.Bool(ModbusAddress.ReadCoils(slaveId, address));
        var read_result = await _reader.ReadAsync(read_tag, TestContext.CancellationToken);

        Assert.IsTrue(read_result, read_result.Message);
        Console.WriteLine(read_result.Value);
    }

    [DataRow((byte)0x01, (ushort)0, DisplayName = "从站0x01,地址0")]
    [DataRow((byte)0x03, (ushort)100, DisplayName = "从站0x03,地址100")]
    [TestMethod(DisplayName = "读保持寄存器")]
    public async Task ReadHoldingRegisters(byte slaveId, ushort address)
    {
        var read_tag = Tag.UInt16(ModbusAddress.ReadHoldingRegisters(slaveId, address));
        var read_result = await _reader.ReadAsync(read_tag, TestContext.CancellationToken);

        Assert.IsTrue(read_result, read_result.Message);
        Console.WriteLine(read_result.Value);
    }
}
