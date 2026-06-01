using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 数据类型
/// </summary>
public enum DataType
{
    // 8bit 
    Bool, SByte, Byte,

    // 16bit
    Int16, UInt16, Char,

    // 32bit
    Int32, UInt32, Float,

    // 64bit
    Int64, UInt64, Double
}

public static class DataTypeExtensions
{
    private static readonly Dictionary<DataType, int> _dataByteLength = new()
    {
        { DataType.Bool, 1 }, { DataType.SByte, 1 }, { DataType.Byte, 1 },
        { DataType.Int16, 2 }, { DataType.UInt16, 2 }, { DataType.Char, 2 },
        { DataType.Int32, 4 }, { DataType.UInt32, 4 }, { DataType.Float, 4 },
        { DataType.Int64, 8 }, { DataType.UInt64, 8 }, { DataType.Double, 8 },
    };

    extension(DataType dataType)
    {
        /// <summary>
        /// 所占用的字节数量
        /// </summary>
        public Result<int> GetByteLength()
        {
            if (_dataByteLength.TryGetValue(dataType, out var result))
            {
                return Result.Ok(result);
            }

            return Result.NotSupported<int>($"不支持的数据类型: {dataType}");
        }
    }
}