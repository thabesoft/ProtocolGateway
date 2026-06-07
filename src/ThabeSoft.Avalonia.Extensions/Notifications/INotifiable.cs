using System.Runtime.CompilerServices;
using ThabeSoft.Primitives;

namespace ThabeSoft.Avalonia.Notifications;


/// <summary>
/// 可以通知的
/// </summary>
public interface INotifiable;


/// <summary>
/// 可通知对象扩展
/// </summary>
public static class NotifiableExtensions
{
    private readonly static ConditionalWeakTable<object, INotificationService> services = [];


    extension<T>(T obj) where T : INotifiable
    {
        /// <summary>
        /// 是否可以通知
        /// </summary>
        public bool CanNotify => services.TryGetValue(obj, out _);

        /// <summary>
        /// 注册导航业务
        /// </summary>
        public Result RegisterNotificationService(INotificationService service)
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
        /// 尝试通知
        /// </summary>
        public Result TryNotify(Func<INotificationService, INotificationOptions> action)
        {
            return obj.GetNotificationService()
                .OnValue(action, (x, state) => state.Invoke(x).Show());
        }

        /// <summary>
        /// 尝试通知 并且可以携带一个参数进入委托
        /// </summary>
        public Result TryNotify<TState>(TState state, Func<INotificationService, TState, INotificationOptions> action)
        {
            return obj.GetNotificationService()
                .OnValue((action, state), (x, state) => state.action.Invoke(x, state.state).Show());
        }

        /// <summary>
        /// 获取通知服务
        /// </summary>
        public Result<INotificationService> GetNotificationService()
        {
            if (!services.TryGetValue(obj, out var service))
            {
                return Result.Error<INotificationService>("无法通知, 通知业务未初始化");
            }

            return Result.Success(service);
        }
    }
}
