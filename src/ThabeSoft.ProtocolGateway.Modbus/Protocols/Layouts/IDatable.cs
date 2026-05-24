namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;

/// <summary>
/// 拥有数据
/// </summary>
public interface IDatable
{
    /// <summary>数据长度 (字节)</summary>
    int DataLength { get; }

    /// <summary>数据范围</summary>
    Range DataRange { get; }

    /// <summary>数据数量 (比如n个线圈, n个寄存器) 和字节无关</summary>
    int DataQuantity { get; }
}