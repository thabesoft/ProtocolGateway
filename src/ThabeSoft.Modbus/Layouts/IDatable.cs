namespace ThabeSoft.Modbus.Layouts;

/// <summary>
/// 包含数据的
/// </summary>
public interface IDatable
{
    /// <summary>数据范围</summary>
    Range DataRange { get; }

    /// <summary>数据总字节数</summary>
    int DataLength { get; }

    /// <summary>数据数量</summary>
    int DataQuantity { get; }
}