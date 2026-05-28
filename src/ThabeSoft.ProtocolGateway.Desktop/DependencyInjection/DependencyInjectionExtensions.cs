using Microsoft.Extensions.DependencyInjection.Extensions;
using ThabeSoft.ProtocolGateway.Desktop.Services;
using ThabeSoft.ProtocolGateway.Desktop.Services.Hosted;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


/// <summary>
/// 微软Ioc注入扩展
/// </summary>
public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// 添加桌面业务
        /// </summary>
        public void AddProtocolGatewayDesktop()
        {
            // 图标
            services.TryAddSingleton<IconService>();
            services.TryAddSingleton<IIconService>(x => x.GetRequiredService<IconService>());
            services.TryAddSingleton<IIconRegistry>(x => x.GetRequiredService<IconService>());
            services.TryAddSingleton<IIconProvider>(x => x.GetRequiredService<IconService>());

            // 资源初始化
            services.AddHostedService<ResourceInitialization>();
        }
    }
}
