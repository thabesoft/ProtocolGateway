using ThabeSoft.ProtocolGateway.Configuration.Options;
using ThabeSoft.ProtocolGateway.Configuration.Repositories;
using ThabeSoft.ProtocolGateway.Infrastructure.Json;
using ThabeSoft.ProtocolGateway.Infrastructure.Repositories;


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
        /// 添加协议网关配置相关服务
        /// </summary>
        public void AddProtocolGatewayConfiguration(Action<ConfigOptions> optionsAction)
        {
            // 配置选项
            services.Configure(optionsAction);

            // 配置Json序列化
            services.AddSingleton<ConfigJsonSerializerContext>();
            services.AddSingleton<ConfigJsonSerializer>();

            // 配置仓储
            services.AddSingleton<IChannelRepository, ChannelConfigRepository>();
        }
    }
}