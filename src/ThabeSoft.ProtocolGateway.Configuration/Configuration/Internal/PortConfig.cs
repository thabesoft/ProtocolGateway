using System.Text.Json.Serialization;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration.Internal;


/// <summary>
/// 串口配置
/// </summary>
[JsonDerivedType(typeof(SerialPortConfig), typeDiscriminator: nameof(PortType.SerialPort))]

internal abstract class PortConfig : IPortConfig
{
    public int RetryCount { get; set; } = 5;
    public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(1);
    public TimeSpan ReadTimeout { get; set; } = TimeSpan.FromSeconds(3);
    public TimeSpan WriteTimeout { get; set; } = TimeSpan.FromSeconds(3);

    public Result Validate() => Result.Success();
}

internal static class PortConfigExtensions
{
    extension(IPortConfig config)
    {
        public PortType GetPortType()
        {
            if (config is SerialPortConfig) return PortType.SerialPort;
            return PortType.None;
        }
    }
}
