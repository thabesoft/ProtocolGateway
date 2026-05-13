using System.Linq.Expressions;
using System.Numerics;

namespace IndustrialHub.Modbus;

/// <summary>
/// 设备构建器
/// </summary>
public interface IDeviceBuilder
{
    /// <summary>
    /// 配置从站
    /// </summary>
    /// <typeparam name="TModel">数据模型</typeparam>
    /// <param name="slaveId">从站Id</param>
    /// <param name="slaveBuilder">从站模型映射构建</param>
    /// <returns></returns>
    IDeviceBuilder WithSlave<TModel>(byte slaveId, Action<ITagBuilder<TModel>> slaveBuilder) where TModel : notnull;
}

/// <summary>
/// 标签构建器
/// </summary>
/// <typeparam name="TModel">数据模型</typeparam>
public interface ITagBuilder<TModel> where TModel : notnull
{
    /// <summary>
    /// 配置线圈
    /// </summary>
    /// <param name="tagSelector">映射源</param>
    /// <param name="address"></param>
    /// <returns></returns>
    IColiBuilder Coil(Expression<Func<TModel, bool>> tagSelector, ushort address);
    IColiBuilder DiscreteInputs(Expression<Func<TModel, bool>> tagSelector, ushort address);

    IRegisterBuilder<TTag> HoldingRegister<TTag>(Expression<Func<TModel, TTag>> tagSelector, ushort address) where TTag : struct, INumber<TTag>;
    IRegisterBuilder<TTag> InputRegister<TTag>(Expression<Func<TModel, TTag>> tagSelector, ushort address) where TTag : struct, INumber<TTag>;
}                   


public interface IRegisterBuilder<TTag>
{
    IRegisterBuilder<TTag> Endian(IEndianConverter converter);
    IRegisterBuilder<TTag> PollInterval(TimeSpan interval);
                  
    IRegisterBuilder<TTag> MapFrom<TPlcType>(Func<TPlcType, TTag> mapper);
    IRegisterBuilder<TTag> Filter(Func<TTag, bool> fliter, Action<TTag>? onDiscarded = null);
    IRegisterBuilder<TTag> Distinct();
}

public interface IColiBuilder
{
    IColiBuilder PollInterval(TimeSpan interval);
    IColiBuilder Distinct();
}