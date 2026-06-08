using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Platform;
using Avalonia.Styling;
using System.Resources;
using ThabeSoft.Avalonia.Extensions;
using ThabeSoft.Avalonia.Themes;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 主题业务
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// 切换主题
    /// </summary>
    Result ChangeTheme(ThemeVariant variant);

    /// <summary>
    /// 切换强调色
    /// </summary>
    Result ChangeAccent(AccentVariant accent);

    /// <summary>
    /// 注册主题
    /// </summary>
    Result RegisterAccent(AccentVariant name, IResourceProvider accentResourceProvider);
}


public static class ThemeServiceExtensions
{
    extension(IThemeService themeService)
    {
        public void RegisterAccentIncludeResource(AccentVariant name, Uri uri)
        {
            themeService.RegisterAccent(name, new ResourceInclude(baseUri: null) { Source = uri });
        }
    }
}




internal sealed class ThemeService : IThemeService
{
    private readonly object _lock = new();
    private IResourceProvider? _currentAccentResourceProvider;
    private readonly Dictionary<AccentVariant, IResourceProvider> _providers = [];

    public Result ChangeTheme(ThemeVariant variant)
    {
        lock (_lock)
        {
            return Application.GetCurrentApplication()
                    .OnValue(variant, (app, variant) => app.RequestedThemeVariant = variant);
        }
    }

    public Result RegisterAccent(AccentVariant name, IResourceProvider accentResourceProvider)
    {
        lock (_lock)
        {
            if (_providers.ContainsKey(name)) return Result.Error($"强调色注册失败, 已存在相同名称 [{name}]");
            _providers[name] = accentResourceProvider;
            
            return Result.Success();
        }
    }

    public Result ChangeAccent(AccentVariant accent)
    {
        lock (_lock)
        {
            try
            {
                // 获取资源
                if (Application.Current is null) return Result.Error("切换强调色失败, 无法获取应用实例");
                if (!_providers.TryGetValue(accent, out var providerFactory)) return Result.Error($"强调色切换失败, 未知的强调色 [{accent}]");

                // 替换新旧
                var old_resource = _currentAccentResourceProvider;
                var new_resource = providerFactory;

                if (old_resource is not null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(old_resource);
                }
                Application.Current.Resources.MergedDictionaries.Add(new_resource);
                _currentAccentResourceProvider = new_resource;

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error($"切换强调色失败: {ex.Message}");
            }
        }
    }
}