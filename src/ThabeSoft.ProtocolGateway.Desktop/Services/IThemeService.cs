//using Avalonia;
//using Avalonia.Controls;
//using Avalonia.Styling;
//using ThabeSoft.Primitives;

//namespace ThabeSoft.ProtocolGateway.Services;


///// <summary>
///// 主题业务
///// </summary>
//public interface IThemeService
//{
//    /// <summary>
//    /// 切换主题
//    /// </summary>
//    Result ChageThme(ThemeVariant variant);

//    /// <summary>
//    /// 切换强调色
//    /// </summary>
//    Result ChangeAccent(AccentType accent);
//}

//internal sealed class ThemeService : IThemeService
//{
//    // 当前激活的强调色主题名称（用于资源查找）
//    private string _currentAccentTheme = "Docker";

//    // 支持的所有强调色主题（可扩展）
//    private readonly Dictionary<AccentType, string> _accentThemeMap = new()
//    {
//        [AccentType.Docker] = "Docker",
//        // 可添加更多：Blue, Purple 等
//    };

//    // 主题变体切换事件（供 ViewModel 订阅刷新）
//    public event Action<ThemeVariant>? ThemeChanged;
//    public event Action<string>? AccentChanged;

//    /// <summary>
//    /// 切换主题变体（Light / Dark）
//    /// </summary>
//    public Result ChangeTheme(ThemeVariant variant)
//    {
//        try
//        {
//            if (Application.Current is not null)
//            {
//                Application.Current.RequestedThemeVariant = variant;
//                ThemeChanged?.Invoke(variant);

//                // 可选：持久化保存用户选择
//                SaveThemePreference(variant);
//            }
//            return Result.Success();
//        }
//        catch (Exception ex)
//        {
//            return Result.Error($"切换主题失败: {ex.Message}");
//        }
//    }

//    /// <summary>
//    /// 切换强调色主题（比如从 Docker 切换到其他）
//    /// </summary>
//    public Result ChangeAccent(AccentType accent)
//    {
//        try
//        {
//            if (!_accentThemeMap.TryGetValue(accent, out var accentKey))
//                return Result.Error($"不支持的强调色类型: {accent}");

//            _currentAccentTheme = accentKey;

//            // 重新加载强调色资源（关键步骤）
//            ApplyAccentResources(accentKey);

//            AccentChanged?.Invoke(accentKey);

//            // 可选：持久化
//            SaveAccentPreference(accent);

//            return Result.Success();
//        }
//        catch (Exception ex)
//        {
//            return Result.Fail($"切换强调色失败: {ex.Message}");
//        }
//    }

//    /// <summary>
//    /// 初始化服务（在 App 启动时调用）
//    /// </summary>
//    public void Initialize()
//    {
//        // 加载用户偏好
//        var savedTheme = LoadThemePreference();
//        if (savedTheme != null)
//            ChangeTheme(savedTheme);
//        else
//            ChangeTheme(ThemeVariant.Default); // 跟随系统

//        var savedAccent = LoadAccentPreference();
//        if (savedAccent.HasValue)
//            ChangeAccent(savedAccent.Value);
//        else
//            ChangeAccent(AccentType.Docker);
//    }

//    // ---------- 资源切换核心逻辑 ----------
//    private void ApplyAccentResources(string accentKey)
//    {
//        if (Application.Current?.Resources.MergedDictionaries is not { } dicts)
//            return;

//        // 假设你的主题资源文件名为 "Themes/Docker.axaml"（包含 Light/Dark 变体）
//        // 需要根据 accentKey 替换强调色部分。更常见的方式是：
//        // 1. 将基础灰度和强调色分开定义
//        // 2. 动态替换强调色资源字典

//        // 简单实现：重新加载整个主题字典（包含灰度和强调色）
//        // 但注意不要重置主题变体。更细粒度的做法是只替换 Accent 颜色。

//        // 示例：找到已加载的主主题字典（包含 ThemeDictionaries）
//        var mainThemeDict = dicts.OfType<ResourceDictionary>()
//            .FirstOrDefault(d => d.ContainsKey("ThemeDictionaries"));

//        if (mainThemeDict == null)
//        {
//            // 如果没有，就新建并添加
//            var newDict = new ResourceDictionary();
//            newDict.MergedDictionaries.Add(new ResourceDictionary
//            {
//                Source = new Uri($"avares://YourAssembly/Themes/{accentKey}.axaml", UriKind.Absolute)
//            });
//            dicts.Add(newDict);
//        }
//        else
//        {
//            // 替换现有的主题字典源
//            // 更简单：移除旧的，添加新的
//            var oldAccentDict = dicts.FirstOrDefault(d => d.Source?.OriginalString.Contains("Themes/") == true);
//            if (oldAccentDict != null)
//                dicts.Remove(oldAccentDict);

//            var newAccentDict = new ResourceDictionary
//            {
//                Source = new Uri($"avares://YourAssembly/Themes/{accentKey}.axaml", UriKind.Absolute)
//            };
//            dicts.Add(newAccentDict);
//        }
//    }

//    // ---------- 持久化（示例） ----------
//    private void SaveThemePreference(ThemeVariant variant)
//    {
//        // 存入本地设置，例如使用 Application.Current.StorageProvider
//        var value = variant == ThemeVariant.Light ? "Light" : variant == ThemeVariant.Dark ? "Dark" : "Default";
//        // 存储逻辑...
//    }

//    private ThemeVariant? LoadThemePreference()
//    {
//        // 读取存储，返回 ThemeVariant.Light / Dark / null（表示跟随系统）
//        return null;
//    }

//    private void SaveAccentPreference(AccentType accent)
//    {
//        // 存储 accent.ToString()
//    }

//    private AccentType? LoadAccentPreference()
//    {
//        // 读取存储，返回 AccentType 或 null
//        return null;
//    }
//}