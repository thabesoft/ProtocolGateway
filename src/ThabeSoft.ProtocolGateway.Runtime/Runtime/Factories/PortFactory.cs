using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Runtime.Factories;


/// <summary>
/// 端口工厂
/// </summary>
internal static class PortFactory
{
    /// <summary>
    /// 从配置创建端口
    /// </summary>
    public static IResult<ITransport> Create(ITransportConfig config)
    {
        // 串口
        if (config is SerialPortConfig serialPort)
        {
            return CreateSerialPort(serialPort);
        }

        return Result.Error<ITransport>($"端口构建失败, 未知的配置类型: {config.GetType()}");
    }


    /// <summary>
    /// 从串口配置创建串口传输器
    /// </summary>
    public static Result<SerialPortTransport> CreateSerialPort(ISerialPortConfig config)
    {
        // 解析配置
        var options_result = config.ToOptions();
        if (!options_result.IsSuccess) return options_result.Cast<SerialPortTransport>();

        // 创建
        var transport = new SerialPortTransport(options_result.Value);
        return Result.Success(transport);
    }
}
