using System.Buffers.Binary;

namespace ThabeSoft.IndustriaHub.Protocol;


internal interface IResponsePacketParser
{
    /// <summary>
    /// 尝试解析读线圈操作
    /// </summary>
    /// <param name="data"></param>
    /// <param name="slaveId"></param>
    /// <param name="address"></param>
    /// <param name="functionCode"></param>
    /// <returns></returns>
    bool TryParseReadColis(ReadOnlySpan<byte> data, out int slaveId, out ReadOnlySpan<bool> values);
}

internal sealed class ResponsePacketParser(ICrcCalculator crcCalculator) : IResponsePacketParser
{
    public bool TryParseReadColis(ReadOnlySpan<byte> data, out int slaveId, out ReadOnlySpan<bool> values)
    {
        slaveId = 0;
        values = default;

        // 最短有效响应：1(地址)+1(功能码)+1(字节数)+0(数据)+2(CRC) = 5 字节
        // 实际最常见至少有1个字节数据 → 至少6字节
        if (data.Length < 5)
        {
            return false;
        }

        slaveId = data[0];

        // 检查功能码
        ModbusFunctionCode functionCode = data[1];
        if (functionCode != ModbusFunctionCode.ReadCoils || functionCode.IsException) return false;

        // 字节计数（表示后续数据有多少字节）
        byte byteCount = data[2];
        int expectedDataStart = 3;
        int expectedDataEnd = expectedDataStart + byteCount;
        int expectedTotalLength = expectedDataEnd + 2; // + CRC

        // 帧长度是否匹配
        if (data.Length < expectedTotalLength) return false;

        // 校验 CRC（只校验到数据结束，不含 CRC 本身）
        ReadOnlySpan<byte> crcPart = data.Slice(expectedDataEnd, 2);
        ushort calculatedCrc = crcCalculator.Calc(data.Slice(0, expectedDataEnd));
        ushort receivedCrc = BinaryPrimitives.ReadUInt16LittleEndian(crcPart);

        if (calculatedCrc != receivedCrc)
        {
            return false;
        }

        // 提取线圈状态数据
        ReadOnlySpan<byte> coilBytes = data.Slice(expectedDataStart, byteCount);

        // 我们需要把字节展开成 bool[]
        // 但因为是 ReadOnlySpan<bool>，我们不能直接返回数组
        // 常见的做法是：返回展开后的 span，但 .NET 没有内置 BitSpan
        // 所以这里有两种主流做法：

        // 方案A：返回 bool[]（最常用，简单）
        // 方案B：返回自定义的 BitSpan 或使用 BitArray（但 BitArray 不支持 Span）

        // 这里我们选择最实用的方案：返回 bool[]
        bool[] result = new bool[byteCount * 8];

        int coilIndex = 0;
        for (int i = 0; i < byteCount; i++)
        {
            byte b = coilBytes[i];
            for (int bit = 0; bit < 8 && coilIndex < result.Length; bit++, coilIndex++)
            {
                result[coilIndex] = (b & (1 << bit)) != 0;
            }
        }

        // 实际线圈数量 ≤ byteCount*8，但我们不知道确切数量
        // 调用方通常知道自己请求了多少个线圈
        // 所以这里返回全部展开的，但建议调用方自己截取需要的长度

        values = result.AsSpan();

        return true;
    }

}