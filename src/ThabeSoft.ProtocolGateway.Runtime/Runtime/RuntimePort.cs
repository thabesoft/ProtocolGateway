using ThabeSoft.Lifecycle;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime;


/// <summary>
/// 运行时端口
/// </summary>
public sealed class RuntimePort : LifecycleObject, IRuntimePort
{
    // 传输层
    internal ITransport Transport { get; init; } = default!;
    // 配置
    public ITransportConfig Config { get; internal set; } = default!;


    // 私有构造
    private RuntimePort() { }


    // 读取
    public ValueTask<Result> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        => Transport.ReadExactAsync(buffer, cancellationToken);
    // 写入
    public ValueTask<Result> WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
        => Transport.WriteAsync(data, cancellationToken);

    // 启动
    protected override ValueTask<Result> StartProcessAsync(CancellationToken cancellationToken = default)
        => Transport.StartAsync(cancellationToken);
    // 停止
    protected override ValueTask<Result> StopProcessAsync(CancellationToken cancellationToken = default)
        => Transport.StopAsync(cancellationToken);



    #region --工厂--

    /// <summary>
    /// 根据配置自动创建
    /// </summary>
    public static Result<RuntimePort> Create(IPortConfig config)
    {
        if (config is ISerialPortConfig serialPort)
        {
            return CreateSerialPort(serialPort);
        }

        return Result.Error<RuntimePort>($"无法创建端口, 不支持的配置类型 [{config.Type}]");
    }

    /// <summary>
    /// 创建运行时串口端口
    /// </summary>
    public static Result<RuntimePort> CreateSerialPort(ISerialPortConfig config)
    {
        // 解析配置
        var options_result = config.ToOptions();
        if (!options_result.IsSuccess) return options_result.Cast<RuntimePort>();

        // 创建
        var transport = new SerialPortTransport(options_result.Value);
        var runtime_port = new RuntimePort() { Config = config, Transport = transport };

        return Result.Success(runtime_port);
    }

    #endregion
}