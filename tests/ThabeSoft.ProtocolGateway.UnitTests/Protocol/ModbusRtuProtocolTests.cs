using ThabeSoft.ProtocolGateway.Modbus.Primitives;
using ThabeSoft.ProtocolGateway.Modbus.Protocols;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Headers;
using ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Protocol;


[TestClass]
public sealed class ModbusRtuProtocolTests
{
    public TestContext TestContext { get; set; }


    [DataRow((byte)01, (ushort)100, (byte)1, DisplayName = "从站01, 寄存器(100~101]")]
    [DataRow((byte)03, (ushort)200, (byte)5, DisplayName = "从站03, 寄存器(200~205]")]
    [DataRow((byte)05, (ushort)300, (byte)123, DisplayName = "从站05, 寄存器(300~425]")]
    [TestMethod(DisplayName = "写多个寄存器打包解包")]
    public async Task WriteMultipleRegisters_Pack_Unpack(byte slaveId, ushort address, byte quantity)
    {
        // 获取帧布局
        var layout_result = WriteRegistersQuantity.Create(quantity)
            .Then(RtuWriteMultipleRequestLayout.CreateRegisters);
        Assert.IsTrue(layout_result, layout_result.Message);

        Span<byte> pack_buffer = stackalloc byte[layout_result.Value.TotalLength];

        // 创建请求头
        var header_result = WriteMultipleRequestHeader.Registers(slaveId, address);
        Assert.IsTrue(header_result, header_result.Message);

        // 创建请求数据
        Span<ushort> pack_value = stackalloc ushort[quantity];
        RandomFill(pack_value, ushort.MinValue, ushort.MaxValue);

        // 编码
        var encode_result = RtuRequestEncoder.WriteMultipleRegisters(pack_buffer, pack_value, header_result.Value, layout_result.Value);
        Assert.IsTrue(encode_result, encode_result.Message);

        // 解码
        Span<ushort> unpack_value = stackalloc ushort[quantity];
        var unpack_result = RtuRequestDecoder.WriteMultipleRegisters(pack_buffer, unpack_value, layout_result.Value);
        Assert.IsTrue(unpack_result, unpack_result.Message);


        // Assert
        var pack_values = pack_value.ToArray();
        var unpack_header = unpack_result.Value;
        var unpack_values = unpack_value.ToArray();

        Assert.AreEqual(slaveId, unpack_header.SlaveId);
        Assert.AreEqual(address, unpack_header.Address);
        CollectionAssert.AreEqual(pack_values, unpack_values);

        Console.WriteLine($"写入{quantity}个寄存器: {ToString(pack_values)}");
        Console.WriteLine($"从站Id: {unpack_header.SlaveId}");
        Console.WriteLine($"起始地址: {unpack_header.Address}");
        Console.WriteLine($"数据: {ToString(unpack_values)}");
    }


    [DataRow((byte)01, (ushort)100, (ushort)3, DisplayName = "从站01, 线圈(100~103]")]
    [DataRow((byte)03, (ushort)200, (ushort)5, DisplayName = "从站03, 线圈(200~205]")]
    [TestMethod(DisplayName = "写多个线圈打包解包")]
    public async Task WriteMultipleCoils_Pack_Unpack(byte slaveId, ushort address, ushort quantity)
    {
        // 获取帧布局
        var layout_result = WriteCoilsQuantity.Create(quantity)
            .Then(RtuWriteMultipleRequestLayout.CreateCoils);
        Assert.IsTrue(layout_result, layout_result.Message);

        Span<byte> pack_buffer = stackalloc byte[layout_result.Value.TotalLength];

        // 创建请求头
        var header_result = WriteMultipleRequestHeader.Registers(slaveId, address);
        Assert.IsTrue(header_result, header_result.Message);

        // 创建请求数据
        Span<bool> pack_value = stackalloc bool[quantity];
        RandomFill(pack_value);

        // 编码
        var encode_result = RtuRequestEncoder.WriteMultipleCoils(pack_buffer, pack_value, header_result.Value);
        Assert.IsTrue(encode_result, encode_result.Message);

        // 解码
        Span<bool> unpack_value = stackalloc bool[quantity];
        var unpack_result = RtuRequestDecoder.WriteMultipleCoils(pack_buffer, unpack_value, layout_result.Value);
        Assert.IsTrue(unpack_result, unpack_result.Message);


        // Assert
        var pack_values = pack_value.ToArray();
        var unpack_header = unpack_result.Value;
        var unpack_values = unpack_value.ToArray();

        Assert.AreEqual(slaveId, unpack_header.SlaveId);
        Assert.AreEqual(address, unpack_header.Address);
        CollectionAssert.AreEqual(pack_values, unpack_values);

        Console.WriteLine($"写入{quantity}个线圈: {ToString(pack_values)}");
        Console.WriteLine($"从站Id: {unpack_header.SlaveId}");
        Console.WriteLine($"起始地址: {unpack_header.Address}");
        Console.WriteLine($"数据: {ToString(unpack_values)}");
    }

    [DataRow((byte)01, (ushort)100, true, DisplayName = "从站01, 线圈100, 值True")]
    [DataRow((byte)03, (ushort)200, false, DisplayName = "从站03, 线圈200, 值False")]
    [TestMethod(DisplayName = "写单个线圈打包解包")]
    public async Task WriteSingleCoil_Pack_Unpack(byte slaveId, ushort address, bool value)
    {
        // 获取帧布局
        var layout = RtuWriteSingleRequestLayout.Instance;
        Span<byte> pack_buffer = stackalloc byte[layout.TotalLength];

        // 创建请求头
        var header_result = WriteSingleRequestHeader.Coil(slaveId, address, true);
        Assert.IsTrue(header_result, header_result.Message);

        // 编码
        var encode_result = RtuRequestEncoder.WriteSingle(pack_buffer, header_result.Value);
        Assert.IsTrue(encode_result, encode_result.Message);

        // 解码
        var unpack_result = RtuRequestDecoder.WriteSingleCoil(pack_buffer);
        Assert.IsTrue(unpack_result, unpack_result.Message);


        // Assert
        var unpack_header = unpack_result.Value;

        Assert.AreEqual(slaveId, unpack_header.SlaveId);
        Assert.AreEqual(address, unpack_header.Address);

        Console.WriteLine($"写入线圈: {unpack_header.Value}");
        Console.WriteLine($"从站Id: {unpack_header.SlaveId}");
        Console.WriteLine($"起始地址: {unpack_header.Address}");
        Console.WriteLine($"数据: {unpack_header.Value}");
    }

    [DataRow((byte)01, (ushort)100, (ushort)0xFF, DisplayName = "从站01, 线圈100, 值0xFF")]
    [DataRow((byte)03, (ushort)200, (ushort)0xF0, DisplayName = "从站03, 线圈200, 值0xF0")]
    [DataRow((byte)05, (ushort)300, (ushort)0x0F, DisplayName = "从站05, 线圈300, 值0x0F")]
    [TestMethod(DisplayName = "写单个寄存器打包解包")]
    public async Task WriteSingleRegister_Pack_Unpack(byte slaveId, ushort address, ushort value)
    {
        // 获取帧布局
        var layout = RtuWriteSingleRequestLayout.Instance;
        Span<byte> pack_buffer = stackalloc byte[layout.TotalLength];

        // 创建请求头
        var header_result = WriteSingleRequestHeader.Register(slaveId, address, 0xF0);
        Assert.IsTrue(header_result, header_result.Message);

        // 编码
        var encode_result = RtuRequestEncoder.WriteSingle(pack_buffer, header_result.Value);
        Assert.IsTrue(encode_result, encode_result.Message);

        // 解码
        var unpack_result = RtuRequestDecoder.WriteSingleRegister(pack_buffer);
        Assert.IsTrue(unpack_result, unpack_result.Message);


        // Assert
        var unpack_header = unpack_result.Value;

        Assert.AreEqual(slaveId, unpack_header.SlaveId);
        Assert.AreEqual(address, unpack_header.Address);

        Console.WriteLine($"写入寄存器: {unpack_header.Value}");
        Console.WriteLine($"从站Id: {unpack_header.SlaveId}");
        Console.WriteLine($"起始地址: {unpack_header.Address}");
        Console.WriteLine($"数据: {unpack_header.Value}");
    }

    [DataRow((byte)01, (ushort)100, (ushort)3, DisplayName = "从站01, 线圈100, 数量3")]
    [DataRow((byte)03, (ushort)200, (ushort)5, DisplayName = "从站03, 线圈200, 数量5")]
    [TestMethod(DisplayName = "读取线圈打包解包")]
    public async Task ReadCoils_Pack_Unpack(byte slaveId, ushort address, ushort quantity)
    {
        // 获取帧布局
        var layout = RtuReadRequestLayout.Instance;
        Span<byte> pack_buffer = stackalloc byte[layout.TotalLength];

        // 创建请求头
        var header_result = ReadRequestHeader.Coils(slaveId, address, 10);
        Assert.IsTrue(header_result, header_result.Message);

        // 编码
        var encode_result = RtuRequestEncoder.Read(pack_buffer, header_result.Value);
        Assert.IsTrue(encode_result, encode_result.Message);

        // 解码
        var unpack_result = RtuRequestDecoder.ReadCoils(pack_buffer);
        Assert.IsTrue(unpack_result, unpack_result.Message);


        // Assert
        var unpack_header = unpack_result.Value;

        Assert.AreEqual(slaveId, unpack_header.SlaveId);
        Assert.AreEqual(address, unpack_header.Address);

        Console.WriteLine($"从站Id: {unpack_header.SlaveId}");
        Console.WriteLine($"功能码: {unpack_header.FunctionCode}");
        Console.WriteLine($"起始地址: {unpack_header.Address}");
        Console.WriteLine($"读取数量: {unpack_header.Quantity}");
    }


    // 转为字节字符串
    private static string ToString(IEnumerable<byte> items)
    {
        return string.Join(' ', items.Select(x => x.ToString("X2")));
    }
    private static string ToString(IEnumerable<bool> items)
    {
        return string.Join(' ', items);
    }
    private static string ToString(IEnumerable<ushort> items)
    {
        var bytes = items.SelectMany<ushort, byte>(x => [(byte)(x >> 8), (byte)x]);
        return ToString(bytes);
    }
    // 随机填充数据
    private static void RandomFill(Span<ushort> values, int min, int max)
    {
        for (int i = 0; i < values.Length; i++) values[i] = (ushort)Random.Shared.Next(min, max);
    }
    // 随机填充数据
    private static void RandomFill(Span<bool> values)
    {
        for (int i = 0; i < values.Length; i++) values[i] = Random.Shared.NextDouble() < 0.6;
    }
}
