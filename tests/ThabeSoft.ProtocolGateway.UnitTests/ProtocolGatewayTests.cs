using ThabeSoft.ProtocolGateway.Channels;

namespace ThabeSoft.ProtocolGateway;


[TestClass]
public class ProtocolGatewayTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task FuckAsync()
    {
        IProtocolGateway gateway = default!;
        var chanel = gateway.GetChannel(default!);

        if(chanel is IReadChannel readChannel)
        {
            await readChannel.ReadAsync(default!, default!, TestContext.CancellationToken);
        }
    }
}
