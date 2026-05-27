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

    extension<T>(ValueTask<T> task) where T : IResult
    {
        public async ValueTask<T> AssertIsTrue()
        {
            var result = await task;

            Assert.IsTrue(result.IsSuccess, result.Message);
            return result;
        }
    }

    extension<T>(Task<T> task) where T : IResult
    {
        public async Task<T> AssertIsTrue()
        {
            var result = await task;

            Assert.IsTrue(result.IsSuccess, result.Message);
            return result;
        }
    }
}