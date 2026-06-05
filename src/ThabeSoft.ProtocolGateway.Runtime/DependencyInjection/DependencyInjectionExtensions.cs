using ThabeSoft.ProtocolGateway;
using ThabeSoft.ProtocolGateway.Runtime;
using ThabeSoft.ProtocolGateway.Runtime.Factories;


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
        /// 添加运行时网关
        /// </summary>
        public void AddRuntimeGateway()
        {
            // 通道处理器工厂
            services.AddSingleton<IRuntimeChannelFactory, RuntimeChannelFactory>();
            // 通道句柄
            services.AddSingleton<IRuntimeChannelProvider, RuntimeModbusChannelProvider>();


            // 运行时网关
            services.AddSingleton<IRuntimeGateway, RuntimeGateway>();
            services.AddSingleton<IRuntimeGatewayFactory, RuntimeGatewayFactory>();
        }
    }
}