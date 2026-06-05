using System.ComponentModel;
using ThabeSoft.Lifecycle;
using ThabeSoft.Modbus;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime.Factories;

/// <summary>
/// 通道句柄提供者
/// </summary>
internal class RuntimeModbusChannelProvider : IRuntimeChannelProvider
{
    public bool CanCreate(IChannelConfig config)
    {
        if (!config.Validate().IsSuccess) return false;
        return config.Protocol is ProtocolType.ModbusRtu or ProtocolType.ModbusTcp or ProtocolType.ModbusUdp;
    }

    public Result<IRuntimeChannel> Create(ChannelConfig config)
    {
        var value= new RuntimeModbusChannel(config);
        return Result.Success<IRuntimeChannel>(value);
    }
}



public class RuntimeModbusChannel(ChannelConfig config) : LifecycleObject, IRuntimeChannel, IReadWriteChannel
{
    private ITransport _transport;
    private IModbusMaster _master;
    private ModbusChannel _channel;
    private List<IRuntimeTag> _tags = [];


    public IChannelConfig Config { get; private set; } = config.DeepClone();
    public bool IsEnable { get; }
    public IReadOnlyCollection<IRuntimeTag> Tags => _tags;



    protected override async ValueTask<Result> StartProcessAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var port = PortFactory.Create(Config.Port).Value;
            var master = ModbusMasterFactory.CreateFromTransport(port).Value;
            var channel = new ModbusChannel(master);

            await channel.StartAsync(cancellationToken);

            _transport = port;
            _master = master;
            _channel = channel;

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error($"Modbus通道启动失败: {ex.Message}");
        }
    }

    protected override async ValueTask<Result> StopProcessAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _channel.StopAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error($"Modbus通道启动失败: {ex.Message}");
        }
    }

    public ValueTask<Result<TValue>> ReadAsync<TValue>(ITag<TValue> tag, CancellationToken cancellationToken = default) where TValue : unmanaged
        => _channel.ReadAsync(tag, cancellationToken);
    public ValueTask<Result> WriteAsync<TValue>(ITag<TValue> tag, TValue value, CancellationToken cancellationToken = default) where TValue : unmanaged
        => _channel.WriteAsync(tag, value, cancellationToken);
}