using IndustrialHub.Modbus.Protocol.Rtu;

namespace IndustrialHub.UnitTests;


[TestClass]
public sealed class ModbusRtuProtocolTests
{
    public TestContext TestContext { get; set; }


    [DataRow((byte)01, (ushort)100, 1, 3, DisplayName = "从站01, 地址100, 寄存器[1~3]")]
    [DataRow((byte)03, (ushort)200, 1, 5, DisplayName = "从站03, 地址200, 寄存器[1~5]")]
    [TestMethod(DisplayName = "写多个寄存器")]
    public async Task TryPackWriteMultipleRegistersFrameLayout(byte slaveId, ushort address, int min, int max)
    {
        var frame_data = new byte[1024];
        var values = CreateRandomLength<ushort>(min, max);
        RandomFill(values, ushort.MinValue, ushort.MaxValue);


        var quantity =(byte)values.Length;
        var layout = RtuProtocol.WriteMultipleRegisters(quantity);

        var span = frame_data.AsSpan(0, layout.FullByteLength);
        var data = values.AsSpan(0, layout.DataMaxQuantity);

        layout.TryPack(span, slaveId, address, data);


        Console.WriteLine("写入{0}个寄存器: ({1})", quantity, string.Join(", ", values));
        Console.WriteLine(string.Join(' ', span.ToArray().Select(x => x.ToString("X2"))));
    }



    [DataRow((byte)01, (ushort)100, 1, 3, DisplayName = "从站01, 地址100, 线圈[1~3]")]
    [DataRow((byte)03, (ushort)200, 1, 5, DisplayName = "从站03, 地址200, 线圈[1~5]")]
    [TestMethod(DisplayName = "写多个线圈")]
    public async Task TryPackWriteMultipleCoilsFrameLayout(byte slaveId, ushort address, int min, int max)
    {
        var frame_data = new byte[1024];
        var values = CreateRandomLength<bool>(min, max);
        RandomFill(values);

        var quantity = (byte)values.Length;
        var layout = RtuProtocol.WriteMultipleCoils(quantity);

        var span = frame_data.AsSpan(0, layout.FullByteLength);
        var data = values.AsSpan(0, layout.DataMaxQuantity);

        layout.TryPack(span, slaveId, address, data);


        Console.WriteLine("写入{0}个线圈: ({1})", quantity, string.Join(", ", values));
        Console.WriteLine(string.Join(' ', span.ToArray().Select(x => x.ToString("X2"))));
    }



    // 创建随机长度的数组
    private static T[] CreateRandomLength<T>(int min, int max)
    {
        return new T[Random.Shared.Next(min, max)];
    }
    // 随机填充数据
    private static void RandomFill(Span<ushort> values, int min, int max)
    {
        for (int i = 0; i < values.Length; i++) values[i] = (ushort)Random.Shared.Next(min, max);
    }
    // 随机填充数据
    private static void RandomFill(Span<bool> values)
    {
        for (int i = 0; i < values.Length; i++) values[i] = Random.Shared.Next(0, 2) == 1;
    }
}
