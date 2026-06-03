using System.Diagnostics;

namespace ThabeSoft.Primitives.Results;


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
                Result.Error("调试状态下抛出异常"));
        }
        else
        {
            // 无调试器时，测试 Result 行为
            var result = Result.Error("错误消息");
            Assert.IsFalse(result.IsSuccess);
        }
#else
    Assert.Inconclusive("Debug 配置下才运行此测试");
#endif
    }
}
