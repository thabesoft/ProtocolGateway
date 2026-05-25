using System.Runtime.CompilerServices;
using ThabeSoft.Modbus.Primitives;
using ThabeSoft.Primitives;

namespace ThabeSoft.Modbus.Helpers;


public static class ModbusHelper
{
    private const ushort CoilOpen = 0xFF00;
    private const ushort CoilClose = 0x0000;

    /// <summary>
    /// 从线圈开关值转为字 开:0xFF00, 关0x0000
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ToModbusWordValue(this bool value) => value ? CoilOpen : CoilClose;

    /// <summary>
    /// 转为Modbus线圈值, 只能是 0xFF00(开) 或 0x0000(关)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<bool> ToModbusCoilValue(this ushort value)
    {
        return value switch
        {
            CoilOpen => true,
            CoilClose => false,
            _ => Result.InvalidParameter<bool>($"无效的 Modbus 线圈值: 0x{value:X4}，有效值为 0xFF00 (ON) 或 0x0000 (OFF)")
        };
    }
}