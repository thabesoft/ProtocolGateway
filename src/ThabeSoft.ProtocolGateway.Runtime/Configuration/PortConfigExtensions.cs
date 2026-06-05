using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Configuration;


/// <summary>
/// 端口配置扩展
/// </summary>
public static class PortConfigExtensions
{
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
    }
}