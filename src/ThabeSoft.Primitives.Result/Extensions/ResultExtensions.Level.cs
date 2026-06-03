namespace ThabeSoft.Primitives;


/// <summary>
/// Template
/// </summary>
public static partial class ResultExtensions
{
    // Value
    extension<T>(T result) where T : IResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess => result.Level is ResultLevel.Info or ResultLevel.Success;

        /// <summary>
        /// 是否存在问题
        /// </summary>
        public bool IsProblem => result.Level >= ResultLevel.Warning;

        /// <summary>
        /// 是否失败
        /// </summary>
        public bool IsFailure => result.Level == ResultLevel.Error;
    }
}
