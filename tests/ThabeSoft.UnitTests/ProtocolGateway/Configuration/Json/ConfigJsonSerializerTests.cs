using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Infrastructure.Json;

namespace ThabeSoft.ProtocolGateway.Configuration.Json;


[TestClass]
public class ConfigJsonSerializerTests
{
    public TestContext TestContext { get; set; }


    [TestMethod(DisplayName = "在文件中序列化和反序列化")]
    public async Task Serialize_And_Deserialize_From_File()
    {
        // Arrange
        var service = new ConfigJsonSerializer(ConfigJsonSerializerContext.Default);
        var config = new GatewayConfig()
        {
            Name = "测试网关",
            Channels =
            [
                new ChannelConfig()
                {
                    Name = ChannelName.Create("TestChannel").Value,
                    Protocol = ProtocolType.ModbusRtu,
                    Port = new SerialPortConfig() { PortName = "Com2" },
                    Tags =
                    [
                        new ModbusTagConfig()
                        {
                            Name ="布尔",
                            ValueType = TagValueType.Bool,
                            SlaveId = 1,
                            Address = 100,
                            FunctionCode =Modbus.FunctionCode.ReadCoils
                        },
                        new ModbusTagConfig()
                        {
                            Name = "字",
                            ValueType = TagValueType.Int16,
                            SlaveId = 1,
                            Address = 102,
                            FunctionCode =Modbus.FunctionCode.ReadHoldingRegisters
                        },
                    ]
                }
            ]
        };

        var tempFile = Path.GetTempFileName();

        try
        {
            // Act - Save
            await service.SerializeToFileAsync(config, tempFile, TestContext.CancellationToken);

            // Act - Load
            var loaded_esult = await service.DeserializeFromFileAsync(tempFile, TestContext.CancellationToken);

            // Assert
            Assert.IsTrue(loaded_esult.IsSuccess, loaded_esult.Message);
            Assert.AreEqual(config.Name, loaded_esult.Value.Name);
            Assert.HasCount(config.Channels.Count, loaded_esult.Value.Channels);

            // 输出
            var json_content = await File.ReadAllTextAsync(tempFile);
            Console.WriteLine(json_content);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }


    [TestMethod(DisplayName = "反序列化")]
    public async Task Deserialize()
    {
        // Arrange
        var service = new ConfigJsonSerializer(ConfigJsonSerializerContext.Default);
        const string json = """
        {
          "Name": "测试网关",
          "Channels": [
            {
              "Name": "TestChannel",
              "Protocol": "ModbusRtu",
              "Port": {
                "$type": "SerialPort",
                "PortName": "Com2",
                "BaudRate": 9600,
                "Parity": "None",
                "DataBits": 8,
                "StopBits": "One",
                "DuplexMode": "FullDuplex",
                "RetryCount": 5,
                "RetryInterval": "00:00:01",
                "ReadTimeout": "00:00:03",
                "WriteTimeout": "00:00:03"
              },
              "Tags": [
                {
                  "$type": "Modbus",
                  "SlaveId": 1,
                  "Address": 100,
                  "FunctionCode": 1,
                  "Name": "测试",
                  "ValueType": "Bool"
                }
              ]
            }
          ]
        }
        """;


        // Act - Save
        var config_result = service.Deserialize(json);

        // Assert
        Assert.IsTrue(config_result.IsSuccess, config_result.Message);
        Assert.AreEqual("测试网关", config_result.Value.Name);
        Assert.HasCount(1, config_result.Value.Channels);
    }


    [TestMethod(DisplayName = "序列化")]
    public async Task Serialize()
    {
        // Arrange
        var service = new ConfigJsonSerializer(ConfigJsonSerializerContext.Default);
        var config = new GatewayConfig()
        {
            Name = "测试网关",
            Channels =
            [
                new ChannelConfig()
                {
                    Name = ChannelName.Create("TestChannel").Value,
                    Protocol = ProtocolType.ModbusRtu,
                    Port = new SerialPortConfig() { PortName = "Com2" },
                    Tags =
                    [
                        new ModbusTagConfig()
                        {
                            Name ="布尔",
                            ValueType = TagValueType.Bool,
                            SlaveId = 1,
                            Address = 100,
                            FunctionCode =Modbus.FunctionCode.ReadCoils
                        },
                        new ModbusTagConfig()
                        {
                            Name = "字",
                            ValueType = TagValueType.Int16,
                            SlaveId = 1,
                            Address = 102,
                            FunctionCode =Modbus.FunctionCode.ReadHoldingRegisters
                        },
                    ]
                }
            ]
        };


        // Act - Save
        var json_result = service.Serialize(config);
        Assert.IsTrue(json_result.IsSuccess, json_result.Message);
        Console.WriteLine(json_result);
    }
}
