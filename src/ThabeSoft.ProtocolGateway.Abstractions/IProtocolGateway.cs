namespace ThabeSoft.ProtocolGateway;


public interface IProtocolGateway
{
    void AddDeviceContext<TDevice>(Action<IDeviceOptionsBuilders<TDevice>> optionActinos);

    IReader GetReader<TDevice>();
    IWriter GetWriter<TDevice>();

    ISubscriber GetSubscriber();
    ISubscriber GetSubscriber<TDevice>();
}
