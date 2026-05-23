using ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;
using ThabeSoft.ProtocolGateway.Protocols;
using ThabeSoft.ProtocolGateway.Protocols.Serializer;

namespace ThabeSoft.ProtocolGateway.Protocol;


[TestClass]
public sealed class ModbusRtuProtocolTests
{
    public TestContext TestContext { get; set; }


    [DataRow((byte)01, (ushort)100, (byte)1, DisplayName = "从站01, 寄存器(100~101]")]
    [DataRow((byte)03, (ushort)200, (byte)5, DisplayName = "从站03, 寄存器(200~205]")]
    [DataRow((byte)05, (ushort)300, (byte)125, DisplayName = "从站05, 寄存器(300~425]")]
    [TestMethod(DisplayName = "写多个寄存器打包解包")]
    public async Task WriteMultipleRegisters_Pack_Unpack(byte slaveId, ushort address, byte quantity)
    {
        IModbusWriteMultipleRegistersRequestSerializer serializer = ModbusRtuRequest.WriteMultipleRegistersSerializer;


        RtuWriteMultipleRequestLayout.TryCreayeRegisters(quantity, out var layout);

        ModbusRtuWriteMultipleRequestSerializer.TryPackRegisters(layout, null, slaveId, address, []);



        Span<byte> buffer_span = stackalloc byte[1024];
        Span<ushort> value_span = stackalloc ushort[quantity];
        RandomFill(value_span, ushort.MinValue, ushort.MaxValue);

        // Pack
        serializer.TryPack(buffer_span, slaveId, address, value_span);

        // Unpack
        Span<ushort> unpack_value_span = stackalloc ushort[quantity];
        serializer.TryUnpack(buffer_span, out var unpack_slave_id, out var unpack_address, unpack_value_span);

        // Assert
        var values = value_span.ToArray();
        var unpack_values = unpack_value_span.ToArray();

        Assert.AreEqual(slaveId, unpack_slave_id);
        Assert.AreEqual(address, unpack_address);
        CollectionAssert.AreEqual(values, unpack_values);

        Console.WriteLine($"写入{quantity}个寄存器: {ToString(buffer_span.ToArray())}");
        Console.WriteLine($"从站Id: {unpack_slave_id}");
        Console.WriteLine($"起始地址: {unpack_address}");
        Console.WriteLine($"数据: {ToString(unpack_values)}");
    }


    [DataRow((byte)01, (ushort)100, (ushort)3, DisplayName = "从站01, 线圈(100~103]")]
    [DataRow((byte)03, (ushort)200, (ushort)5, DisplayName = "从站03, 线圈(200~205]")]
    [TestMethod(DisplayName = "写多个线圈打包解包")]
    public async Task WriteMultipleCoils_Pack_Unpack(byte slaveId, ushort address, ushort quantity)
    {
        IModbusWriteMultipleCoilsRequestSerializer serializer = ModbusRtuRequest.WriteMultipleCoilsSerializer;

        Span<byte> source_span = stackalloc byte[1024];
        Span<bool> value_span = stackalloc bool[quantity];
        RandomFill(value_span);

        // Pack
        serializer.TryPack(source_span, slaveId, address, value_span);

        // Unpack
        Span<bool> unpack_value_span = stackalloc bool[quantity];
        serializer.TryUnpack(source_span, out var unpack_slave_id, out var unpack_address, unpack_value_span);

        // Assert
        var values = value_span.ToArray();
        var unpack_values = unpack_value_span.ToArray();

        Assert.AreEqual(slaveId, unpack_slave_id);
        Assert.AreEqual(address, unpack_address);
        CollectionAssert.AreEqual(values, unpack_values);

        Console.WriteLine($"写入{quantity}个线圈: {ToString(value_span.ToArray())}");
        Console.WriteLine($"从站Id: {unpack_slave_id}");
        Console.WriteLine($"起始地址: {unpack_address}");
        Console.WriteLine($"数据: {ToString(unpack_values)}");
    }

    [DataRow((byte)01, (ushort)100, true, DisplayName = "从站01, 线圈100, 值True")]
    [DataRow((byte)03, (ushort)200, false, DisplayName = "从站03, 线圈200, 值False")]
    [TestMethod(DisplayName = "写单个线圈打包解包")]
    public async Task WriteSingleCoil_Pack_Unpack(byte slaveId, ushort address, bool value)
    {
        IModbusWriteSingleCoilRequestSerializer serializer = ModbusRtuRequest.WriteSingleCoilSerializer;

        Span<byte> source_span = stackalloc byte[1024];

        // Pack
        serializer.TryPack(source_span, slaveId, address, value);
        // Unpack
        serializer.TryUnpack(source_span, out var unpack_slave_id, out var unpack_address, out var unpack_value);

        // Assert
        Assert.AreEqual(slaveId, unpack_slave_id);
        Assert.AreEqual(address, unpack_address);
        Assert.AreEqual(value, unpack_value);

        Console.WriteLine($"写入线圈: {ToString(source_span.ToArray())}");
        Console.WriteLine($"从站Id: {unpack_slave_id}");
        Console.WriteLine($"起始地址: {unpack_address}");
        Console.WriteLine($"值: {value}");
    }

    [DataRow((byte)01, (ushort)100, (ushort)0xFF, DisplayName = "从站01, 线圈100, 值0xFF")]
    [DataRow((byte)03, (ushort)200, (ushort)0xF0, DisplayName = "从站03, 线圈200, 值0xF0")]
    [DataRow((byte)05, (ushort)300, (ushort)0x0F, DisplayName = "从站05, 线圈300, 值0x0F")]
    [TestMethod(DisplayName = "写单个寄存器打包解包")]
    public async Task WriteSingleRegister_Pack_Unpack(byte slaveId, ushort address, ushort value)
    {
        IModbusWriteSingleRegisterRequestSerializer serializer = ModbusRtuRequest.WriteSingleRegisterSerializer;

        Span<byte> source_span = stackalloc byte[1024];

        // Pack
        serializer.TryPack(source_span, slaveId, address, value);
        // Unpack
        serializer.TryUnpack(source_span, out var unpack_slave_id, out var unpack_address, out var unpack_value);

        // Assert
        Assert.AreEqual(slaveId, unpack_slave_id);
        Assert.AreEqual(address, unpack_address);
        Assert.AreEqual(value, unpack_value);

        Console.WriteLine($"写入线圈: {ToString(source_span.ToArray())}");
        Console.WriteLine($"从站Id: {unpack_slave_id}");
        Console.WriteLine($"起始地址: {unpack_address}");
        Console.WriteLine($"值: {value}");
    }

    [DataRow((byte)01, (ushort)100, (ushort)3, DisplayName = "从站01, 线圈100, 数量3")]
    [DataRow((byte)03, (ushort)200, (ushort)5, DisplayName = "从站03, 线圈200, 数量5")]
    [TestMethod(DisplayName = "读取线圈打包解包")]
    public async Task ReadCoils_Pack_Unpack(byte slaveId, ushort address, ushort quantity)
    {
        IModbusWriteSingleRegisterRequestSerializer serializer = ModbusRtuRequest.WriteSingleRegisterSerializer;

        Span<byte> source_span = stackalloc byte[1024];

        // Pack
        serializer.TryPack(source_span, slaveId, address, quantity);
        // Unpack
        serializer.TryUnpack(source_span, out var unpack_slave_id, out var unpack_address, out var unpack_quantity);

        // Assert
        Assert.AreEqual(slaveId, unpack_slave_id);
        Assert.AreEqual(address, unpack_address);
        Assert.AreEqual(quantity, unpack_quantity);

        Console.WriteLine($"写入线圈: {ToString(source_span.ToArray())}");
        Console.WriteLine($"从站Id: {unpack_slave_id}");
        Console.WriteLine($"起始地址: {unpack_address}");
        Console.WriteLine($"数量: {quantity}");
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
