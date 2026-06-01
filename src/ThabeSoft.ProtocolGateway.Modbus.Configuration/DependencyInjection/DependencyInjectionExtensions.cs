using System.Text.Json;
using ThabeSoft.ProtocolGateway.JsonConverters;
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
        public void AddProtocolGatewayModbusConfigure()
        {
            // Json 序列化
            services.Configure<JsonSerializerOptions>(x =>
            {

                // SerialPortConfig : IPortConfig

                x.Converters.Add(new BaudRateConverter());
                x.Converters.Add(new FunctionCodeConverter());
            });

            // 通道句柄
            services.AddSingleton<IChannelHandleProvider, ChannelHandleProvider>();
        }
    }
}