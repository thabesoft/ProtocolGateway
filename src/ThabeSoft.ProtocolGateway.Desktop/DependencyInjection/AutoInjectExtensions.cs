using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配


public static class AutoInjectExtensions
{
    extension(IServiceCollection service)
    {
        public void AddMemberInjectService<
            TServices,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllFields | DynamicallyAccessedMemberTypes.AllProperties)] TImplementation>
            (ServiceLifetime serviceLifetime)
            where TImplementation : notnull, new()
        {
            var descriptor = ServiceDescriptor.Describe(typeof(TServices), sp => CreateInstance<TImplementation>(sp), serviceLifetime);
            service.Add(descriptor);
        }
    }


    private delegate void SetValueDelegate(object instance, object value);
    private record struct InjectMemberInfo(Type MemberType, InjectAttribute Info, SetValueDelegate Handler);

    private static readonly ConcurrentDictionary<Type, InjectMemberInfo[]> _injectMemberInfos = [];

    private static T CreateInstance<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllProperties | DynamicallyAccessedMemberTypes.AllFields)] T>(IServiceProvider provider)
        where T : new()
    {
        var type = typeof(T);
        var instance = new T();

        foreach (var i in _injectMemberInfos.GetOrAdd(type, x => [.. GetInjectMembers(type)]))
        {
            object services;

            if (i.Info.Key is not null)
            {
                services = provider.GetRequiredKeyedService(i.MemberType, i.Info.Key);
            }
            else
            {
                services = provider.GetRequiredService(i.MemberType);
            }

            i.Handler.Invoke(instance, services);
        }

        return instance;

        // 获取注入成员
        static IEnumerable<InjectMemberInfo> GetInjectMembers([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllProperties | DynamicallyAccessedMemberTypes.AllFields)] Type type)
        {
            foreach (var i in type.GetProperties())
            {
                var att = i.GetCustomAttribute<InjectAttribute>();
                if (att is null) continue;

                if (i is PropertyInfo property)
                {
                    yield return new(property.PropertyType, att, property.SetValue);
                }
            }

            foreach (var i in type.GetFields())
            {
                var att = i.GetCustomAttribute<InjectAttribute>();
                if (att is null) continue;

                if (i is FieldInfo field)
                {
                    yield return new(field.FieldType, att, field.SetValue);
                }
            }
        }
    }
}
