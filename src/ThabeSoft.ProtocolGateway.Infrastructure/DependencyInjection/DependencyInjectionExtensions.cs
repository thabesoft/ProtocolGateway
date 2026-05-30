using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThabeSoft.ProtocolGateway.JsonConverters;
using ThabeSoft.ProtocolGateway.Options;
using ThabeSoft.ProtocolGateway.Services;


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
        /// 添加基础层业务
        /// </summary>
        public void AddProtocolGatewayInfrastructure(Action<ConfigOptions> optionsAction)
        {
            // 配置
            services.Configure(optionsAction);

            // Json 序列化
            services.Configure<JsonSerializerOptions>(x =>
            {
                x.Converters.Add(new JsonStringEnumConverter());
                x.Converters.Add(new BaudRateConverter());
                x.Converters.Add(new ChannelNameConverter());
                x.Converters.Add(new FunctionCodeConverter());
            });

            // 视图模型提供者
            services.TryAddSingleton<IViewModelProvider, ViewModelProvider>();
            // 通道配置
            services.AddSingleton<IChannelConfigService, ChannelJsonConfigService>();
        }
    }
}