using Avalonia.Controls.ApplicationLifetimes;

namespace ThabeSoft.ProtocolGateway.Services.Application;

/// <summary>
/// 应用生命周期提供者
/// </summary>
public interface IApplicationLifetimeAccessor
{
    /// <summary>
    /// 生命周期
    /// </summary>
    IApplicationLifetime? ApplicationLifetime { get; }

    /// <summary>
    /// 关闭应用
    /// </summary>
    Task ShutdownAsync();
}