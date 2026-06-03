using Avalonia.Threading;
using ThabeSoft.Mvvm;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 视图模型
/// </summary>
public abstract class ViewModelBase : ViewModel
{
    protected virtual DispatcherPriority DispatcherPriority { get; } = DispatcherPriority.Normal;


    protected override void Update<T>(string propertyName, T oldValue, T newValue, Action<T> update)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            base.Update(propertyName, oldValue, newValue, update);
        }
        else
        {
            Dispatcher.UIThread.Post(() => base.Update(propertyName, oldValue, newValue, update), DispatcherPriority);
        }
    }
}