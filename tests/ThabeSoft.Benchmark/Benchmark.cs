using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ThabeSoft.Primitives;

namespace ThabeSoft.Benchmark;



[MemoryDiagnoser(true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class Benchmark
{
    [Benchmark(Baseline = true)]
    public void Call()
    {
        var result = TestResult1();
        if (!result.IsSuccess) return;

        var result1 = TestResult2(result.Value);
        if (!result1.IsSuccess) return;

        var result2 = TestResult3(result1.Value);
        if (!result2.IsSuccess) return;

        var result3 = TestResult4(result2.Value);
        if (!result3.IsSuccess) return;

        var result4 = TestResult5(result3.Value);
        if (!result4.IsSuccess) return;
    }

    [Benchmark]
    public async Task CallAsync()
    {
        var result = await TestResult1Async();
        if (!result.IsSuccess) return;

        var result1 = await TestResult2Async(result.Value);
        if (!result1.IsSuccess) return;

        var result2 = await TestResult3Async(result1.Value);
        if (!result2.IsSuccess) return;

        var result3 = await TestResult4Async(result2.Value);
        if (!result3.IsSuccess) return;

        var result4 = await TestResult5Async(result3.Value);
        if (!result4.IsSuccess) return;
    }


    [Benchmark]
    public void PipeDelegate()
    {
        var result = TestResult1()
            .Bind(TestResult2)
            .Bind(TestResult3)
            .Bind(TestResult4)
            .Bind(TestResult5);
    }

    [Benchmark]
    public async Task PipeDelegateAsync()
    {
        var result = await TestResult1Async()
            .BindAsync(TestResult2Async)
            .BindAsync(TestResult3Async)
            .BindAsync(TestResult4Async)
            .BindAsync(TestResult5Async);
    }

    [Benchmark]
    public void Pipe()
    {
        var result = Result.Ok(10)
            .Bind(x => Result.Ok<double>(x))
            .Bind(x => Result.Ok(x.ToString()))
            .Bind(x => Result.Ok(x.Length))
            .Bind(x => Result.Ok());
    }

    [Benchmark]
    public async Task PipeAsync()
    {
        var result = TestResult1Async()
            .BindAsync(x => ValueTask.FromResult(Result.Ok<double>(x)))
            .BindAsync(x => ValueTask.FromResult(Result.Ok(x.ToString())))
            .BindAsync(x => ValueTask.FromResult(Result.Ok(x.Length)))
            .BindAsync(x => ValueTask.FromResult(Result.Ok()));
    }


    private static ValueTask<Result<int>> TestResult1Async()
    {
        return ValueTask.FromResult(Result.Ok(10));
    }
    private static ValueTask<Result<double>> TestResult2Async(int x)
    {
        return ValueTask.FromResult(Result.Ok<double>(x));
    }
    private static ValueTask<Result<string>> TestResult3Async(double x)
    {
        return ValueTask.FromResult(Result.Ok(x.ToString()));
    }
    private static ValueTask<Result<int>> TestResult4Async(string x)
    {
        return ValueTask.FromResult(Result.Ok(x.Length));
    }
    private static ValueTask<Result> TestResult5Async(int _)
    {
        return ValueTask.FromResult(Result.Ok());
    }




    private static Result<int> TestResult1()
    {
        return Result.Ok(10);
    }
    private static Result<double> TestResult2(int x)
    {
        return Result.Ok<double>(x);
    }
    private static Result<string> TestResult3(double x)
    {
        return Result.Ok(x.ToString());
    }
    private static Result<int> TestResult4(string x)
    {
        return Result.Ok(x.Length);
    }
    private static Result TestResult5(int _)
    {
        return Result.Ok();
    }
}