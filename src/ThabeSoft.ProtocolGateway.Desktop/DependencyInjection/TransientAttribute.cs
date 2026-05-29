#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 瞬态
/// </summary>
public sealed class TransientAttribute<TService>() : DependencyAttribute<TService>(ServiceLifetime.Transient)
    where TService : notnull;
