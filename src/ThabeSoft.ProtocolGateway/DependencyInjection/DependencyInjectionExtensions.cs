using Microsoft.Extensions.DependencyInjection;

namespace ThabeSoft.ProtocolGateway.DependencyInjection;


/// <summary>
/// 微软Ioc注入扩展
/// </summary>
public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddProtocolGateway()
        {
            services.AddSingleton<IGateway, Gateway>();
        }
    }
}
