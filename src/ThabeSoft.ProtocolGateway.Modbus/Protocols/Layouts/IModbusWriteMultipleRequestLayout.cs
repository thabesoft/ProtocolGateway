namespace ThabeSoft.ProtocolGateway.Protocols.Layouts;


/// <summary>
/// Rtu 写多个值请求帧布局
/// </summary>
public interface IModbusWriteMultipleRequestLayout : IModbusRequestLayout
{
    /// <summary>值数量范围[4..6)</summary>
    Range QuantityRange { get; }
    /// <summary>数据长度索引(6)</summary>
    int DataLengthIndex { get; }
    /// <summary>数据范围</summary>
    Range DataRange { get; }
    /// <summary>数据总字节数</summary>
    int DataByteLength { get; }
    /// <summary>数据最大数量</summary>
    int DataMaxQuantity { get; }
}