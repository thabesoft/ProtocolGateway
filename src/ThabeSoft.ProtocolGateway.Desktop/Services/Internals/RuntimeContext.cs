using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Runtime;

namespace ThabeSoft.ProtocolGateway.Services.Internals;


/// <summary>
/// 运行时上下文
/// </summary>
internal sealed class RuntimeContext(IGatewayConfigRepository gatewayConfigRepository) : IRuntimeContext, IHostedService
{
    public IRuntimeGateway? Gateway { get; private set; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var result = await gatewayConfigRepository.FindBytNameAsync("Default", cancellationToken);

        result.Bind(RuntimeGateway.Create)
            .Tap(this, static (x, state) => state.Gateway = x)
            .TapError(static  (_, msg) => Debug.WriteLine($"网关初始化失败: {msg}"));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}