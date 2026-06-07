using System.Runtime.CompilerServices;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Navigations;


/// <summary>
/// 实现获得导航能力
/// </summary>
public interface INavigatable;


/// <summary>
/// 可导航对象扩展
/// </summary>
public static class NavigatableExtensions
{
    private readonly static ConditionalWeakTable<object, INavigationService> services = [];


    extension<T>(T obj) where T : INavigatable
    {
        /// <summary>
        /// 是否可以导航
        /// </summary>
        public bool CanNavigate => services.TryGetValue(obj, out _);

        /// <summary>
        /// 注册导航服务
        /// </summary>
        public Result RegisterNavigationService(INavigationService service)
        {
            try
            {
                services.AddOrUpdate(obj, service);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error($"通知业务注册失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 尝试导航
        /// </summary>
        public Result TryNavigate(Func<INavigationService, Result> action)
        {
            return obj.GetNavigationService()
                .Then(action, static (x, state) => state.Invoke(x));
        }

        /// <summary>
        /// 尝试导航
        /// </summary>
        public Result TryNavigate<TState>(TState state, Action<INavigationService, TState> action)
        {
            return obj.GetNavigationService()
                .OnValue((action, state), static (x, state) => state.action.Invoke(x, state.state));
        }


        /// <summary>
        /// 尝试导航 并且可以携带一个参数进入委托
        /// </summary>
        public Result TryNavigate<TState>(TState state, Func<INavigationService, TState, Result> action)
        {
            return obj.GetNavigationService()
                .Then((action, state), static (x, state) => state.action.Invoke(x, state.state));
        }

        /// <summary>
        /// 获取导航服务
        /// </summary>
        public Result<INavigationService> GetNavigationService()
        {
            if (!services.TryGetValue(obj, out var service))
            {
                return Result.Error<INavigationService>("无法导航, 导航业务未初始化");
            }

            return Result.Success(service);
        }
    }
}