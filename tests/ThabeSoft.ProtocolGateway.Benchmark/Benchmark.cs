using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Benchmark;



[MemoryDiagnoser(true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class Benchmark
{
    private readonly byte[] buffer = new byte[1000];


    [Benchmark(Baseline = true)]
    public void Call()
    {
        var result = Result.Ok(10);
        if (!result.IsSuccess) return;

        var result1 = Result.Ok($"输入了: {result.Value}");
        if (!result1.IsSuccess) return;

        var result2 = Result.Ok(result1.Value.GetHashCode());
        if (!result2.IsSuccess) return;

        var result3 = result2.ToString();
    }

    [Benchmark]
    public async Task CallAsync()
    {
        var result = await GetIntResultAsync();
        if (!result.IsSuccess) return;

        var result1 = await GetStringResultAsync(result.Value);
        if (!result1.IsSuccess) return;

        var result2 = await GetHashCodeResultAsync(result1.Value);
        if (!result2.IsSuccess) return;

        var result3 = await GetHashCodeStringResultAsync(result2.Value);
    }


    [Benchmark]
    public void Pipe()
    {
        var result = ResultPipeExtensions.Pipe(Result.Ok(10))
            .Then(x => Result.Ok($"输入了: {x}"))
            .Then(x => Result.Ok(x.GetHashCode()))
            .Map(x => x.ToString())
            .Execute();
    }

    [Benchmark]
    public async Task PipeAsync()
    {
        //var result = await ResultPipeExtensions.AsyncPipe(await GetIntResultAsync())
        //    .Then(x => GetStringResultAsync(x))
        //    .Then(GetHashCodeResultAsync)
        //    .Then(GetHashCodeStringResultAsync)
        //    .ExecuteAsync();
    }


    private async ValueTask<Result<int>> GetIntResultAsync()
    {
        await Task.Yield();
        return 10;
    }
    private async ValueTask<Result<string>> GetStringResultAsync(int x)
    {
        await Task.Yield();
        return $"输入了: {x}";
    }
    private async ValueTask<Result<int>> GetHashCodeResultAsync(string x)
    {
        await Task.Yield();
        return x.GetHashCode();
    }
    private async ValueTask<Result<string>> GetHashCodeStringResultAsync(int x)
    {
        await Task.Yield();
        return x.ToString();
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