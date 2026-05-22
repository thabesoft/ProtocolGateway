using ThabeSoft.ProtocolGateway.Channels;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway;


[TestClass]
public class ProtocolGatewayTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public async Task FuckAsync()
    {
        Word world = Word.FromUInt(10);
        if(world.ToInt16() > 10)
        {

        }



        IProtocolGateway gateway = default!;
        var chanel = gateway.GetChannel(default!);

        if(chanel is IReadChannel readChannel)
        {
            await readChannel.ReadAsync(default!, default!, TestContext.CancellationToken);
        }
    }
}
