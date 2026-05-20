//using IndustrialHub.Modbus.Bits;
//using ThabeSoft.IndustriaHub.Protocol;

//namespace IndustrialHub.UnitTests;


//[TestClass]
//public class Bits8Test
//{
//    [TestMethod(DisplayName = "小端序解析")]
//    public void TryFromBits_LittleEndian()
//    {
//        bool[] data = [ false, false, true, false,   true, true, true, true ];

//        if (!Bits8.TryFromBits(data, Endianness.LittleEndian, out var result))
//        {
//            Assert.Fail("无法从位数组创建Bits8实例");
//        }

//        const byte expected = 0b1111_0100;
//        Assert.AreEqual(expected, result.ToByte());
//    }

//    [TestMethod(DisplayName = "大端序解析")]
//    public void TryFromBits_BigEndian()
//    {
//        bool[] data = [false, true, false, false,  true, true, true, true];

//        if (!Bits8.TryFromBits(data, Endianness.BigEndian, out var result))
//        {
//            Assert.Fail("无法从位数组创建Bits8实例");
//        }

//        Console.WriteLine(result);

//        const byte expected = 0b0100_1111;
//        Assert.AreEqual(expected, result.ToByte());
//    }


//    [TestMethod(DisplayName = "小端序解析缺位填充")]
//    public void TryFromBits_LittleEndian_Fill()
//    {
//        bool[] data = [true, false, true];

//        if (!Bits8.TryFromBits(data, Endianness.LittleEndian, out var result))
//        {
//            Assert.Fail("无法从位数组创建Bits8实例");
//        }

//        const byte expected = 0b0000_0101;
//        Assert.AreEqual(expected, result.ToByte());
//    }

//    [TestMethod(DisplayName = "大端序解析缺位填充")]
//    public void TryFromBits_BigEndian_Fill()
//    {
//        bool[] data = [true, false, true];

//        if (!Bits8.TryFromBits(data, Endianness.BigEndian, out var result))
//        {
//            Assert.Fail("无法从位数组创建Bits8实例");
//        }

//        Console.WriteLine(result);

//        const byte expected = 0b1010_0000;
//        Assert.AreEqual(expected, result.ToByte());
//    }
//}
