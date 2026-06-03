using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ThabeSoft.Mvvm.Internal;

namespace ThabeSoft.Mvvm;

/// <summary>
/// 视图模型
/// </summary>
public interface IViewModel : INotifyPropertyChanged, INotifyPropertyChanging;


/// <summary>
/// 视图模型基类
/// </summary>
public abstract class ViewModel : INotifyDataErrorInfo, IErrorContainer, IPropertyChangeNotifier, IViewModel
{
    #region --属性通知--

    /// <summary>
    /// 属性正在改变
    /// </summary>
    public event PropertyChangingEventHandler? PropertyChanging;
    /// <summary>
    /// 属性已改变
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// 属性改变中通知
    /// </summary>
    protected void OnPropertyChanging(string propertyName)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }
    /// <summary>
    /// 属性已改变通知
    /// </summary>
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    void IPropertyChangeNotifier.OnPropertyChanging(string propertyName) => OnPropertyChanging(propertyName);
    void IPropertyChangeNotifier.OnPropertyChanged(string propertyName) => OnPropertyChanged(propertyName);

    #endregion

    #region --错误--

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


    void IErrorContainer.AddError(string propertyName, string message) => AddError(propertyName, message);
    bool IErrorContainer.HasError(string propertyName) => HasError(propertyName);
    void IErrorContainer.ClearError(string propertyName) => ClearError(propertyName);


    #endregion


    #region --更新验证--
    protected INotifyProperty<T> Change<T>(T oldValue, T newValue, Action<T> update, [CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName), "属性名不可为空");
        }

        ClearError(propertyName!);

        if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
        {
            return EmptyNotifyProperty<T>.Empty;
        }

        return new NotifyProperty<T>(
            propertyName: propertyName!,
            oldValue: oldValue,
            newValue: newValue,
            errorContainer: this,
            changeNotifier: this,
            changeHandler: (name, _, @new) => Update(name, oldValue, @new, update));
    }
    protected void Apply<T>(T oldValue, T newValue, Action<T> update, [CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName), "属性名不可为空");
        }
        ClearError(propertyName!);

        if (!EqualityComparer<T>.Default.Equals(oldValue, newValue))
        {
            OnPropertyChanging(propertyName!);
            Update(propertyName!, oldValue, newValue, update);
            OnPropertyChanged(propertyName!);
        }
    }

    #endregion



    protected virtual void Update<T>(string propertyName, T oldValue, T newValue, Action<T> update)
    {
        try
        {
            if (HasError(propertyName))
            {
                update(oldValue);
            }
            else
            {
                update(newValue);
            }
        }
        catch(Exception ex)
        {
            AddError(propertyName, ex.Message);
        }
    }
}