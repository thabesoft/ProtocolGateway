using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Services;


[TestClass]
public class ChannelJsonConfigServiceTests
{
    public TestContext TestContext { get; set; }


    //[TestMethod]
    //public async Task Should_Save_And_Load_From_File()
    //{
    //    // Arrange
    //    var service = new ChannelJsonConfigService();
    //    var config = new ChannelConfig()
    //    {
    //        Name = ChannelName.Create("TestChannel").Value,
    //        Protocol = Enums.ProtocolType.ModbusRtu,
    //        Port = new SerialPortConfig() { PortName = "Com2" },
    //        Tags =
    //        [
    //            new ModbusTagConfig(){ ValueType = TagValueType.Bool, SlaveId = 1, Address = 100, FunctionCode =Modbus.FunctionCode.ReadCoils },
    //            new ModbusTagConfig(){ ValueType = TagValueType.Int16, SlaveId = 1, Address = 102, FunctionCode =Modbus.FunctionCode.ReadHoldingRegisters },
    //        ]
    //    };

    //    ChannelConfig[] values = [config];
    //    var tempFile = Path.GetTempFileName();

    //    try
    //    {
    //        // Act - Save
    //        await service.SaveToFileAsync(values, tempFile, TestContext.CancellationToken);

    //        // Act - Load
    //        var loaded = await service.LoadFromFileAsync(tempFile, TestContext.CancellationToken);

    //        // Assert
    //        Assert.IsNotNull(loaded);
    //        Assert.AreEqual(config.Name, loaded[0].Name);
    //    }
    //    finally
    //    {
    //        File.Delete(tempFile);
    //    }
    //}
}
