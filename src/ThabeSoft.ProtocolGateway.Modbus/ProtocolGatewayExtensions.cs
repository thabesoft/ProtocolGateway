using ThabeSoft.Modbus;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


public static class ProtocolGatewayExtensions
{
    extension(IGateway gateway)
    {
        /// <summary>
        /// 添加Modbus通道
        /// </summary>
        /// <param name="name">通道名称</param>
        /// <param name="master">Modbus主站</param>
        public ModbusChannel AddChannel(ChannelName name, IModbusMaster master)
        {
            ModbusChannel channel = new(master);
            gateway.AddChannel(name, channel);

            return channel;
        }

        /// <summary>
        /// 添加通道
        /// </summary>
        /// <param name="name">通道名称</param>
        /// <param name="master">Modbus主站</param>
        public Result<ModbusChannel> AddChannel(string name, IModbusMaster master)
        {
            ModbusChannel channel = new(master);
            gateway.AddChannel(name, channel);

            return Result.Ok(channel);
        }
    }
}
