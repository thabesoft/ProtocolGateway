namespace ThabeSoft.Modbus.Encoding;


/// <summary>
/// 主站编码器
/// </summary>
public interface IMasterCodec : IReadCodec, IWriteSingleCodec, IWriteMultipleCodec;