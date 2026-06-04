using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;

public static class PortTypeExtensions
{
    extension(PortType type)
    {
        /// <summary>
        /// 从端口类型创建传输器
        /// </summary>
        public Result<ITransport> CreatePort()
        {
            if (type == PortType.SerialPort)
            {
                return Result.Success<ITransport>(new SerialPortTransport());
            }

            return Result.Error<ITransport>($"端口构建失败, 未知的配置类型: {type}");
        }
    }
}
