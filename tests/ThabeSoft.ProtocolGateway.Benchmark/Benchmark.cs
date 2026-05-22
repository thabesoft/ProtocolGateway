using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ThabeSoft.ProtocolGateway.Benchmark;



[MemoryDiagnoser(true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class Benchmark
{
    private readonly byte[] buffer = new byte[1000];


    [Benchmark(Baseline = true)]
    public void 传递()
    {
        ResultPipe
    }

    [Benchmark]
    public void 闭包()
    {
        int number = 256;
        var result = Fuck(() => number * number);
    }


    //[Benchmark]
    //public void ReadCoilsPack()
    //{
    //    ModbusRtuRequest.ReadCoils.TryPack(buffer, 1, 100, 100);
    //}
    //[Benchmark]
    //public void ReadDiscreteInputs()
    //{
    //    ModbusRtuRequest.ReadDiscreteInputsSerializer.TryPack(buffer, 1, 100, 100);
    //}
    //[Benchmark]
    //public void ReadInputRegisters()
    //{
    //    ModbusRtuRequest.ReadInputRegistersSerializer.TryPack(buffer, 1, 100, 100);
    //}
    //[Benchmark]
    //public void ReadHoldingRegisters()
    //{
    //    ModbusRtuRequest.ReadHoldingRegisters.TryPack(buffer, 1, 100, 100);
    //}


    //[Benchmark]
    //public void WriteSingleCoil()
    //{
    //    ModbusRtuRequest.WriteSingleCoilSerializer.TryPack(buffer, 1, 100, true);
    //}
    //[Benchmark]
    //public void WriteSingleRegister()
    //{
    //    ModbusRtuRequest.WriteSingleRegisterSerializer.TryPack(buffer, 1, 100, 0xFF);
    //}
    //[Benchmark]
    //public void WriteMultipleCoils()
    //{
    //    Span<bool> values = [false, false, true];
    //    ModbusRtuRequest.WriteMultipleCoilsSerializer.TryPack(buffer, 1, 100, values);
    //}
    //[Benchmark]
    //public void WriteMultipleRegisters()
    //{
    //    Span<ushort> values = [0xFF, 0xEE, 0xCC];
    //    ModbusRtuRequest.WriteMultipleRegisters.TryPack(buffer, 1, 100, values);
    //}
}