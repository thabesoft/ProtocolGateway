using Microsoft.Extensions.Hosting;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services.Internals;


/// <summary>
/// 运行时上下文
/// </summary>
/// <param name="factory"></param>
internal sealed class RuntimeContext(IRuntimeGatewayFactory factory) : IRuntimeContext, IHostedService
{
    public IRuntimeGateway? RuntimeGateway { get; private set; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var result = await factory.CreateAsync("Default");
        RuntimeGateway = result.GetValueOrDefault();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}