namespace System;


/// <summary>
/// 深拷贝
/// </summary>
public interface IDeepCloneable<out T>
{
    /// <summary>
    /// 深拷贝
    /// </summary>
    T DeepClone();
}