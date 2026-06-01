using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThabeSoft.ProtocolGateway;
using ThabeSoft.ProtocolGateway.Infrastructure.JsonConverters;
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
        /// 添加协议网关核心业务
        /// </summary>
        public void AddProtocolGateway(Action<ConfigOptions> optionsAction)
        {
            // 默认网关实现
            services.AddSingleton<IGateway, Gateway>();


            // Json序序列化 选项
            services.Configure<JsonSerializerOptions>(x =>
            {
                x.Converters.Add(new JsonStringEnumConverter<ProtocolType>());
                x.Converters.Add(new JsonStringEnumConverter<PortType>());
                x.Converters.Add(new JsonStringEnumConverter<ChannelType>());
                x.Converters.Add(new JsonStringEnumConverter<TagValueType>());

                x.Converters.Add(new ChannelNameConverter());
            });
            services.Configure(optionsAction);


            // 配置加载业务
            services.TryAddSingleton<IChannelConfigService, ChannelConfigService>();
            // 通道句柄工厂
            services.TryAddSingleton<IChannelHandleFactory, ChannelHandleFactory>();
        }
    }
}