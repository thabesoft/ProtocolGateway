#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


/// <summary>
/// 依赖项
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DependencyAttribute<TService>(ServiceLifetime lifetime) : Attribute
    where TService : notnull
{
    public ServiceLifetime Lifetime => lifetime;
    public object? Key { get; init; }
}
