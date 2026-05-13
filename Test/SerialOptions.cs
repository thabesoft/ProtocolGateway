using System.IO.Ports;

namespace Test;

/// <summary>
/// 串口配置
/// </summary>
public record SerialOptions
{
    public required string PortName { get; init; }
    public int BaudRate { get; init; } = 9600;
    public Parity Parity { get; init; } = Parity.None;
    public int DataBits { get; init; } = 8;
    public StopBits StopBits { get; init; } = StopBits.One;
    /// <summary>
    /// 是否全双工
    /// </summary>
    public bool IsFullDuplex { get; set; } = true;
}


public enum DriverOptionType
{
    ModbusRtu = 100,
    ModbusAscii = 101,
    ModbusTcp = 110,
    ModbusUcp = 111,
}



public record ModbusSerialOptions
{
    public DriverOptionType Type { get; } = DriverOptionType.ModbusRtu;
    public required SerialOptions Config { get; init; }


}

public record Tag
{
    public required string Name { get; set; }        // 变量名，如 "Boiler_Temp"
    public required object Value { get; internal set; } // 当前值
    public required string Address { get; set; }     // 原始地址，如 "40001" 或 "sensor/temp"
    public required string DriverId { get; set; }    // 归属于哪个驱动实例
    //public required TagQuality Quality { get; set; } // 质量戳（Good/Bad/Timeout）
    public required DateTime Timestamp { get; set; } // 时间戳
}