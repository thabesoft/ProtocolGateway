using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Conversion;


[TestClass]
public class ByteExtensionsTest
{

    [DataRow((byte)0b0000_1111, (byte)0b1111_0000, Endianness.LittleEndian, (ushort)0b1111_0000_0000_1111, DisplayName = "小端序：低字节0x0F,高字节0xF0 → 0xF00F")]
    [DataRow((byte)0b1111_0000, (byte)0b0000_1111, Endianness.BigEndian, (ushort)0b1111_0000_0000_1111, DisplayName = "大端序：高字节0xF0,低字节0x0F → 0xF00F")]
    [DataRow((byte)0b1010_1010, (byte)0b0101_0101, Endianness.LittleEndian, (ushort)0b0101_0101_1010_1010, DisplayName = "小端序：0xAA,0x55 → 0x55AA")]
    [DataRow((byte)0b1010_1010, (byte)0b0101_0101, Endianness.BigEndian, (ushort)0b1010_1010_0101_0101, DisplayName = "大端序：0xAA,0x55 → 0xAA55")]
    [DataRow((byte)0x12, (byte)0x34, Endianness.LittleEndian, (ushort)0x3412, DisplayName = "小端序：0x12,0x34 → 0x3412")]
    [DataRow((byte)0x12, (byte)0x34, Endianness.BigEndian, (ushort)0x1234, DisplayName = "大端序：0x12,0x34 → 0x1234")]
    [DataRow((byte)0xFF, (byte)0x00, Endianness.LittleEndian, (ushort)0x00FF, DisplayName = "小端序：0xFF,0x00 → 0x00FF")]
    [DataRow((byte)0xFF, (byte)0x00, Endianness.BigEndian, (ushort)0xFF00, DisplayName = "大端序：0xFF,0x00 → 0xFF00")]
    [DataRow((byte)0x00, (byte)0xFF, Endianness.LittleEndian, (ushort)0xFF00, DisplayName = "小端序：0x00,0xFF → 0xFF00")]
    [DataRow((byte)0x00, (byte)0xFF, Endianness.BigEndian, (ushort)0x00FF, DisplayName = "大端序：0x00,0xFF → 0x00FF")]
    [TestMethod(DisplayName = "字节转ushort测试")]
    public void TryToUInt16(byte first, byte second, Endianness endianness, ushort expected)
    {
        byte[] source = [first, second];

        source.ToWord(endianness).Tap(x => Assert.AreEqual(expected, x));
    }

    [TestMethod(DisplayName = "字节转位数组测试_LSB0")]
    public void Byte_ToBits_LSB0()
    {
        const byte source = 0b1010_1010;
        bool[] destination = new bool[8];

        var result = source.ToBits(destination, BitOrder.LSB0);
        Assert.IsTrue(result, result.Message);

        bool[] expected = [false, true, false, true, false, true, false, true];
        CollectionAssert.AreEqual(expected, destination);
    }

    [TestMethod(DisplayName = "字节转位数组测试_MSB0")]
    public void Byte_ToBits_MSB0()
    {
        const byte source = 0b1010_1010;
        bool[] destination = new bool[8];

        var result = source.ToBits(destination, BitOrder.MSB0);
        Assert.IsTrue(result, result.Message);

        bool[] expected = [true, false, true, false, true, false, true, false];
        CollectionAssert.AreEqual(expected, destination);
    }
}
