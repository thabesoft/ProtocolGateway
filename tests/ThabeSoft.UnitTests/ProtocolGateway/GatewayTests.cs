using Moq;
using ThabeSoft.Modbus;
using ThabeSoft.Modbus.Encoding;
using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Layouts;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


[TestClass]
public class GatewayTests
{
    public TestContext TestContext { get; set; }



    [DataRow((byte)1, (ushort)100, true)]
    [DataRow((byte)1, (ushort)200, false)]
    [TestMethod(DisplayName = "读Modbus线圈")]
    public async Task ReadModbusCoils(byte slaveId, ushort address, bool value)
    {
        // 数据
        var mock_port = MockPort(slaveId, value);
        ModbusRtuMaster master = new(mock_port.Object);
        ModbusChannel channel = new(master);

        // 发送数据
        var read_tag = TagFactory.Bool(ModbusAddress.ReadCoils(slaveId, address));
        var read_result = await channel.ReadAsync(read_tag, TestContext.CancellationToken);

        // 验证
        Assert.IsTrue(read_result.IsSuccess, read_result.Message);
        Assert.AreEqual(value, read_result.Value);

        // 读线圈端口模拟
        static Mock<ITransport> MockPort(byte slaveId, params bool[] values)
        {
            // 值数量
            var quantity_result = ReadCoilsQuantity.Create(values.Length);
            Assert.IsTrue(quantity_result.IsSuccess, quantity_result.Message);

            // 响应布局
            var layout = RtuReadResponseLayout.FromQuantity(quantity_result.Value);
            // 响应头
            var header = ReadResponseHeader.Coils(slaveId, quantity_result.Value.ByteLength);
            // 响应构建
            var read_index = 0;
            Memory<byte> resp_buffer = new byte[quantity_result.Value.ByteLength];
            var encode_result = RtuSlaveReadCodec.EncodeCoilsResponse(resp_buffer.Span, header, values, layout);
            Assert.IsTrue(encode_result.IsSuccess, encode_result.Message);

            // 响应模拟
            var mock_port = new Mock<ITransport>();
            mock_port
                .Setup(x => x.ReadExactAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
                .Returns<Memory<byte>, CancellationToken>((buffer, _) =>
                {
                    var data = resp_buffer.Slice(read_index, buffer.Length);
                    data.CopyTo(buffer);

                    read_index += buffer.Length;
                    return new ValueTask<Result>(Result.Success());
                });
            mock_port.Setup(x => x.WriteAsync(It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
                .Returns(() => ValueTask.FromResult(Result.Success()));

            return mock_port;
        }
    }


    [DataRow((ushort)0x00FF)]
    [DataRow((ushort)0XFF00)]
    [DataRow((ushort)0xFFFF)]
    [TestMethod(DisplayName = "读Modbus保持寄存器")]
    public async Task ReadMobudsHoldingRegisters(ushort value)
    {
        const byte slave_id = 1;
        const ushort address = 1;

        // 模拟
        var mock_port = MockPort(slave_id, value);
        ModbusRtuMaster master = new(mock_port.Object);
        ModbusChannel channel = new(master);

        // 发送数据
        var read_tag = TagFactory.UInt16(ModbusAddress.ReadHoldingRegisters(slave_id, address));
        var read_result = await channel.ReadAsync(read_tag, TestContext.CancellationToken);

        // 验证
        Assert.IsTrue(read_result.IsSuccess, read_result.Message);
        Assert.AreEqual(value, read_result.Value);


        // 读线圈端口模拟
        static Mock<ITransport> MockPort(byte slaveId, params ushort[] values)
        {
            // 值数量
            var quantity_result = ReadCoilsQuantity.Create(values.Length);
            Assert.IsTrue(quantity_result.IsSuccess, quantity_result.Message);

            // 响应布局
            var layout = RtuReadResponseLayout.FromQuantity(quantity_result.Value);
            // 响应头
            var header = ReadResponseHeader.HoldingRegisters(slaveId, quantity_result.Value.ByteLength);
            // 响应构建
            var read_index = 0;
            Memory<byte> resp_buffer = new byte[quantity_result.Value.ByteLength];
            var encode_result = RtuSlaveReadCodec.EncodeRegistersResponse(resp_buffer.Span, header, values, layout);
            Assert.IsTrue(encode_result.IsSuccess, encode_result.Message);

            // 响应模拟
            var mock_port = new Mock<ITransport>();
            mock_port
                .Setup(x => x.ReadExactAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
                .Returns<Memory<byte>, CancellationToken>((buffer, _) =>
                {
                    var data = resp_buffer.Slice(read_index, buffer.Length);
                    data.CopyTo(buffer);

                    read_index += buffer.Length;
                    var result = Result.Success(buffer.Length);
                    return new ValueTask<Result>(Result.Success());
                });
            mock_port.Setup(x => x.WriteAsync(It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
                .Returns(() => ValueTask.FromResult(Result.Success()));

            return mock_port;
        }
    }




    internal static Mock<IPort> MockPort<T>(EncodeHandler<T> encodeHandler, RtuReadResponseLayout layout, ReadResponseHeader header, params T[] values)
        where T : unmanaged
    {
        // 值数量
        var quantity_result = ReadCoilsQuantity.Create(values.Length);
        Assert.IsTrue(quantity_result.IsSuccess, quantity_result.Message);

        // 响应构建
        var read_index = 0;
        Memory<byte> resp_buffer = new byte[quantity_result.Value.ByteLength];
        var encode_result = encodeHandler(resp_buffer, header, values, layout);
        Assert.IsTrue(encode_result.IsSuccess, encode_result.Message);

        // 响应模拟
        var mock_port = new Mock<IPort>();
        mock_port
            .Setup(x => x.ReadExactAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns<Memory<byte>, CancellationToken>((buffer, _) =>
            {
                var data = resp_buffer.Slice(read_index, buffer.Length);
                data.CopyTo(buffer);

                read_index += buffer.Length;
                return new ValueTask<Result>(Result.Success());
            });
        mock_port.Setup(x => x.WriteAsync(It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
            .Returns(() => ValueTask.FromResult(Result.Success()));

        return mock_port;
    }

    internal delegate Result<int> EncodeHandler<T>(Memory<byte> Buffer, ReadResponseHeader header, ReadOnlySpan<T> valeus, RtuReadResponseLayout layout)
        where T : unmanaged;
}
