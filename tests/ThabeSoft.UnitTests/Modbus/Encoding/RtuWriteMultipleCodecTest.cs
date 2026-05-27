using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Layouts;

namespace ThabeSoft.Modbus.Encoding;


[TestClass]
public sealed class RtuCodecTest
{
    [DataRow((byte)01, (ushort)100, (ushort)10, DisplayName = "从站=01,地址=100,数量=10")]
    [DataRow((byte)03, (ushort)200, (ushort)20, DisplayName = "从站=03,地址=200,数量=20")]
    [TestMethod(DisplayName = "读多个线圈")]
    public void ReadCoils(byte slaveId, ushort address, ushort quantity)
    {
        // 布局
        var layout = RtuReadRequestLayout.Instance;
        Span<byte> encode_buffer = stackalloc byte[layout.TotalLength];

        // 请求头
        var request_header_result = ReadRequestHeader.Coils(slaveId, address, quantity);
        Assert.IsTrue(request_header_result.IsSuccess, request_header_result.Message);

        // 编码
        var encode_result = RtuMasterReadCodec.EncodeRequest(encode_buffer, request_header_result.Value, layout);
        Console.WriteLine($"编码: {encode_buffer.ToHexString()}");
        Assert.IsTrue(encode_result.IsSuccess, encode_result.Message);

        // 解码
        var decoode_result = RtuSlaveReadCodec.DecodeRequest(encode_buffer, layout);
        Assert.IsTrue(decoode_result.IsSuccess, decoode_result.Message);


        var request_header = request_header_result.Value;
        var response_header = decoode_result.Value;

        // Assert
        Assert.AreEqual(slaveId, response_header.SlaveId, message: "从站Id不匹配");
        Assert.AreEqual(address, response_header.Address, message: "地址不匹配");
        Assert.AreEqual(quantity, response_header.Quantity, message: "数量不匹配");


        Console.WriteLine($"从站: {response_header.SlaveId}");
        Console.WriteLine($"地址: {response_header.Address}");
        Console.WriteLine($"数量: {response_header.Quantity}");
        Console.WriteLine($"Crc: {response_header.Crc.ToHexString()}");
    }


    [DataRow((byte)01, (ushort)100, (ushort)10, DisplayName = "从站=01,地址=100,数量=10")]
    [DataRow((byte)03, (ushort)200, (ushort)20, DisplayName = "从站=03,地址=200,数量=20")]
    [TestMethod(DisplayName = "读多个保持寄存器")]
    public void ReadHoldingRegisters(byte slaveId, ushort address, ushort quantity)
    {
        // 布局
        var layout = RtuReadRequestLayout.Instance;
        Span<byte> encode_buffer = stackalloc byte[layout.TotalLength];

        // 请求头
        var request_header_result = ReadRequestHeader.HoldingRegisters(slaveId, address, quantity);
        Assert.IsTrue(request_header_result.IsSuccess, request_header_result.Message);

        // 编码
        var encode_result = RtuMasterReadCodec.EncodeRequest(encode_buffer, request_header_result.Value, layout);
        Console.WriteLine($"编码: {encode_buffer.ToHexString()}");
        Assert.IsTrue(encode_result.IsSuccess, encode_result.Message);

        // 解码
        var decoode_result = RtuSlaveReadCodec.DecodeRequest(encode_buffer, layout);
        Assert.IsTrue(decoode_result.IsSuccess, decoode_result.Message);


        var request_header = request_header_result.Value;
        var response_header = decoode_result.Value;

        // Assert
        Assert.AreEqual(slaveId, response_header.SlaveId, message: "从站Id不匹配");
        Assert.AreEqual(address, response_header.Address, message: "地址不匹配");
        Assert.AreEqual(quantity, response_header.Quantity, message: "数量不匹配");


        Console.WriteLine($"从站: {response_header.SlaveId}");
        Console.WriteLine($"地址: {response_header.Address}");
        Console.WriteLine($"数量: {response_header.Quantity}");
        Console.WriteLine($"Crc: {response_header.Crc.ToHexString()}");
    }


    [DataRow((byte)01, (ushort)100, true, false, DisplayName = "从站=01,地址=100,值=True,False")]
    [DataRow((byte)03, (ushort)200, false, true, DisplayName = "从站=03,地址=200,值=False,True")]
    [TestMethod(DisplayName = "写多个线圈")]
    public void WriteMultipleCoils(byte slaveId, ushort address, bool value1, bool value2)
    {
        // 布局
        var layout_result = RtuWriteMultipleRequestLayout.FromCoilsQuantity(2);
        Assert.IsTrue(layout_result.IsSuccess, layout_result.Message);
        Span<byte> buffer = stackalloc byte[layout_result.Value.TotalLength];

        // 请求头
        var header = WriteMultipleRequestHeader.Coils(slaveId, address);
        // 值
        Span<bool> values = [value1, value2];
        // 编码
        RtuMasterWriteMultipleCodec.EncodeCoilsRequest(buffer, header, values, layout_result.Value);

        // 打印
        Console.WriteLine(buffer.ToHexString());


    }


    [DataRow((byte)01, (ushort)100, (ushort)10, (ushort)20, DisplayName = "从站=01,地址=100,值=10,20")]
    [DataRow((byte)03, (ushort)200, (ushort)100, (ushort)200, DisplayName = "从站=03,地址=200,值=100,200")]
    [TestMethod(DisplayName = "写多个线圈")]
    public void WriteMultipleRegisters(byte slaveId, ushort address, ushort value1, ushort value2)
    {
        // 布局
        var layout_result = RtuWriteMultipleRequestLayout.FromRegistersQuantity(2);
        Assert.IsTrue(layout_result.IsSuccess, layout_result.Message);
        Span<byte> buffer = stackalloc byte[layout_result.Value.TotalLength];

        // 请求头
        var header = WriteMultipleRequestHeader.Registers(slaveId, address);
        // 值
        Span<ushort> values = [value1, value2];
        // 编码
        RtuMasterWriteMultipleCodec.EncodeRegistersRequest(buffer, header, values, layout_result.Value);

        // 打印
        Console.WriteLine(buffer.ToHexString());
    }



    
}
