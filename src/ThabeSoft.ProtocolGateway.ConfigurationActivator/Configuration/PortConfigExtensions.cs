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
        /// <summary>
        /// 从配置创建传输器
        /// </summary>
        public Result<ITransport> CreateTransport()
        {
            // 串口
            if (config is ISerialPortConfig serialPort)
            {
                return serialPort.CreateSerialPortTransport().Map(x => (ITransport)x);
            }

            return Result.Error<ITransport>($"该配置无法创建传输器: {config.GetType().Name}");
        }
    }


    extension(ISerialPortConfig config)
    {
        /// <summary>
        /// 从串口配置转为串口选项
        /// </summary>
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

        /// <summary>
        /// 从串口配置创建串口传输器
        /// </summary>
        public Result<SerialPortTransport> CreateSerialPortTransport()
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
