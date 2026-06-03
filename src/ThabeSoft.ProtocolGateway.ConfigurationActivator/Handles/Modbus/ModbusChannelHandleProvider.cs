using ThabeSoft.Modbus;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Handles.Modbus;


/// <summary>
/// 通道句柄提供者
/// </summary>
internal class ModbusChannelHandleProvider : IChannelHandleProvider
{
    public bool CanCreate(ChannelConfig config)
    {
        return config.Protocol is (ProtocolType.ModbusRtu or ProtocolType.ModbusTcp or ProtocolType.ModbusUdp);
    }

    public Result<IChannelHandle> Create(ChannelConfig config)
    {
        // 串口
        if(config.Port is SerialPortConfig serialPortConfig)
        {
            var options = ToOptions(serialPortConfig);
            var port = new SerialPortTransport();

            // Rtu
            if(config.Protocol == ProtocolType.ModbusRtu)
            {
                var master = new ModbusRtuMaster(port);
                var channel = new ModbusChannel(master);
                var handle = new ModbusRtuChannelHandle(config.Name, channel, options, port);

                return Result.Success<IChannelHandle>(handle);
            }
        }

        return Result.Error<IChannelHandle>($"无法获取通道句柄, 不支持的协议类型: {config.Protocol}");
    }


    private static SerialPortOptions ToOptions(SerialPortConfig config)
    {
        return SerialPortOptions.Create(config.PortName)
            .SetBaudRate(config.BaudRate)
            .SetParity(config.Parity)
            .SetStopBits(config.StopBits)
            .SetDuplexMode(config.DuplexMode);
    }
}
