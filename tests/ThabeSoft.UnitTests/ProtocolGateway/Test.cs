using ThabeSoft.ProtocolGateway.Modbus;

namespace ThabeSoft.ProtocolGateway;

internal static class Test
{
    public static async Task Process()
    {
        IProtocolGateway gateway = default!;

        var result = await gateway.ReadAsync(ModbusRoutableTag.ReadHoldingRegisterQWord(1, 100));
        Console.Write(result.Value);
    }
}