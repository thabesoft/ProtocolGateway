using Microsoft.Extensions.DependencyInjection.Extensions;
using ThabeSoft.ProtocolGateway.Provisioners;


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
        /// 添加配置业务
        /// </summary>
        public void AddProtocolGatewayConfiguration()
        {
            // 主视图
            services.TryAddSingleton<IChannelProvisioner, ChannelProvisioner>();
        }
    }
}