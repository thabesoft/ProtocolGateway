namespace ThabeSoft.IndustriaHub.Modbus.Slave;


/// <summary>
/// Modbus 从站
/// </summary>
public interface IModbusSlave
{
    
}

/// <summary>
/// 传输器
/// </summary>


public class ModbusSlave(SerialTransporter transporter)
{
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {

    }
}
