using Moq;
using ThabeSoft.Modbus;
using ThabeSoft.Modbus.Encoding;
using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Layouts;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Modbus;

namespace ThabeSoft.ProtocolGateway.ProtocolGateway;


[TestClass]
public class ModbusChannelTests
{
    public TestContext TestContext { get; set; }



    [DataRow(true)]
    [DataRow(false)]
    [TestMethod(DisplayName = "读线圈")]
    public async Task ReadCoils(bool value)
    {
        const byte slave_id = 1;
        const ushort address = 1;
        const int byte_count = 1;

        // 响应布局
        var layout_result = RtuReadResponseLayout.FromCoilQuantity(byte_count);
        Assert.IsTrue(layout_result, layout_result.Message);
        // 响应头
        var header = ReadResponseHeader.Coils(slave_id, byte_count);
        // 响应构建
        var read_index = 0;
        Memory<byte> resp_buffer = new byte[layout_result.Value.TotalLength];
        Span<bool> values = [value];
        var encode_result = RtuSlaveReadCodec.EncodeCoilsResponse(resp_buffer.Span, header, values, layout_result.Value);
        Assert.IsTrue(layout_result, layout_result.Message);

        // 响应模拟
        var mock_port = new Mock<IPort>();
        mock_port
            .Setup(x => x.ReadExactAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns<Memory<byte>, CancellationToken>((buffer, _) =>
            {
                var data = resp_buffer.Slice(read_index, buffer.Length);
                data.CopyTo(buffer);

                read_index += buffer.Length;
                var result = Result.Ok(buffer.Length);
                return new ValueTask<Result<int>>(result);
            });
        mock_port.Setup(x => x.WriteAsync(It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult(Result.Success));

        // 通道模拟
        ModbusRtuMaster master = new(mock_port.Object);
        ModbusChannel channel = new(master);

        // 发送数据
        var read_tag = Tag.Bool(ModbusAddress.ReadCoils(slave_id, address));
        var read_result = await channel.ReadAsync(read_tag, TestContext.CancellationToken);

        // 验证
        Assert.IsTrue(read_result, read_result.Message);
        Console.WriteLine(read_result.Value);
    }


    [DataRow((ushort)0x00FF)]
    [DataRow((ushort)0XFF00)]
    [DataRow((ushort)0xFFFF)]
    [TestMethod(DisplayName = "读保持寄存器")]
    public async Task ReadHoldingRegisters(ushort value) 
    {
        const byte slave_id = 1;
        const ushort address = 1;
        const int word_count = 1;

        // 响应布局
        var layout_result = RtuReadResponseLayout.FromRegisterQuantity(word_count);
        Assert.IsTrue(layout_result, layout_result.Message);
        // 响应头
        var header = ReadResponseHeader.HoldingRegisters(slave_id, word_count);
        // 响应构建
        var read_index = 0;
        Memory<byte> resp_buffer = new byte[layout_result.Value.TotalLength];
        Span<ushort> values = [value];
        var encode_result = RtuSlaveReadCodec.EncodeRegistersResponse(resp_buffer.Span, header, values, layout_result.Value);
        Assert.IsTrue(layout_result, layout_result.Message);

        // 响应模拟
        var mock_port = new Mock<IPort>();
        mock_port
            .Setup(x => x.ReadExactAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns<Memory<byte>, CancellationToken>((buffer, _) =>
            {
                var data = resp_buffer.Slice(read_index, buffer.Length);
                data.CopyTo(buffer);

                read_index += buffer.Length;
                var result = Result.Ok(buffer.Length);
                return new ValueTask<Result<int>>(result);
            });
        mock_port.Setup(x => x.WriteAsync(It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult(Result.Success));

        // 通道模拟
        ModbusRtuMaster master = new(mock_port.Object);
        ModbusChannel channel = new(master);

        // 发送数据
        var read_tag = Tag.UInt16(ModbusAddress.ReadHoldingRegisters(slave_id, address));
        var read_result = await channel.ReadAsync(read_tag, TestContext.CancellationToken);

        // 验证
        Assert.IsTrue(read_result, read_result.Message);
        Console.WriteLine(read_result.Value);
    }
}
