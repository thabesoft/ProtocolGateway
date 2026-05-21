using ThabeSoft.ProtocolGateway.Channels;

namespace ThabeSoft.ProtocolGateway;


[TestClass]
public class ProtocolGatewayTests
{
    [TestMethod]
    public async Task FuckAsync()
    {
        IChannel gateway = default!;

        var result = await gateway.ReadValueAsync<int>(default!, TestContext.CancellationToken);
        if (result.IsSuccess) Console.WriteLine(result.Value);
    }

    public TestContext TestContext { get; set; }
}
