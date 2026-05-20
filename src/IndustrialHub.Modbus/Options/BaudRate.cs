using System.Diagnostics;

namespace IndustrialHub.Modbus.Options;


/// <summary>
/// 波特率
/// </summary>
public readonly struct BaudRate : IEquatable<BaudRate>
{
    public static BaudRate Empty;

    private readonly int _value;
    private BaudRate(int value) => _value = value;


    public override bool Equals(object obj) => obj is BaudRate other && Equals(other);
    public bool Equals(BaudRate other) => other._value.Equals(_value);
    public override int GetHashCode() => _value.GetHashCode();
    public override string ToString() => $"{_value}bps";


    public static bool operator ==(BaudRate left, BaudRate right) => left.Equals(right);
    public static bool operator !=(BaudRate left, BaudRate right) => !left.Equals(right);

    public static explicit operator BaudRate(int value) => Create(value);
    public static implicit operator int(BaudRate value) => value._value;



    public static BaudRate Rate300 { get; } = new BaudRate(300);
    public static BaudRate Rate600 { get; } = new BaudRate(600);
    public static BaudRate Rate1200 { get; } = new BaudRate(1200);
    public static BaudRate Rate2400 { get; } = new BaudRate(2400);
    public static BaudRate Rate4800 { get; } = new BaudRate(4800);
    public static BaudRate Rate9600 { get; } = new BaudRate(9600);
    public static BaudRate Rate19200 { get; } = new BaudRate(38400);
    public static BaudRate Rate38400 { get; } = new BaudRate(38400);
    public static BaudRate Rate115200 { get; } = new BaudRate(115200);



    private static readonly int[] CommonBaudRate =
    [
        300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200, 128000, 256000, 460800
    ];
    public static BaudRate Create(int baudRate)
    {
        if (baudRate <= 0)
        {
            throw new ArgumentException(nameof(baudRate), "波特率不可小于等于0");
        }

        if (!CommonBaudRate.Contains(baudRate))
        {
            Debug.WriteLine($"警告: 波特率 {baudRate} 不是标准值，请确认硬件支持");
        }

        return new BaudRate(baudRate);
    }
    public static bool TryCreate(int baudRate, out BaudRate result)
    {
        result = default;
        if (baudRate <= 0) return false;

        if (!CommonBaudRate.Contains(baudRate))
        {
            Debug.WriteLine($"警告: 波特率 {baudRate} 不是标准值，请确认硬件支持");
        }

        result = new BaudRate(baudRate);
        return true;
    }
}