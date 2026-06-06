namespace ThabeSoft.Primitives.Results;

public static class AssertExtensions
{
    extension<T>(T result) where T : IResult
    {
        public T AssertIsTrue()
        {
            Assert.IsTrue(result.IsSuccess, result.Message);
            return result;
        }
    }
}