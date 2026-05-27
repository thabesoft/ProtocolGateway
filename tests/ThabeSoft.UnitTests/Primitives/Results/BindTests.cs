using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ThabeSoft.Primitives.Results;

[TestClass]
public class BindTests
{
    [TestMethod(DisplayName = "Result")]
    public async Task FromResult()
    {
        var value = 123456;
        var task_value = Task.Run(() => "Task_Value");
        var value_task_value = new ValueTask<double>(3.14);

        var result = Result.Ok(10);
        var task_result = Task.Run(() => Result.Ok(3.14));
        var value_task_result = new ValueTask<Result<string>>(Result.Ok("Abc"));


        Result<int> test_result = Result.Ok(10);

        test_result.Bind(x => value).AssertIsTrue();
        await test_result.BindAsync(x => task_value).AssertIsTrue();
        await test_result.BindAsync((x, _) => task_value).AssertIsTrue();
        await test_result.BindAsync(x => value_task_value).AssertIsTrue();
        await test_result.BindAsync((x, _) => value_task_value).AssertIsTrue();


        test_result.Bind(x => result).AssertIsTrue();
        await test_result.BindAsync(x => task_result).AssertIsTrue();
        await test_result.BindAsync((x, _) => task_result).AssertIsTrue();
        await test_result.BindAsync(x => value_task_result).AssertIsTrue();
        await test_result.BindAsync((x, _) => value_task_result).AssertIsTrue();
    }

    [TestMethod(DisplayName = "Task<Result>")]
    public async Task FromTaskResult()
    {
        var value = 123456;
        var task_value = Task.Run(() => "Task_Value");
        var value_task_value = new ValueTask<double>(3.14);

        var result = Result.Ok(10);
        var task_result = Task.Run(() => Result.Ok(3.14));
        var value_task_result = new ValueTask<Result<string>>(Result.Ok("Abc"));

        Task<Result<int>> test_result = Task.Run(() => Result.Ok(10));

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

        var result = Result.Ok(10);
        var task_result = Task.Run(() => Result.Ok(3.14));
        var value_task_result = new ValueTask<Result<string>>(Result.Ok("Abc"));

        ValueTask<Result<int>> test_result = new ValueTask<Result<int>>(Result.Ok(10));

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
