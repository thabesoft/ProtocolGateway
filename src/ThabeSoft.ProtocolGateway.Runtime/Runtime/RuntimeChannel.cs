using System.Text;
using ThabeSoft.Lifecycle;
using ThabeSoft.Modbus;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime;


/// <summary>
/// 通用运行时通道
/// </summary>
public sealed class RuntimeChannel : LifecycleObject, IRuntimeChannel
{
    /// <summary>
    /// 处理过程
    /// </summary>
    internal delegate ValueTask<Result> ProcessHandler(CancellationToken cancellationToken = default);
    /// <summary>
    /// 释放过程
    /// </summary>
    internal delegate ValueTask DisposeHandler();


    internal ProcessHandler StartProcess { get; init; } = default!;
    internal ProcessHandler StopProcess { get; init; } = default!;
    internal DisposeHandler DisposeProcess { get; init; } = default!;


    public bool IsEnable { get; } = true;
    public required IChannelConfig Config { get; init; }
    public required IRuntimePort Port { get; init; }
    public required IReadOnlyCollection<IRuntimeTag> Tags { get; init; }


    // 私有构造
    private RuntimeChannel() { }


    protected override ValueTask<Result> StartProcessAsync(CancellationToken cancellationToken = default)
    {
        return StartProcess(cancellationToken);
    }
    protected override ValueTask<Result> StopProcessAsync(CancellationToken cancellationToken = default)
    {
        return StopProcess(cancellationToken);
    }
    protected override ValueTask DisposeProcessAsync()
    {
        return DisposeProcess();
    }



    #region --工厂--

    public static Result<RuntimeChannel> Create(IChannelConfig config)
    {
        // 验证配置
        var validate_result = config.Validate();
        if (!validate_result.IsSuccess) return validate_result.Cast<RuntimeChannel>();

        // 创建端口
        var runtime_port_result = RuntimePort.Create(config.Port);
        if (runtime_port_result.IsFailure) return runtime_port_result.Cast<RuntimeChannel>();

        // 通道
        var channel_result = GetChannel(config.Type, runtime_port_result.Value);
        if (channel_result.IsFailure) return channel_result.Cast<RuntimeChannel>();

        // 标签
        var runtime_tag_result = GetTags(config.Tags);
        if (runtime_tag_result.IsFailure) return runtime_tag_result.Cast<RuntimeChannel>();

        // 运行时通道
        var runtime_channel = new RuntimeChannel()
        {
            StartProcess = channel_result.Value.StartAsync,
            StopProcess = channel_result.Value.StopAsync,
            DisposeProcess = channel_result.Value.DisposeAsync,

            Config = config,
            Port = runtime_port_result.Value,
            Tags = runtime_tag_result.Value
        };
        return Result.Success(runtime_channel);
    }

    private static Result<IChannel> GetChannel(ChannelType type, IRuntimePort port)
    {
        if (type == ChannelType.Modbus)
        {
            var master_result = CreateModbusMaster(port);
            if (master_result.IsFailure) return master_result.Cast<IChannel>();

            var vlaue = new ModbusChannel(master_result.Value);
            return Result.Success<IChannel>(vlaue);
        }

        return Result.Error<IChannel>($"无法通道, 不支持的通道类型 [{type}]");


        // 创建Modbus 主站
        static Result<IModbusMaster> CreateModbusMaster(IRuntimePort port)
        {
            var port_type = port.Config.Type;

            if (port_type == PortType.SerialPort)
            {
                return Result.Success<IModbusMaster>(new ModbusRtuMaster(port));
            }

            return Result.Error<IModbusMaster>($"Modbus主站构建失败, 不支持的端口类型 [{port_type}]");
        }
    }

    private static Result<IReadOnlyList<RuntimeTag>> GetTags(IEnumerable<ITagConfig> configs)
    {
        bool has_warning = false;
        StringBuilder warning_message = new();

        List<RuntimeTag> tags = [];



        foreach (var i in configs)
        {
            var runtime_tag_result = RuntimeTag.Create(i);

            if (runtime_tag_result.IsSuccess)
            {
                tags.Add(runtime_tag_result.Value);
                continue;
            }

            has_warning = true;
            warning_message.AppendLine(runtime_tag_result.Message);
        }

        if(has_warning)
        {
            var message = $"标签没有完全获取成功{Environment.NewLine}{warning_message}";
            return Result.Warning<IReadOnlyList<RuntimeTag>>(message, tags);
        }
        else
        {
            return Result.Success<IReadOnlyList<RuntimeTag>>(tags);
        }
    }

    #endregion
}

