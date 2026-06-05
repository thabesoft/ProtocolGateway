using System.ComponentModel;

namespace ThabeSoft.ProtocolGateway.ViewModels;

/// <summary>
/// 视图模型
/// </summary>
public interface IViewModel : INotifyPropertyChanging, INotifyPropertyChanged, INotifyDataErrorInfo;
