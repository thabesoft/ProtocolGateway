using ThabeSoft.ProtocolGateway;
using ThabeSoft.ProtocolGateway.Handles;
using ThabeSoft.ProtocolGateway.Handles.Modbus;


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
        public void AddProtocolGatewayConfigurationActivator()
        {
            // 通道处理器工厂
            services.AddSingleton<IChannelHandleFactory, ChannelHandleFactory>();

            // 通道句柄
            services.AddSingleton<IChannelHandleProvider, ModbusChannelHandleProvider>();
        }
    }
}