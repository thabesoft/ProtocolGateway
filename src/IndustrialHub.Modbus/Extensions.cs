//using IndustrialHub.Modbus;
//using IndustrialHub.Modbus.Options;
//using System.IO.Ports;

//#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
//namespace IndustrialHub;
//#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

//public static class Extensions
//{
//    extension(IDeviceModelBuilder builder)
//    {
//        public IDeviceBuilder UseModbusRtu(ModbusSerialOptions options)
//        {
//        }

//        public IDeviceBuilder UseModbusAscii(ModbusSerialOptions options)
//        {
//        }

//        public IDeviceBuilder UseModbusTcp(ModbusIpOptions options)
//        {
//        }

//        public IDeviceBuilder UseModbusUdp(ModbusIpOptions options)
//        {
//        }


//        public IDeviceBuilder UseModbusRtu(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
//        {
//            return builder.UseModbusRtu(new ModbusSerialOptions()
//            {
//                PortName = portName,
//                BaudRate = baudRate,
//                Parity = parity,
//                DataBits = dataBits,
//                StopBits = stopBits
//            });
//        }
//        public IDeviceBuilder UseModbusAscii(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
//        {
//            return builder.UseModbusRtu(new ModbusSerialOptions()
//            {
//                PortName = portName,
//                BaudRate = baudRate,
//                Parity = parity,
//                DataBits = dataBits,
//                StopBits = stopBits
//            });
//        }
//        public IDeviceBuilder UseModbusTcp(string address, int port)
//        {
//            return builder.UseModbusTcp(new ModbusIpOptions()
//            {
//                Address = address,
//                Port = port
//            });
//        }
//        public IDeviceBuilder UseModbusUdp(string address, int port)
//        {
//            return builder.UseModbusUdp(new ModbusIpOptions()
//            {
//                Address = address,
//                Port = port
//            });
//        }
//    }
//}
