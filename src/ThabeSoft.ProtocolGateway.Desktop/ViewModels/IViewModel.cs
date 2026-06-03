using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 视图模型
/// </summary>
public interface IViewModel : INotifyPropertyChanged;



public abstract class ValidatableObservableObject : ObservableObject, INotifyDataErrorInfo, IViewModel
{
    private readonly Dictionary<string, List<ValidationResult>> results = [with(StringComparer.OrdinalIgnoreCase)];

    public bool HasErrors => results.Count != 0;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return Array.Empty<ValidationResult>();
        }
        if (!results.TryGetValue(propertyName, out var errors))
        {
            return Array.Empty<ValidationResult>();
        }

        return errors;
    }

    protected void ClearError([CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrEmpty(propertyName)) return;

        results.Remove(propertyName);
        OnErrorsChanged(propertyName);
    }

    protected void AddError(string errorMessage, [CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return;
        }

        if (!results.TryGetValue(propertyName, out var errors))
        {
            errors = [];
            results[propertyName] = errors;
        }

        errors.Add(new ValidationResult(errorMessage, [propertyName]));
        OnErrorsChanged(propertyName);
    }


    protected void OnErrorsChanged([CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrEmpty(propertyName)) return;
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}