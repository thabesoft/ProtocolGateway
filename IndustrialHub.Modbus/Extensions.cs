using IndustrialHub.Modbus;
using System.IO.Ports;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace IndustrialHub;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class Extensions
{
    extension(IDeviceModelBuilder builder)
    {
        public IDeviceBuilder UseModbusRtu(ModbusSerialOptions options)
        {
        }

        public IDeviceBuilder UseModbusAscii(ModbusSerialOptions options)
        {
        }

        public IDeviceBuilder UseModbusTcp(ModbusIpOptions options)
        {
        }

        public IDeviceBuilder UseModbusUdp(ModbusIpOptions options)
        {
        }


        public IDeviceBuilder UseModbusRtu(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            return builder.UseModbusRtu(new ModbusSerialOptions()
            {
                PortName = portName,
                BaudRate = baudRate,
                Parity = parity,
                DataBits = dataBits,
                StopBits = stopBits
            });
        }
        public IDeviceBuilder UseModbusAscii(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            return builder.UseModbusRtu(new ModbusSerialOptions()
            {
                PortName = portName,
                BaudRate = baudRate,
                Parity = parity,
                DataBits = dataBits,
                StopBits = stopBits
            });
        }
        public IDeviceBuilder UseModbusTcp(string address, int port)
        {
            return builder.UseModbusTcp(new ModbusIpOptions()
            {
                Address = address,
                Port = port
            });
        }
        public IDeviceBuilder UseModbusUdp(string address, int port)
        {
            return builder.UseModbusUdp(new ModbusIpOptions()
            {
                Address = address,
                Port = port
            });
        }
    }
}


/// <summary>
/// Modbus 基础选项
/// </summary>
public record ModbusOptions
{
    /// <summary>
    /// 重试次数，默认值为0，表示不重试
    /// </summary>
    public int RetryCount { get; init; }

    /// <summary>
    /// 重试间隔，默认值为0，表示不等待直接重试
    /// </summary>
    public TimeSpan RetryInterval { get; init; }
}

/// <summary>
/// Modbus 串口选项
/// </summary>
public record ModbusSerialOptions : ModbusOptions
{
    /// <summary>
    /// 端口名
    /// </summary>
    public required string PortName { get; init; }

    /// <summary>
    /// 波特率
    /// </summary>
    public int BaudRate { get; init; } = 9600;

    /// <summary>
    /// 校验
    /// </summary>
    public Parity Parity { get; init; } = Parity.None;

    /// <summary>
    /// 数据位
    /// </summary>
    public int DataBits { get; init; } = 8;

    /// <summary>
    /// 停止位
    /// </summary>
    public StopBits StopBits { get; init; } = StopBits.One;
}

/// <summary>
/// Modbus 网线选项
/// </summary>
public record ModbusIpOptions : ModbusOptions
{
    /// <summary>
    /// 地址
    /// </summary>
    public required string Address { get; init; }

    /// <summary>
    /// 端口
    /// </summary>
    public required int Port { get; init; }
}