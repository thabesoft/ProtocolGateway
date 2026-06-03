using System.Numerics;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 属性设置构建器
/// </summary>
public interface IPropertySetBuilder<T>
{
    string PropertyName { get; }
    T OldValue { get; }
    T NewValue { get; }

    IPropertySetBuilder<T> AddError(string message);
    IPropertySetBuilder<T> NotifyProperty(string PropertyName);
    IPropertySetBuilder<T> Tap(Action<T> action);

    void Apply();
}

public static class PropertyChangeExtensions
{
    extension<T>(IPropertySetBuilder<T> property)
    {
        public IPropertySetBuilder<T> NotNull(Func<T, string> message)
        {
            if (property.NewValue is null)
            {
                property.AddError(message(property.NewValue));
            }

            return property;
        }

        public IPropertySetBuilder<T> IsError(bool condition, Func<string> message)
        {
            if (condition)
            {
                property.AddError(message());
            }

            return property;
        }

        public IPropertySetBuilder<T> IsSuccess(Func<T, Result> resultGetter)
        {
            var result = resultGetter(property.NewValue);

            if (!result.IsSuccess)
            {
                property.AddError(result.Message!);
            }

            return property;
        }
    }

    extension(IPropertySetBuilder<string> property)
    {
        public IPropertySetBuilder<string> NotNullOrEmpty(Func<string> message)
        {
            if (string.IsNullOrEmpty(property.NewValue))
            {
                property.AddError(message());
            }

            return property;
        }

        public IPropertySetBuilder<string> NotNullOrWhiteSpace(Func<string> message)
        {
            if (string.IsNullOrWhiteSpace(property.NewValue))
            {
                property.AddError(message());
            }

            return property;
        }
    }

    extension<T>(IPropertySetBuilder<T> property) where T : struct, Enum
    {
        public IPropertySetBuilder<T> NotDefined(Func<T, string> message)
        {
            if (!Enum.IsDefined(property.NewValue))
            {
                property.AddError(message(property.NewValue));
            }

            return property;
        }

        public IPropertySetBuilder<T> HasFlag(T flag, Func<T, string> message)
        {
            if (!property.NewValue.HasFlag(flag))
            {
                property.AddError(message(property.NewValue));
            }

            return property;
        }
    }

    extension<T>(IPropertySetBuilder<T> property) where T : struct, INumber<T>
    {
        public IPropertySetBuilder<T> NotInRange(T min, T max, Func<T, string> message)
        {
            if (property.NewValue < min || property.NewValue > max)
            {
                property.AddError(message(property.NewValue));
            }

            return property;
        }
    }
}