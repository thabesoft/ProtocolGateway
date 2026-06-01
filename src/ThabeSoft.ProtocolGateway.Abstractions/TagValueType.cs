using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 数据类型
/// </summary>
public enum TagValueType
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

public static class TagValueTypeExtensions
{
    private static readonly Dictionary<TagValueType, int> _dataByteLength = new()
    {
        { TagValueType.Bool, 1 }, { TagValueType.SByte, 1 }, { TagValueType.Byte, 1 },
        { TagValueType.Int16, 2 }, { TagValueType.UInt16, 2 }, { TagValueType.Char, 2 },
        { TagValueType.Int32, 4 }, { TagValueType.UInt32, 4 }, { TagValueType.Float, 4 },
        { TagValueType.Int64, 8 }, { TagValueType.UInt64, 8 }, { TagValueType.Double, 8 },
    };

    extension(TagValueType dataType)
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