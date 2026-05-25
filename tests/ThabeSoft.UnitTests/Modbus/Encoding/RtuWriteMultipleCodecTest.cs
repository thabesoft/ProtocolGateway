using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Layouts;

namespace ThabeSoft.Modbus.Encoding;


[TestClass]
public sealed class RtuWriteMultipleCodecTest
{
    [DataRow((byte)01, (ushort)100, true, false, DisplayName = "从站=01,地址=100,值=True,False")]
    [DataRow((byte)03, (ushort)200, false, true, DisplayName = "从站=03,地址=200,值=False,True")]
    [TestMethod(DisplayName = "写多个线圈")]
    public void EncodeCoilsRequest(byte slaveId, ushort address, bool value1, bool value2)
    {
        // 布局
        var layout_result = RtuWriteMultipleRequestLayout.FromCoilsQuantity(2);
        Assert.IsTrue(layout_result, layout_result.Message);
        Span<byte> buffer = stackalloc byte[layout_result.Value.TotalLength];

        // 请求头
        var header = WriteMultipleRequestHeader.Coils(slaveId, address);
        // 值
        Span<bool> values = [value1, value2];
        // 编码
        RtuMasterWriteMultipleCodec.EncodeCoilsRequest(buffer, header, values, layout_result.Value);

        // 打印
        Console.WriteLine(ToString(buffer.ToArray()));


    }


    [DataRow((byte)01, (ushort)100, (ushort)10, (ushort)20, DisplayName = "从站=01,地址=100,值=10,20")]
    [DataRow((byte)03, (ushort)200, (ushort)100, (ushort)200, DisplayName = "从站=03,地址=200,值=100,200")]
    [TestMethod(DisplayName = "写多个线圈")]
    public void EncodeRegistersRequest(byte slaveId, ushort address, ushort value1, ushort value2)
    {
        // 布局
        var layout_result = RtuWriteMultipleRequestLayout.FromRegistersQuantity(2);
        Assert.IsTrue(layout_result, layout_result.Message);
        Span<byte> buffer = stackalloc byte[layout_result.Value.TotalLength];

        // 请求头
        var header = WriteMultipleRequestHeader.Registers(slaveId, address);
        // 值
        Span<ushort> values = [value1, value2];
        // 编码
        RtuMasterWriteMultipleCodec.EncodeRegistersRequest(buffer, header, values, layout_result.Value);

        // 打印
        Console.WriteLine(ToString(buffer.ToArray()));
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
