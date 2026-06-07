using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ThabeSoft.Primitives.Results;

[TestClass]
public class BindTests
{
    [TestMethod]
    public void Select()
    {
        var value_result = Result.Success(10);

        value_result.Then(_ => Result.Success(20)).AssertIsTrue();
        value_result.Then("State", (value, state) => Result.Success($"{value}{state}")).AssertIsTrue();
    }
}
