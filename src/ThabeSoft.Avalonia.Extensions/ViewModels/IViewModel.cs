using System.ComponentModel;

namespace ThabeSoft.Avalonia.ViewModels;

/// <summary>
/// 视图模型
/// </summary>
public interface IViewModel : INotifyPropertyChanging, INotifyPropertyChanged, INotifyDataErrorInfo;
