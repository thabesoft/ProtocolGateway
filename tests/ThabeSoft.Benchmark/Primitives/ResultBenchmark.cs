using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ThabeSoft.Primitives;



[MemoryDiagnoser(true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
#pragma warning disable RCS1102 // Make class static
public class ResultBenchmark
#pragma warning restore RCS1102 // Make class static
{
    [Benchmark(Baseline = true)]
    public static void Call()
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
    public static void PipeDelegate()
    {
        var result = TestResult1()
            .Select(TestResult2)
            .Select(TestResult3)
            .Select(TestResult4)
            .Select(TestResult5);
    }

    [Benchmark]
    public static void PipeLambda()
    {
        var result = Result.Success(10)
            .Select(static x => Result.Success<double>(x))
            .Select(static x => Result.Success(x.ToString()))
            .Select(static x => Result.Success(x.Length))
            .Select(static _ => Result.Success());
    }


    private static Result<int> TestResult1() => Result.Success(10);
    private static Result<double> TestResult2(int x) => Result.Success<double>(x);
    private static Result<string> TestResult3(double x) => Result.Success(x.ToString());
    private static Result<int> TestResult4(string x) => Result.Success(x.Length);
    private static Result TestResult5(int _) => Result.Success();
}