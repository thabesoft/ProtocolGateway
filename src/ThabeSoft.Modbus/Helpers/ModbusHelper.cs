using System.Runtime.CompilerServices;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Helpers;


public static class ModbusHelper
{
    private const ushort CoilOpen = 0xFF00;
    private const ushort CoilClose = 0x0000;

    /// <summary>
    /// 获取线圈数量转为字节后的长度
    /// </summary>
    /// <param name="quantity">线圈数量</param>
    /// <returns>锁占用的字节</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetColisToByteLength(int quantity) => (byte)((quantity + 7) / 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetColisToByteLength(ReadCoilsQuantity quantity) => (byte)((quantity + 7) / 8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetColisToByteLength(WriteCoilsQuantity quantity) => (byte)((quantity + 7) / 8);

    /// <summary>
    /// 获取寄存器转为字节后的长度
    /// </summary>
    /// <param name="quantity">寄存器数量</param>
    /// <returns>所占用的字节</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetRegistersToByteLength(int quantity) => (byte)(quantity * 2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetRegistersToByteLength(ReadRegistersQuantity quantity) => (byte)(quantity * 2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetRegistersToByteLength(WriteRegistersQuantity quantity) => (byte)(quantity * 2);


    /// <summary>
    /// 获取单线圈的值 (Modbus写入值最低要求 两个字节)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetCoilWordValue(bool value) => value ? CoilOpen : CoilClose;

    /// <summary>
    /// 获取单线圈的值
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<bool> GetSingleCoilValue(ushort value)
    {
        return value switch
        {
            CoilOpen => true,
            CoilClose => false,
            _ => Result.InvalidParameter<bool>($"无效的 Modbus 线圈值: 0x{value:X4}，有效值为 0xFF00 (ON) 或 0x0000 (OFF)")
        };
    }
}