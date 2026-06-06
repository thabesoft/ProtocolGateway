using System.Runtime.CompilerServices;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Services;
using ThabeSoft.ProtocolGateway.ViewModels;

namespace ThabeSoft.ProtocolGateway.Extensions;

/// <summary>
/// 视图模型导航业务扩展
/// </summary>
public static class ViewModelNavigationServiceExtensions
{
    private readonly static ConditionalWeakTable<object, INavigationService> services = [];


    extension<T>(T viewModel) where T : IViewModel
    {
        /// <summary>
        /// 是否可以导航
        /// </summary>
        public bool CanNavigate => services.TryGetValue(viewModel, out _);

        /// <summary>
        /// 注册导航服务
        /// </summary>
        public Result RegisterNavigationService(INavigationService service)
        {
            try
            {
                services.AddOrUpdate(viewModel, service);
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
            return viewModel.GetNavigationService()
                .Select(action, static (x, state) => state.Invoke(x));
        }

        /// <summary>
        /// 尝试导航 并且可以携带一个参数进入委托
        /// </summary>
        public Result TryNavigate<TState>(TState state, Func<INavigationService, TState, Result> action)
        {
            return viewModel.GetNavigationService()
                .Select((action, state), static (x, state) => state.action.Invoke(x, state.state));
        }

        /// <summary>
        /// 获取导航服务
        /// </summary>
        public Result<INavigationService> GetNavigationService()
        {
            if (!services.TryGetValue(viewModel, out var service))
            {
                return Result.Error<INavigationService>("无法导航, 导航业务未初始化");
            }

            return Result.Success(service);
        }
    }
}