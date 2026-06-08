using Avalonia;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Extensions;

public static class ApplicationExtensions
{
    extension(Application)
    {
        public static Result<Application> GetCurrentApplication()
        {
            if (Application.Current is null) return Result.Error<Application>("切换强调色失败, 无法获取应用实例");
            return Result.Success(Application.Current);
        }
    }
}
