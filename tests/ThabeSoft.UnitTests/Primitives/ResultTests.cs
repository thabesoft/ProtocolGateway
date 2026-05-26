using System.Diagnostics;
using ThabeSoft.Primitives;

namespace ThabeSoft.ProtocolGateway.Primitives;


[TestClass]
public sealed class ResultTests
{

    [TestMethod(DisplayName = "debug 状态下抛出 异常")]
    public async Task Debugger_Throw_Exception()
    {
#if DEBUG
        if (Debugger.IsAttached)
        {
            Assert.Throws<ResultException>(() =>
                Result.InvalidData("调试状态下抛出异常"));
        }
        else
        {
            // 无调试器时，测试 Result 行为
            var result = Result.InvalidData("错误消息");
            Assert.IsFalse(result.IsSuccess);
        }
#else
    Assert.Inconclusive("Debug 配置下才运行此测试");
#endif
    }


    [TestMethod(DisplayName = "Release 状态下返回 Result.Error")]
    public void Release_Return_Error()
    {
        var result = Result.InvalidData("错误消息");
        Assert.IsFalse(result, result.Message);
    }


    [TestMethod]
    public void Zip()
    {
        var result = Result.Ok(123).Zip(Result.Ok("Str")).Zip(Result.Ok(3.14));

        Assert.AreEqual(123, result.Value.Item1.Item1);
        Assert.AreEqual("Str", result.Value.Item1.Item2);
        Assert.AreEqual(3.14, result.Value.Item2);
    }


    [TestMethod]
    public async Task Then()
    {
        Assert.IsTrue(await Result.Success
            .Then(() => Result.Ok(10))
            .ThenAsync(() => ValueTaskResult(Result.Ok("HelloWorld"))
            .ThenAsync(() => TaskResult(Result.Ok(10000))))
        );

        Assert.IsTrue(await Result.Ok('A')
            .Then(() => Result.Ok(0x001))
            .Then(() => Result.Ok(3.14))
            .ThenAsync(() => ValueTaskResult(Result.Ok("HelloWorld")))
            .ThenAsync(() => TaskResult(Result.Ok(10000)))
        );

        Assert.IsTrue(await Result.Ok('A')
            .Map(() => Result.Ok(3.14))
            .ThenAsync(ct => ValueTaskResult(Result.Ok($"HelloWorld: {ct.GetHashCode()}")))
            .ThenAsync(ct => TaskResult(Result.Ok(10000 + ct.GetHashCode())))
        );
    }

    [TestMethod]
    public async Task Bind()
    {
        var test1 = await Result.Ok('A')
            .Bind(x => Result.Ok(x.GetHashCode() + 3.14))
            .BindAsync(x => ValueTaskResult(Result.Ok($"HelloWorld: {x}")))
            .BindAsync(x => TaskResult(Result.Ok(10000 + x.GetHashCode())));
        Assert.IsTrue(test1, test1.Message);


        var test2 = await Result.Ok('A')
            .Bind(x => Result.Ok(x.GetHashCode() + 3.14))
            .BindAsync((x, ct) => ValueTaskResult(Result.Ok($"HelloWorld: {x}")))
            .BindAsync((x, ct) => TaskResult(Result.Ok(10000 + x.GetHashCode())));
        Assert.IsTrue(test2, test2.Message);
    }

    [TestMethod]
    public async Task Map()
    {
        var test1 = await Result.Ok('A')
            .Bind(x => Result.Ok(x.GetHashCode() + 3.14))
            .BindAsync(x => ValueTaskResult(Result.Ok($"HelloWorld: {x}")))
            .BindAsync(x => TaskResult(Result.Ok(10000 + x.GetHashCode())));
        Assert.IsTrue(test1, test1.Message);


        var test2 = await Result.Ok('A')
            .Bind(x => Result.Ok(x.GetHashCode() + 3.14))
            .BindAsync((x, ct) => ValueTaskResult(Result.Ok($"HelloWorld: {x}")))
            .BindAsync((x, ct) => TaskResult(Result.Ok(10000 + x.GetHashCode())));
        Assert.IsTrue(test2, test2.Message);
    }

    [TestMethod]
    public async Task Where()
    {
        Assert.IsTrue(Result.Ok(1000).Where(x => x > 50));
        Assert.IsTrue(Result.Ok(2000).Where(x => x % 2 == 0).Where(x => x < 5000));
    }


    private static async ValueTask<Result<T>> ValueTaskResult<T>(Result<T> result) => result;
    private static async Task<Result<T>> TaskResult<T>(Result<T> result) => result;
}
