namespace IndustrialHub.Modbus.Protocol;


public interface IFrameLayout
{
    /// <summary>从站Id索引(0)</summary>
    int SalveIdIndex { get; }
    /// <summary>功能码索引(1)</summary>
    int FunctionCodeIndex { get; }
    /// <summary>地址范围[2..4)</summary>
    Range AddressRange { get; }
    /// <summary>内容范围</summary>
    Range ContentRange { get; }
    /// <summary>帧总长度</summary>
    int FullByteLength { get; }
}