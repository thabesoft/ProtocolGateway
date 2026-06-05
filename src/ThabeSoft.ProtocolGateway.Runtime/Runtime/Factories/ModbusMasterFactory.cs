using ThabeSoft.Modbus;
using ThabeSoft.Ports;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Runtime.Factories;

/// <summary>
/// Modbus 主站工厂
/// </summary>
internal static class ModbusMasterFactory
{
    public static Result<IModbusMaster> CreateFromTransport(ITransport transport)
    {
        if(transport is SerialPortTransport)
        {
            return Result.Success<IModbusMaster>(new ModbusRtuMaster(transport));
        }

        return Result.Error<IModbusMaster>($"Modbus主站构建失败, 不支持的传输类型: {transport.GetType().Name}");
    }
}