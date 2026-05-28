using Microsoft.Extensions.Hosting;

namespace ThabeSoft.ProtocolGateway.Desktop.Services.Hosted;


/// <summary>
/// 资源初始化
/// </summary>
internal sealed class ResourceInitialization(IIconRegistry iconRegistry) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        //iconRegistry.AddIcon()

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
