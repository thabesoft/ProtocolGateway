using ThabeSoft.Primitives;

namespace ThabeSoft.Mvvm;

/// <summary>
/// 通知属性扩展
/// </summary>
public static class NotifyPropertyExtensions
{
    extension<T>(INotifyProperty<T> property)
    {
        /// <summary>
        /// 附带通知属性
        /// </summary>
        public INotifyProperty<T> AlsoNotify(string propertyName)
        {
            property.OnChanged((_, _, _) => property.Notifier.OnPropertyChanged(propertyName));
            return property;
        }
        public INotifyProperty<T> AlsoNotify(params string[] propertyNames)
        {
            if (propertyNames.Length == 0) return property;

            property.OnChanged((_, _, _) =>
            {
                foreach (var name in propertyNames)
                {
                    property.Notifier.OnPropertyChanged(name);
                }
            });
            return property;
        }


        public INotifyProperty<T> Changing(NotifyPropertyChangeHandler<T> handler)
        {
            property.OnChanging(handler);
            return property;
        }
        public INotifyProperty<T> Changed(NotifyPropertyChangeHandler<T> handler)
        {
            property.OnChanged(handler);
            return property;
        }
        public INotifyProperty<T> Validate(NotifyPropertyValidateHandler<T> handler)
        {
            property.OnValidate(handler);
            return property;
        }


        public INotifyProperty<T> Tap(Action<T> handler)
        {
            property.OnChanged((_, _, v) => handler(v));
            return property;
        }
    }


    extension<T>(INotifyProperty<T> property)
    {
        public INotifyProperty<T> NotNull(Func<string>? errorMessage = null)
        {
            property.OnValidate((_, value) =>
            {
                if (value is not null) return Result.Success();
                return Result.Error(errorMessage?.Invoke() ?? "不可为空");
            });

            return property;
        }

        public INotifyProperty<T> Must(bool condition, Func<T, string>? errorMessage = null)
        {
            property.OnValidate((_, value) =>
            {
                if (condition) return Result.Success();
                return Result.Error(errorMessage?.Invoke(value) ?? "验证失败");
            });

            return property;
        }
        public INotifyProperty<T> IsSuccess(Func<T, Result> result)
        {
            property.OnValidate((_, value) => result(value));

            return property;
        }
    }

    extension<T>(INotifyProperty<IEnumerable<T>> property)
    {
        public INotifyProperty<IEnumerable<T>> NotNullOrEmpty(Func<string>? errorMessage = null)
        {
            property.OnValidate((_, value) =>
            {
                if (value is not null && !value.Any()) return Result.Success();
                return Result.Error(errorMessage?.Invoke() ?? "集合不可为空");
            });

            return property;
        }
    }

    extension(INotifyProperty<string?> property)
    {
        public INotifyProperty<string?> NotNullOrEmpty(Func<string>? errorMessage = null)
        {
            property.OnValidate((_, value) =>
            {
                if (!string.IsNullOrEmpty(value)) return Result.Success();
                return Result.Error(errorMessage?.Invoke() ?? "不可为空或空白");
            });

            return property;
        }

        public INotifyProperty<string?> NotNullOrWhiteSpace(Func<string>? errorMessage = null)
        {
            property.OnValidate((_, value) =>
            {
                if (!string.IsNullOrWhiteSpace(value)) return Result.Success();
                return Result.Error(errorMessage?.Invoke() ?? "不可为空或空白字符");
            });

            return property;
        }
    }

    extension(INotifyProperty<string> property)
    {
        public INotifyProperty<string> NotEmpty(Func<string>? errorMessage = null)
        {
            property.OnValidate((_, value) =>
            {
                if (!string.IsNullOrEmpty(value)) return Result.Success();
                return Result.Error(errorMessage?.Invoke() ?? "不可为空白");
            });

            return property;
        }

        public INotifyProperty<string> NotWhiteSpace(Func<string>? errorMessage = null)
        {
            property.OnValidate((_, value) =>
            {
                if (!string.IsNullOrWhiteSpace(value)) return Result.Success();
                return Result.Error(errorMessage?.Invoke() ?? "不可为空白字符");
            });

            return property;
        }
    }

    extension<T>(INotifyProperty<T> property) where T : struct, Enum
    {
        public INotifyProperty<T> IsDefined(Func<T, string>? messageGetter = null)
        {
            property.OnValidate((_, value) =>
            {
#if NET8_0_OR_GREATER
                if (!Enum.IsDefined(value))
                {
                    return Result.Error(messageGetter?.Invoke(value) ?? $"不支持的值 [{value}]");
                }
#else
                if (!Enum.IsDefined(typeof(T), value))
                {
                    return Result.Error(messageGetter?.Invoke(value) ?? $"不支持的值 [{value}]");
                }
#endif
                return Result.Success();
            });

            return property;
        }

        public INotifyProperty<T> HasFlag(T flag, Func<T, string>? messageGetter = null)
        {
            property.OnValidate((_, value) =>
            {
                if (value.HasFlag(flag)) return Result.Success();
                return Result.Error(messageGetter?.Invoke(value) ?? $"不支持该值 [{flag}]");
            });

            return property;
        }
    }

#if NET8_0_OR_GREATER
    extension<T>(INotifyProperty<T> property) where T : struct, System.Numerics.INumber<T>
    {
        public INotifyProperty<T> InRange(T min, T max, Func<T, string>? messageGetter = null)
        {
            property.OnValidate((_, value) =>
            {
                if (value >= min && value <= max) return Result.Success();
                return Result.Error(messageGetter?.Invoke(value) ?? $"范围必须在 [{min}-{max}] 之间");
            });

            return property;
        }
    }
#endif
}