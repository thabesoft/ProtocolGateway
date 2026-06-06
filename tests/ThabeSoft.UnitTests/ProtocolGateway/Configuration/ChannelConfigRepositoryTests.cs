using Moq;
using ThabeSoft.ProtocolGateway.Configuration.Json;
using ThabeSoft.ProtocolGateway.Configuration.Options;
using ThabeSoft.ProtocolGateway.Infrastructure.Json;

namespace ThabeSoft.ProtocolGateway.Configuration;


[TestClass]
public class ChannelConfigRepositoryTests
{
    public TestContext TestContext { get; set; }


    [DataRow("Default")]
    [DataRow("Custome")]
    [TestMethod(DisplayName = "根据名称查询配置")]
    public async Task FindByNameAsync_ShouldReturnConfig_WhenFileExists(string name)
    {
        // Arrange
        var config = new GatewayConfig { Name = name };
        var tempFile = Path.GetTempFileName();

        var mock_options = new Mock<IConfigOptions>();
        mock_options.Setup(x => x.GetGatewayConfigFilePath(It.IsAny<string>()))
            .Returns(tempFile);

        var serializer = new ConfigJsonSerializer(ConfigJsonSerializerContext.Default);
        await serializer.SerializeToFileAsync(config, tempFile, TestContext.CancellationToken);

        var repository = new ChannelConfigRepository(mock_options.Object, serializer);

        // Act
        var result = await repository.FindBytNameAsync(name, TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(name, result.Value.Name);

        // Cleanup
        File.Delete(tempFile);
    }


    [DataRow("Default")]
    [DataRow("Custome")]
    [TestMethod(DisplayName = "更新配置")]
    public async Task UpdateAsync_ShouldSaveConfigToFile(string name)
    {
        // Arrange
        var tempFile = Path.GetTempFileName();

        var config = new GatewayConfig { Name = name, Channels = [] };

        var mock_options = new Mock<IConfigOptions>();
        mock_options.Setup(x => x.GetGatewayConfigFilePath(It.IsAny<string>()))
            .Returns(tempFile);

        var serializer = new ConfigJsonSerializer(ConfigJsonSerializerContext.Default);
        var repository = new ChannelConfigRepository(mock_options.Object, serializer);

        // Act
        await repository.UpdateAsync(config, TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(File.Exists(tempFile));
        var content = await File.ReadAllTextAsync(tempFile, TestContext.CancellationToken);
        Assert.Contains(name, content);

        // Cleanup
        File.Delete(tempFile);
    }
}
