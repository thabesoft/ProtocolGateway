using System.Text.Json.Serialization;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 传输配置
/// </summary>
[JsonDerivedType(typeof(SerialPortConfig), typeDiscriminator: nameof(PortType.SerialPort))]
public abstract record class PortConfig : ITransportConfig, IValidatable, IDeepCloneable<PortConfig>
{
    public int RetryCount { get; set; } = 5;
    public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(1);
    public TimeSpan ReadTimeout { get; set; } = TimeSpan.FromSeconds(3);
    public TimeSpan WriteTimeout { get; set; } = TimeSpan.FromSeconds(3);
    public abstract PortType Type { get; }


    public abstract PortConfig DeepClone();
    public virtual Result Validate() => Result.Success();
}