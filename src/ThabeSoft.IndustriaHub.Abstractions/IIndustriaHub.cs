//using Microsoft.Extensions.Hosting;

//namespace ThabeSoft.IndustriaHub;

//public interface IIndustriaHub
//{
//    IDisposable Subscribe<T>(string tag, Action<Response<T>> callback)
//        where T : struct;

//    ValueTask<Response<T>> ReadAsync<T>(string tag, CancellationToken cancellationToken = default)
//        where T : struct;
//    ValueTask WriteAsync<T>(string tag, T value, CancellationToken cancellationToken = default)
//        where T : struct;
//}


//public readonly record struct Response<T> where T : struct
//{
//    public required T Value { get; init; }

    
//}



//public class IndustriaHub() : IIndustriaHub, IHostedService
//{
//    public Task StartAsync(CancellationToken cancellationToken)
//    {
//        // 获取所有设备

//        // 设备轮询



//        return Task.CompletedTask;
//    }

//    public Task StopAsync(CancellationToken cancellationToken)
//    {
//        return Task.CompletedTask;
//    }
//}