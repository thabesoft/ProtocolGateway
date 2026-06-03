namespace ThabeSoft.Primitives.Results;


[TestClass]
public class TapTests
{
    [TestMethod(DisplayName = "Result")]
    public async Task FromResult()
    {
        Result<int> test_result = Result.Success(10);

        test_result.Tap(x => Console.WriteLine($"同步方法: {x}")).AssertIsTrue();
        await test_result.TapAsync(async x => Console.WriteLine($"异步方法: {x}")).AssertIsTrue();
        await test_result.TapAsync(async (x, _) => Console.WriteLine($"异步取消方法: {x}")).AssertIsTrue();
    }

    [TestMethod(DisplayName = "Task<Result>")]
    public async Task FromTaskResult()
    {
        Task<Result<int>> test_result = Task.Run(() => Result.Success(10));

        await test_result.TapAsync(x => Console.WriteLine($"同步方法: {x}")).AssertIsTrue();
        await test_result.TapAsync(async x => Console.WriteLine($"异步方法: {x}")).AssertIsTrue();
        await test_result.TapAsync(async (x, _) => Console.WriteLine($"异步取消方法: {x}")).AssertIsTrue();
    }

    [TestMethod(DisplayName = "ValueTask<Result>")]
    public async Task FromValueTaskResult()
    {
        ValueTask<Result<int>> test_result = new ValueTask<Result<int>>(Result.Success(10));

        await test_result.TapAsync(x => Console.WriteLine($"同步方法: {x}")).AssertIsTrue();
        await test_result.TapAsync(async x => Console.WriteLine($"异步方法: {x}")).AssertIsTrue();
        await test_result.TapAsync(async (x, _) => Console.WriteLine($"异步取消方法: {x}")).AssertIsTrue();
    }
}