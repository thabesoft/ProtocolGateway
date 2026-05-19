namespace IndustrialHub.Modbus.Protocol;

public static class ModbusFunctions
{
    public const byte ReadCoils = 0x01;

    public const byte ReadDiscreteInputs = 0x02;

    public const byte ReadHoldingRegisters = 0x03;

    public const byte ReadInputRegisters = 0x04;

    public const byte WriteSingleCoil = 0x05;

    public const byte WriteSingleRegister = 0x06;

    public const byte WriteMultipleCoils = 0x0F;

    public const byte WriteMultipleRegisters = 0x10;

    public const byte ExceptionMaskCode = 0x80;
}
