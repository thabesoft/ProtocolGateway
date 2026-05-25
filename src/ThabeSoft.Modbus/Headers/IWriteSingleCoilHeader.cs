using ThabeSoft.Modbus.Headers.Requests;

namespace ThabeSoft.Modbus.Headers;


/// <summary>
/// 写单个线圈值
/// </summary>
public interface IWriteSingleCoilHeader : IRequestHeader
{
    /// <summary>
    /// 线圈值
    /// </summary>
    bool Value { get; }
}
