using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ThabeSoft.Primitives.Results;


[TestClass]
public class ThenTests
{
    [TestMethod(DisplayName = "Result")]
    public async Task FromResult()
    {
        const int value = 123456;
        var task_value = Task.Run(() => "Task_Value");
        var value_task_value = new ValueTask<double>(3.14);

        var result = Result.Success(10);
        var task_result = Task.Run(() => Result.Success(3.14));
        var value_task_result = new ValueTask<Result<string>>(Result.Success("Abc"));


        Result test_result = Result.Success();

        test_result.Then(value).AssertIsTrue();
        test_result.Then(() => value).AssertIsTrue();

        test_result.Then(result).AssertIsTrue();
        test_result.Then(() => result).AssertIsTrue();

        await test_result.ThenAsync(() => task_value);
        await test_result.ThenAsync(_ => task_value, TestContext.CancellationToken);

        test_result.Then(() => result);

        await test_result.ThenAsync(() => value_task_value);
        await test_result.ThenAsync(_ => value_task_value);

        await test_result.ThenAsync(() => task_result).AssertIsTrue();
        await test_result.ThenAsync(_ => task_result).AssertIsTrue();

        await test_result.ThenAsync(() => value_task_result).AssertIsTrue();
        await test_result.ThenAsync(ct => value_task_result).AssertIsTrue();


        Result<int> test_result2 = Result.Success(10);


        test_result2.Then(value).AssertIsTrue();
        test_result2.Then(() => value).AssertIsTrue();

        test_result2.Then(result).AssertIsTrue();
        test_result2.Then(() => result).AssertIsTrue();

        await test_result2.ThenAsync(() => task_value);
        await test_result2.ThenAsync(_ => task_value);

        await test_result2.ThenAsync(() => value_task_value);
        await test_result2.ThenAsync(_ => value_task_value);

        await test_result2.ThenAsync(() => task_result).AssertIsTrue();
        await test_result2.ThenAsync(_ => task_result).AssertIsTrue();

        await test_result2.ThenAsync(() => value_task_value).AssertIsTrue();
        await test_result2.ThenAsync(ct => value_task_value).AssertIsTrue();
    }

    [TestMethod(DisplayName = "Task<Result>")]
    public async Task FromTaskResult()
    {
        var value = 123456;
        var task_value = Task.Run(() => "Task_Value");
        var value_task_value = new ValueTask<Result<double>>(Result.Success(3.14));

        var result = Result.Success(10);
        var task_result = Task.Run(() => Result.Success(3.14));
        var value_task_result = new ValueTask<Result<string>>(Result.Success("Abc"));

        Task<Result> test_result = Task.Run(Result.Success);

        await test_result.ThenAsync(value).AssertIsTrue();
        await test_result.ThenAsync(() => value).AssertIsTrue();

        await test_result.ThenAsync(result).AssertIsTrue();
        await test_result.ThenAsync(() => result).AssertIsTrue();

        await test_result.ThenAsync(() => task_value);
        await test_result.ThenAsync(_ => task_value);

        await test_result.ThenAsync(() => result);

        await test_result.ThenAsync(() => value_task_value);
        await test_result.ThenAsync(_ => value_task_value);

        await test_result.ThenAsync(() => task_result).AssertIsTrue();
        await test_result.ThenAsync(_ => task_result).AssertIsTrue();

        await test_result.ThenAsync(() => value_task_result).AssertIsTrue();
        await test_result.ThenAsync(ct => value_task_result).AssertIsTrue();


        Result<int> test_result2 = Result.Success(10);


        test_result2.Then(value).AssertIsTrue();
        test_result2.Then(() => value).AssertIsTrue();

        test_result2.Then(result).AssertIsTrue();
        test_result2.Then(() => result).AssertIsTrue();

        await test_result2.ThenAsync(() => task_value);
        await test_result2.ThenAsync(_ => task_value);

        await test_result2.ThenAsync(() => value_task_value);
        await test_result2.ThenAsync(_ => value_task_value);

        await test_result2.ThenAsync(() => task_result).AssertIsTrue();
        await test_result2.ThenAsync(_ => task_result).AssertIsTrue();

        await test_result2.ThenAsync(() => value_task_value).AssertIsTrue();
        await test_result2.ThenAsync(ct => value_task_value).AssertIsTrue();
    }

    [TestMethod(DisplayName = "ValueTask<Result>")]
    public async Task FromValueTaskResult()
    {
        var value = 123456;
        var task_value = Task.Run(() => "Task_Value");
        var value_task_value = new ValueTask<Result<double>>(Result.Success(3.14));

        var result = Result.Success(10);
        var task_result = Task.Run(() => Result.Success(3.14));
        var value_task_result = new ValueTask<Result<string>>(Result.Success("Abc"));

        ValueTask<Result> test_result = new ValueTask<Result>(Result.Success());

        await test_result.ThenAsync(value).AssertIsTrue();
        await test_result.ThenAsync(() => value).AssertIsTrue();

        await test_result.ThenAsync(result).AssertIsTrue();
        await test_result.ThenAsync(() => result).AssertIsTrue();

        await test_result.ThenAsync(() => task_value);
        await test_result.ThenAsync(_ => task_value);

        await test_result.ThenAsync(() => result);

        await test_result.ThenAsync(() => value_task_value);
        await test_result.ThenAsync(_ => value_task_value);

        await test_result.ThenAsync(() => task_result).AssertIsTrue();
        await test_result.ThenAsync(_ => task_result).AssertIsTrue();

        await test_result.ThenAsync(() => value_task_result).AssertIsTrue();
        await test_result.ThenAsync(ct => value_task_result).AssertIsTrue();


        Result<int> test_result2 = Result.Success(10);


        test_result2.Then(value).AssertIsTrue();
        test_result2.Then(() => value).AssertIsTrue();

        test_result2.Then(result).AssertIsTrue();
        test_result2.Then(() => result).AssertIsTrue();

        await test_result2.ThenAsync(() => task_value);
        await test_result2.ThenAsync(_ => task_value);

        await test_result2.ThenAsync(() => value_task_value);
        await test_result2.ThenAsync(_ => value_task_value);

        await test_result2.ThenAsync(() => task_result).AssertIsTrue();
        await test_result2.ThenAsync(_ => task_result).AssertIsTrue();

        await test_result2.ThenAsync(() => value_task_value).AssertIsTrue();
        await test_result2.ThenAsync(ct => value_task_value).AssertIsTrue();
    }

    public TestContext TestContext { get; set; }
}