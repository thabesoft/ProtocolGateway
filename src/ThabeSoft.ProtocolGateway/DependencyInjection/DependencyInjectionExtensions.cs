using ThabeSoft.ProtocolGateway;

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
        public void AddProtocolGateway()
        {
            services.AddSingleton<IGateway, Gateway>();
        }
    }
}