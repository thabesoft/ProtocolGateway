namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 写单个寄存器值
/// </summary>
public interface IWriteSingleRegisterHeader : IRequestHeader
{
    /// <summary>
    /// 寄存器值
    /// </summary>
    ushort Value { get; }
}