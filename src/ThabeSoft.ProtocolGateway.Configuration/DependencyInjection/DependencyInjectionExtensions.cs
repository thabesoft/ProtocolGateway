using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Configuration.Json;
using ThabeSoft.ProtocolGateway.Configuration.Options;
using ThabeSoft.ProtocolGateway.Infrastructure.Json;


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
        public void AddGatewayConfiguration(Action<IConfigOptions> optionsAction)
        {
            // 配置选项
            services.Configure<ConfigOptions>(optionsAction);

            // 配置Json序列化
            services.AddSingleton(_ => ConfigJsonSerializerContext.Default);
            services.AddSingleton<ConfigJsonSerializer>();

            // 配置仓储
            services.AddSingleton<IGatewayConfigRepository, ChannelConfigRepository>();
        }
    }
}