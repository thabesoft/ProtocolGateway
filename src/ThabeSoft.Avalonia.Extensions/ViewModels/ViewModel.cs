using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.ComponentModel;

namespace ThabeSoft.Avalonia.ViewModels;


/// <summary>
/// 视图模型
/// </summary>
public abstract class ViewModel : ObservableObject, IViewModel
{
    // 所有错误
    private readonly Dictionary<string, List<string>> _results = [with(StringComparer.OrdinalIgnoreCase)];

    /// <summary>
    /// 是否有错误
    /// </summary>
    public bool HasErrors => _results.Sum(x => x.Value.Count) != 0;
    /// <summary>
    /// 错误已改变
    /// </summary>
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    /// <summary>
    /// 获取所有错误
    /// </summary>
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return Array.Empty<string>();
        }

        if (!_results.TryGetValue(propertyName!, out var errors))
        {
            return Array.Empty<string>();
        }

        return errors;
    }


    /// <summary>
    /// 清空指定属性的错误
    /// </summary>
    protected void ClearError(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName)) return;

        _results.Remove(propertyName);
        OnErrorsChanged(propertyName);
    }
    /// <summary>
    /// 给指定属性添加错误
    /// </summary>
    protected void AddError(string propertyName, string errorMessage)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return;
        }

        if (!_results.TryGetValue(propertyName, out var errors))
        {
            errors = [];
            _results[propertyName] = errors;
        }

        errors.Add(errorMessage);
        OnErrorsChanged(propertyName);
    }
    /// <summary>
    /// 属性是否有错误
    /// </summary>
    protected bool HasError(string propertyName)
    {
        if (!_results.TryGetValue(propertyName, out var results))
        {
            return false;
        }

        return results.Count != 0;
    }
    /// <summary>
    /// 错误改变通知
    /// </summary>
    protected void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}