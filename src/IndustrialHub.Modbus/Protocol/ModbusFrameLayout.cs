using System.Runtime.CompilerServices;

namespace IndustrialHub.Modbus.Protocol;


/// <summary>
/// Modbus 帧布局
/// </summary>
public sealed class ModbusFrameLayout
{
    /// <summary>
    /// 获取线圈数量转为字节后的长度
    /// </summary>
    /// <param name="quantity">线圈数量</param>
    /// <returns>锁占用的字节</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetColisToByteLength(ushort quantity) => (quantity + 7) / 8;

    /// <summary>
    /// 获取寄存器转为字节后的长度
    /// </summary>
    /// <param name="quantity">寄存器数量</param>
    /// <returns>所占用的字节</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRegistersToByteLength(ushort quantity) => quantity * 2;


    /// <summary>
    /// 获取单线圈的值 (Modbus写入值最低要求 两个字节)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetSingleCoilValue(bool value) => value ? (ushort)0xFF00 : (ushort)0x0000;

    /// <summary>
    /// 获取单线圈的值
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GetSingleCoilValue(ushort value)
    {
        return value switch
        {
            0xFF00 => true,
            0x0000 => false,
            _ => throw new ArgumentException($"无效的 Modbus 线圈值: 0x{value:X4}，有效值为 0xFF00 (ON) 或 0x0000 (OFF)", nameof(value))
        };
    }
}