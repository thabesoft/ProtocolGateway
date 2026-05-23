using ThabeSoft.ProtocolGateway.Primitives;

namespace ThabeSoft.ProtocolGateway.Tags;

/// <summary>
/// 标签信息构建器
/// </summary>
public interface ITagBuilder<TValue>
    where TValue : unmanaged
{
    ITagBuilder<TValue> Name(string name);
    ITagBuilder<TValue> Address(IAddress address);
    ITagBuilder<TValue> Converter(IValueConverter<TValue> bytesConverter);

    ITag<TValue> Build();
}
