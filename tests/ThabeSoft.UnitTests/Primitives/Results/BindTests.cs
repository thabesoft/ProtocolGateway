using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ThabeSoft.Primitives.Results;

[TestClass]
public class BindTests
{
    [TestMethod(DisplayName = "Result")]
    public async Task FromResult()
    {
        var result = Result.Success(10);
        var task_result = Task.Run(() => Result.Success(3.14));
        var value_task_result = new ValueTask<Result<string>>(Result.Success("Abc"));




        result.Select(_ => result).AssertIsTrue();
        result.Select("State", (_, _) => result).AssertIsTrue();

        _ = await result.BindAsync(_ => task_result).AssertIsTrue();
        await result.BindAsync((_, _) => task_result).AssertIsTrue();
        await result.BindAsync("State", (_, _, _) => task_result).AssertIsTrue();

        await result.BindAsync(_ => value_task_result).AssertIsTrue();
        await result.BindAsync((_, _) => value_task_result).AssertIsTrue();
        await result.BindAsync("State", (_, _, _) => value_task_result).AssertIsTrue();
    }

    [TestMethod(DisplayName = "Task<Result>")]
    public async Task FromTaskResult()
    {
        var value = 123456;
        var task_value = Task.Run(() => "Task_Value");
        var value_task_value = new ValueTask<double>(3.14);

        var result = Result.Success(10);
        var task_result = Task.Run(() => Result.Success(3.14));
        var value_task_result = new ValueTask<Result<string>>(Result.Success("Abc"));

        Task<Result<int>> test_result = Task.Run(() => Result.Success(10));

        await test_result.BindAsync(x => value).AssertIsTrue();
        await test_result.BindAsync(x => task_value).AssertIsTrue();
        await test_result.BindAsync((x, _) => task_value).AssertIsTrue();
        await test_result.BindAsync(x => value_task_value).AssertIsTrue();
        await test_result.BindAsync((x, _) => value_task_value).AssertIsTrue();


        await test_result.BindAsync(x => result).AssertIsTrue();
        await test_result.BindAsync(x => task_result).AssertIsTrue();
        await test_result.BindAsync((x, _) => task_result).AssertIsTrue();
        await test_result.BindAsync(x => value_task_result).AssertIsTrue();
        await test_result.BindAsync((x, _) => value_task_result).AssertIsTrue();
    }

    [TestMethod(DisplayName = "ValueTask<Result>")]
    public async Task FromValueTaskResult()
    {
        var value = 123456;
        var task_value = Task.Run(() => "Task_Value");
        var value_task_value = new ValueTask<double>(3.14);

        var result = Result.Success(10);
        var task_result = Task.Run(() => Result.Success(3.14));
        var value_task_result = new ValueTask<Result<string>>(Result.Success("Abc"));

        ValueTask<Result<int>> test_result = new ValueTask<Result<int>>(Result.Success(10));

        await test_result.BindAsync(x => value).AssertIsTrue();
        await test_result.BindAsync(x => task_value).AssertIsTrue();
        await test_result.BindAsync((x, _) => task_value).AssertIsTrue();
        await test_result.BindAsync(x => value_task_value).AssertIsTrue();
        await test_result.BindAsync((x, _) => value_task_value).AssertIsTrue();


        await test_result.BindAsync(x => result).AssertIsTrue();
        await test_result.BindAsync(x => task_result).AssertIsTrue();
        await test_result.BindAsync((x, _) => task_result).AssertIsTrue();
        await test_result.BindAsync(x => value_task_result).AssertIsTrue();
        await test_result.BindAsync((x, _) => value_task_result).AssertIsTrue();
    }
}
