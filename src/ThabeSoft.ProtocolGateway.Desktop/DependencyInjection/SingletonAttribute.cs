#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 单例
/// </summary>
public sealed class SingletonAttribute<TService>() : DependencyAttribute<TService>(ServiceLifetime.Singleton)
    where TService : notnull;
