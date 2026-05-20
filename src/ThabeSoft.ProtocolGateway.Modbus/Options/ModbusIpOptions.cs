using System.Net;
using ThabeSoft.ProtocolGateway.Transports;

namespace IndustrialHub.Modbus.Options;

public interface IModbusIpOptions : ITransportOptions
{
    /// <summary>
    /// 地址
    /// </summary>
    IPEndPoint IpAddress { get; }
}


/// <summary>
/// Modbus 网线选项
/// </summary>
public sealed class ModbusIpOptions : ProtocolOptions
{
    /// <summary>
    /// 地址
    /// </summary>
    public IPEndPoint IpAddress { get; }


    private ModbusIpOptions(IPEndPoint ipAddress)
    {
        IpAddress = ipAddress;
    }

    /// <summary>
    /// 创建
    /// </summary>
    public static ModbusIpOptions Create(IPEndPoint ipAddress)
    {
        return new ModbusIpOptions(ipAddress);
    }
}