using ThabeSoft.Modbus;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;

public static class ProtocolTypeExtensions
{
    extension(ProtocolType type)
    {
        public Result<IModbusMaster> CreateModbusMaster(ITransport transport)
        {
            if (type == ProtocolType.ModbusRtu)
            {
                return Result.Success<IModbusMaster>(new ModbusRtuMaster(transport));
            }

            return Result.Error<IModbusMaster>($"Modbus主站构建失败, 不支持的协议: {type}");
        }
    }
}