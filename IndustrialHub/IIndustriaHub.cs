using IndustrialHub.Modbus;

namespace ThabeSoft.IndustrialHub;


public interface IIndustriaHub
{
    void AddDriver();

    ITag<T> SetTag<T>(string name) where T : struct;
    ITag<T> GetTag<T>(string name) where T : struct;
}


public interface ITag<T> : IObservable<Response<T>> where T : struct
{
    Task<Response<T>> ReadAsync(CancellationToken cancellationToken = default);
    Task WriteAsync(T value, PriorityType priority = PriorityType.Immediate, CancellationToken cancellationToken = default);
}

public interface IDriver
{
    Task<Response<T>> ReadAsync<T>(string address) where T : struct;
    Task WriteAsync<T>(string address, T value) where T : struct;
}

readonly record struct A
{

}

public readonly record struct Response<T> where T : struct
{
    public  T Value { get; }
    public  StatusCode Status { get; }
    public DateTime Timestamp { get; }

    public bool IsSuccess => Status == StatusCode.Good;


    public Response(T value)
    {
        Value = value;
        Status = StatusCode.Good;
        Timestamp = DateTime.UtcNow;
    }
    public Response(T value, StatusCode status)
    {
        Value = value;
        Status = status;
        Timestamp = DateTime.UtcNow;
    }
    public Response(T value, StatusCode status, DateTime timestamp)
    {
        Value = value;
        Status = status;
        Timestamp = timestamp;
    }
}


public enum StatusCode
{
    Good = 0,         // 数据正常
    Uncertain = 1,    // 不确定（例如设备正在初始化，或者值超限）
    Bad = 2,          // 通讯故障（掉线、超时）
    NotFound = 3,     // 标签不存在
    AccessDenied = 4  // 权限不足（写操作常见）
}