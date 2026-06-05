using ThabeSoft.Lifecycle;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Runtime.Factories;

namespace ThabeSoft.ProtocolGateway.Runtime;

/// <summary>
/// 运行时端口
/// </summary>
internal sealed class RuntimeSerialPort : LifecycleObject, IRuntimePort
{
    // 传输层
    private readonly SerialPortTransport _transport;

    // 配置
    ITransportConfig IRuntimePort.Config => Config;

    /// <summary>
    /// 配置
    /// </summary>
    public ISerialPortConfig Config { get; }


    private RuntimeSerialPort(ISerialPortConfig config, SerialPortTransport transport)
    {
        Config = config;
        _transport = transport;
    }
    public static Result<RuntimeSerialPort> CreateFromConfig(ISerialPortConfig config)
    {
        var result = PortFactory.CreateSerialPort(config);
        if (result.IsProblem) return result.Cast<RuntimeSerialPort>();

        var value = new RuntimeSerialPort(config, result.Value);
        return Result.Success(value);
    }


    // 读取
    public ValueTask<Result> ReadExactAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        => _transport.ReadExactAsync(buffer, cancellationToken);
    // 写入
    public ValueTask<Result> WriteAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
        => _transport.WriteAsync(data, cancellationToken);

    // 启动
    protected override ValueTask<Result> StartProcessAsync(CancellationToken cancellationToken = default)
        => _transport.StartAsync(cancellationToken);
    // 停止
    protected override ValueTask<Result> StopProcessAsync(CancellationToken cancellationToken = default)
        => _transport.StopAsync(cancellationToken);
}