namespace ThabeSoft.Primitives;

// Bind
public static partial class ResultExtensions
{
    extension(Result result)
    {
        public static Result Success() => new(ResultLevel.Success, null);
        public static Result Info(string message) => new(ResultLevel.Info, message);
        public static Result Warning(string message) => new(ResultLevel.Warning, message);
        public static Result Error(string message) => new(ResultLevel.Error, message);



        public static Result<TValue> Success<TValue>(TValue value) where TValue : notnull
            => new(ResultLevel.Success, null, value);

        public static Result<TValue> Info<TValue>(string message) where TValue : notnull
            => new(ResultLevel.Info, message);
        public static Result<TValue> Info<TValue>(string message, TValue value) where TValue : notnull
            => new(ResultLevel.Info, message, value);

        public static Result<TValue> Warning<TValue>(string message) where TValue : notnull
            => new(ResultLevel.Warning, message);
        public static Result<TValue> Warning<TValue>(string message, TValue value) where TValue : notnull
            => new(ResultLevel.Warning, message, value);

        public static Result<TValue> Error<TValue>(string message) where TValue : notnull
            => new(ResultLevel.Error, message);
        public static Result<TValue> Error<TValue>(string message, TValue value) where TValue : notnull
            => new(ResultLevel.Error, message, value);
    }
}