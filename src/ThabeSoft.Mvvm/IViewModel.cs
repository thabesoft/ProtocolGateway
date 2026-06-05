using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ThabeSoft.Mvvm.Internals;

namespace ThabeSoft.Mvvm;


/// <summary>
/// 视图模型
/// </summary>
public interface IViewModel : INotifyPropertyChanged, INotifyPropertyChanging;


/// <summary>
/// 视图模型基类
/// </summary>
public abstract class ViewModel : IViewModel, INotifyDataErrorInfo, IErrorContainer, IPropertyChangeNotifier
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


    #region --通知属性缓存--


    private readonly NotifyPropertyFactorty factorty = new();


    protected INotifyProperty<T> SetProperty<T>(T oldValue, T newValue, [CallerMemberName] string? propertyName = null)
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

        var property = NotifyPropertyFactorty<T>.GetOrCreate(propertyName!, this, (_, state) => new NotifyProperty<T>(state, state));
        property.OnChanged
        var context = new PropertyChangeContext<T>(propertyName!, oldValue, newValue);

        return new NotifyProperty<T>(changeNotifier: this, errorContainer: this);
    }

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

        var property = new NotifyProperty<T>(
            context: new PropertyChangeContext<T>(propertyName!, oldValue, newValue),
            changeNotifier: this,
            errorContainer: this);
        property.OnChanged(x => update(x.NewValue));

        return property;
    }


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



    protected virtual void OnChange<T>(PropertyChangeContext<T> context, Action<T> update)
    {
        try
        {
            if (HasError(context.Name))
            {
                update(context.OldValue);
            }
            else
            {
                update(context.NewValue);
            }
        }
        catch(Exception ex)
        {
            AddError(context.Name, ex.Message);
        }
    }



    
}



internal static class NotifyPropertyFactoryExtensions
{
    extension<TViewModel>(TViewModel viewModel) where TViewModel : IViewModel, IPropertyChangeNotifier, IErrorContainer
    {
        public INotifyProperty<TValue> GetNotifyProperty<TValue>(string name)
        {
            return InternalFactory<TViewModel, TValue>.GetOrCreate(name, viewModel, (_, state) => new NotifyProperty<TValue>(state, state));
        }
    }

    private static class InternalFactory<TOwer, TValue>
    {
        private static readonly object _lock = new();
        private static readonly Dictionary<string, NotifyProperty<TValue>> _properties = [];

        public static NotifyProperty<TValue> GetOrCreate(string name, Func<string, NotifyProperty<TValue>> factory)
        {
            if (_properties.TryGetValue(name, out var property))
            {
                return property;
            }

            lock (_lock)
            {
                if (_properties.TryGetValue(name, out var exists))
                {
                    return exists;
                }

                return _properties[name] = factory(name);
            }
        }

        public static NotifyProperty<TValue> GetOrCreate<TState>(string name, TState state, Func<string, TState, NotifyProperty<TValue>> factory)
        {
            if (_properties.TryGetValue(name, out var property))
            {
                return property;
            }

            lock (_lock)
            {
                if (_properties.TryGetValue(name, out var exists))
                {
                    return exists;
                }

                return _properties[name] = factory(name, state);
            }
        }
    };
}