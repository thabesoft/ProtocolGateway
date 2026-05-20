namespace ThabeSoft.IndustrialHub.Modbus;


/// <summary>
/// Modbus打包器
/// </summary>
public interface IModbusRequestPacker
{
    /// <summary>
    /// 尝试将读线圈请求帧打包到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="quantity">数量</param>
    /// <returns>是否打包成功</returns>
    bool TryPackReadCoils(Span<byte> destination, byte slaveId, ushort address, ushort quantity, out int length);
    /// <summary>
    /// 尝试将离散输入请求帧打包到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="quantity">数量</param>
    /// <returns>是否打包成功</returns>
    bool TryPackReadDiscreteInputs(Span<byte> destination, byte slaveId, ushort address, ushort quantity, out int length);
    /// <summary>
    /// 尝试将保持寄存器请求帧打包到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="quantity">数量</param>
    /// <returns>是否打包成功</returns>
    bool TryPackReadHoldingRegisters(Span<byte> destination, byte slaveId, ushort address, ushort quantity, out int length);
    /// <summary>
    /// 尝试将输入寄存器请求帧打包到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="quantity">数量</param>
    /// <returns>是否打包成功</returns>
    bool TryPackReadInputRegisters(Span<byte> destination, byte slaveId, ushort address, ushort quantity, out int length);


    /// <summary>
    /// 写单个寄存器打包到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="value">值</param>
    /// <returns>是否打包成功</returns>
    bool TryPackWriteSingleRegister(Span<byte> destination, byte slaveId, ushort address, ushort value, out int length);
    /// <summary>
    /// 打包写多寄存器帧到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">站号</param>
    /// <param name="address">地址</param>
    /// <param name="values">寄存器值</param>
    /// <returns>是否打包成功</returns>
    bool TryPackWriteMultipleRegisters(Span<byte> destination, byte slaveId, ushort address, ReadOnlySpan<ushort> values, out int length);
    /// <summary>
    /// 打包写多线圈帧到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">站号</param>
    /// <param name="address">地址</param>
    /// <param name="coils">线圈值</param>
    /// <returns>是否打包成功</returns>
    bool TryPackWriteMultipleCoils(Span<byte> destination, byte slaveId, ushort address, ReadOnlySpan<bool> coils, out int length);
    /// <summary>
    /// 写单个线圈帧打包到目标缓冲区
    /// </summary>
    /// <param name="destination">目标缓冲区</param>
    /// <param name="slaveId">从站号</param>
    /// <param name="address">地址</param>
    /// <param name="value">值</param>
    /// <returns>是否打包成功</returns>
    bool TryPackWriteSingleCoil(Span<byte> destination, byte slaveId, ushort address, bool value, out int length);
}

public interface IModbusResponseUnpacker
{
    bool TryGetReadResponseDataLength(ReadOnlySpan<byte> source, out int length);


    bool TryUnpackReadCoilsResponse(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out byte dataLengths);
    bool TryUnpackReadDiscreteInputsResponse(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out byte dataLengths);
    bool TryUnpackReadHoldingRegistersResponse(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out byte dataLengths);
    bool TryUnpackReadInputRegistersResponse(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out byte dataLengths);


    bool TryUnpackWriteSingleResponse(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, bool coils);
}

///// <summary>
///// Modbus解包器
///// </summary>
//public interface IModbusUnpacker
//{
//    /// <summary>
//    /// 尝试从源数据解包读线圈请求帧
//    /// </summary>
//    /// <param name="source">源数据</param>
//    /// <param name="slaveId">从站号</param>
//    /// <param name="address">地址</param>
//    /// <param name="quantity">数量</param>
//    /// <returns>是否解包成功</returns>
//    bool TryUnpackReadCoils(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort quantity);
//    /// <summary>
//    /// 尝试从源数据解包读离散输入请求帧
//    /// </summary>
//    /// <param name="source">源数据</param>
//    /// <param name="slaveId">从站号</param>
//    /// <param name="address">地址</param>
//    /// <param name="quantity">数量</param>
//    /// <returns>是否解包成功</returns>
//    bool TryUnpackReadDiscreteInputs(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort quantity);
//    /// <summary>
//    /// 尝试从源数据解包读保持寄存器请求帧
//    /// </summary>
//    /// <param name="source">源数据</param>
//    /// <param name="slaveId">从站号</param>
//    /// <param name="address">地址</param>
//    /// <param name="quantity">数量</param>
//    /// <returns>是否解包成功</returns>
//    bool TryUnpackReadHoldingRegisters(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort quantity);
//    /// <summary>
//    /// 尝试从源数据解包读输入寄存器请求帧
//    /// </summary>
//    /// <param name="source">源数据</param>
//    /// <param name="slaveId">从站号</param>
//    /// <param name="address">地址</param>
//    /// <param name="quantity">数量</param>
//    /// <returns>是否解包成功</returns>
//    bool TryUnpackReadInputRegisters(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort quantity);


//    /// <summary>
//    /// 解包写多寄存器帧从源数据
//    /// </summary>
//    /// <param name="source">源数据</param>
//    /// <param name="slaveId">从站号</param>
//    /// <param name="address">地址</param>
//    /// <param name="values">寄存器值</param>
//    /// <returns>是否解包成功</returns>
//    bool TryUnpackWriteMultipleRegisters(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, Span<ushort> values);
//    /// <summary>
//    /// 解包写多线圈帧从源数据
//    /// </summary>
//    /// <param name="source">源数据</param>
//    /// <param name="slaveId">从站号</param>
//    /// <param name="address">地址</param>
//    /// <param name="coils">线圈值</param>
//    /// <returns>是否解包成功</returns>
//    bool TryUnpackWriteMultipleCoils(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, Span<bool> coils);
//    /// <summary>
//    /// 写单个线圈帧解包从源数据
//    /// </summary>
//    /// <param name="source">源数据</param>
//    /// <param name="slaveId">从站号</param>
//    /// <param name="address">地址</param>
//    /// <param name="value">值</param>
//    /// <returns>是否解包成功</returns>
//    bool TryUnpackWriteSingleCoil(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out bool value);
//    /// <summary>
//    /// 写单个寄存器帧解包从源数据
//    /// </summary>
//    /// <param name="source">源数据</param>
//    /// <param name="slaveId">从站号</param>
//    /// <param name="address">地址</param>
//    /// <param name="value">值</param>
//    /// <returns>是否解包成功</returns>
//    bool TryUnpackWriteSingleRegister(ReadOnlySpan<byte> source, out byte slaveId, out ushort address, out ushort value);
//}