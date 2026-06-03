using System.Text.Json;
using System.Text.Json.Serialization;
using ThabeSoft.ProtocolGateway;
using ThabeSoft.ProtocolGateway.JsonConverters;
using ThabeSoft.ProtocolGateway.Options;


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

            // Json 序列化
            services.Configure<JsonSerializerOptions>(x =>
            {
                x.Converters.Add(new BaudRateConverter());
                x.Converters.Add(new FunctionCodeConverter());
                x.Converters.Add(new ChannelNameConverter());
                x.Converters.Add(new JsonStringEnumConverter<ChannelType>());
                x.Converters.Add(new JsonStringEnumConverter<PortType>());
                x.Converters.Add(new JsonStringEnumConverter<ProtocolType>());
            });

            // 通道句柄
            services.AddSingleton<IChannelConfigRepository, ChannelConfigRepository>();
        }
    }
}