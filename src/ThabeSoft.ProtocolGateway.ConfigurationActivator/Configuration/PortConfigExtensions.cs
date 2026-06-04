using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 端口配置扩展
/// </summary>
public static class PortConfigExtensions
{
    extension(IPortConfig config)
    {
        public Result<ITransport> CreateTransport()
        {
            // 串口
            if (config is ISerialPortConfig serialPort)
            {
                return serialPort.ToSerialPortTransport().Map(x => (ITransport)x);
            }


            return Result.Error<ITransport>($"该配置无法创建传输器: {config.GetType().Name}");
        }
    }


    extension(ISerialPortConfig config)
    {
        public Result<SerialPortOptions> ToOptions()
        {
            var result = config.Validate();
            if (!result.IsSuccess) return result.Cast<SerialPortOptions>();

            var value = SerialPortOptions.Create(config.PortName)
                .SetBaudRate(config.BaudRate)
                .SetStopBits(config.StopBits)
                .SetDataBits(config.DataBits)
                .SetDuplexMode(config.DuplexMode)
                .SetParity(config.Parity);
            return Result.Success(value);
        }


        public Result<SerialPortTransport> ToSerialPortTransport()
        {
            var result = config.Validate();
            if (!result.IsSuccess) return result.Cast<SerialPortTransport>();

            // 解析配置
            var options_result = config.ToOptions();
            if (!options_result.IsSuccess) return options_result.Cast<SerialPortTransport>();

            // 创建
            var transport = new SerialPortTransport();
            transport.ChangeOptions(options_result.Value);

            return Result.Success(transport);
        }
    }
}
