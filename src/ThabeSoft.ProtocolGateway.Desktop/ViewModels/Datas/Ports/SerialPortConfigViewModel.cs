using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ThabeSoft.Ports;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.ViewModels;


[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public sealed partial class SerialPortConfigViewModel : ValidatableObservableObject, IPortViewModel
{
    public Parity[] Paritys { get; } = [Parity.Odd, Parity.Even, Parity.Mark];
    public DuplexMode[] DuplexModes { get; } = [DuplexMode.FullDuplex, DuplexMode.HalfDuplex];
    public StopBits[] StopBitss { get; } = [StopBits.One, StopBits.OnePointFive, StopBits.Two];


    // 串口名称
    [ObservableProperty]
    public partial string PortName { get; private set; } = "None";

    // 波特率
    [ObservableProperty]
    public partial int BaudRate { get; set; } = 9600;

    // 校验模式
    [ObservableProperty]
    public partial Parity Parity { get; set; } = Parity.None;

    // 数据位
    [ObservableProperty]
    public partial int DataBits { get; set; } = 8;

    // 停止位
    [ObservableProperty]
    public partial StopBits StopBits { get; set; } = StopBits.One;

    // 双工模式
    [ObservableProperty]
    public partial DuplexMode DuplexMode { get; set; } = DuplexMode.FullDuplex;



    public void LoadConfig(SerialPortConfig config)
    {

        PortName = config.PortName;
        BaudRate = config.BaudRate;
        Parity = config.Parity;
        DataBits = config.DataBits;
        StopBits = config.StopBits;
        DuplexMode = config.DuplexMode;
    }



    partial void OnPortNameChanging(string value)
    {
        ClearError(nameof(PortName));

        if (string.IsNullOrWhiteSpace(value))
        {
            AddError("串口名称不可为空", nameof(PortName));
        }
        if (!value.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
        {
            AddError("串口名称应以 COM 开头", nameof(PortName));
        }
    }

    partial void OnBaudRateChanging(int value)
    {
        ClearError(nameof(BaudRate));

        var result = Ports.BaudRate.Create(value);
        if (!result.IsSuccess)
        {
            AddError(result.Message!, nameof(BaudRate));
        }
    }
}