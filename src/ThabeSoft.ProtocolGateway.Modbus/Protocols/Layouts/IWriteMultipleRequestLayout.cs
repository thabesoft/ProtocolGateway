namespace ThabeSoft.ProtocolGateway.Modbus.Protocols.Layouts;


/// <summary>
/// Rtu 写多个值请求帧布局
/// </summary>
public interface IWriteMultipleRequestLayout : IRequestLayout, IDatable
{
    /// <summary>值数量范围</summary>
    Range QuantityRange { get; }

    /// <summary>数据长度索引</summary>
    int DataLengthIndex { get; }

    /// <summary>数据数量</summary>
    int DataQuantity { get; }
}