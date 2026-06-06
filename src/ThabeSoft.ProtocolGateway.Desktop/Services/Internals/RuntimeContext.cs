using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Runtime;

namespace ThabeSoft.ProtocolGateway.Services.Internals;


/// <summary>
/// 运行时上下文
/// </summary>
/// <param name="factory"></param>
internal sealed class RuntimeContext(IGatewayConfigRepository gatewayConfigRepository) : IRuntimeContext, IHostedService
{
    public IRuntimeGateway? Gateway { get; private set; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var result = await gatewayConfigRepository.FindBytNameAsync("Default", cancellationToken);
        if(!result.IsFailure)
        {
            var create_result = RuntimeGateway.Create(result.Value);
            if(!create_result.IsFailure)
            {
                Gateway = create_result.Value;
                return;
            }

            Debug.WriteLine($"网关初始化失败: {create_result.Message}");
        }

        Debug.WriteLine($"运行时上下文初始化失败: {result.Message}");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}