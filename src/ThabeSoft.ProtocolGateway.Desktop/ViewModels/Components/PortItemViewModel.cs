using CommunityToolkit.Mvvm.ComponentModel;
using ThabeSoft.Ports;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.ViewModels.Components;


/// <summary>
/// 运行时端口视图模型
/// </summary>
public sealed partial class PortItemViewModel : ViewModel
{
    private readonly Lock _lock = new();

    public Parity[] Paritys { get; } = [Parity.Odd, Parity.Even, Parity.Mark];
    public DuplexMode[] DuplexModes { get; } = [DuplexMode.FullDuplex, DuplexMode.HalfDuplex];
    public StopBits[] StopBitss { get; } = [StopBits.One, StopBits.OnePointFive, StopBits.Two];


    #region --SerialPort--

    // 是否是串口
    [ObservableProperty]
    public partial bool IsSerialPort { get; private set; }

    // 串口名称
    [ObservableProperty]
    public partial string? PortName { get; private set; }

    // 波特率
    [ObservableProperty]
    public partial int BaudRate { get; private set; }

    // 校验模式
    [ObservableProperty]
    public partial Parity Parity { get; private set; }

    // 数据位
    [ObservableProperty]
    public partial int DataBits { get; private set; }

    // 停止位
    [ObservableProperty]
    public partial StopBits StopBits { get; private set; }

    // 双工模式
    [ObservableProperty]
    public partial DuplexMode DuplexMode { get; private set; }

    // 是否是全双工
    [ObservableProperty]
    public partial bool IsFullDuplexMode { get; private set; }


    #endregion


    public PortItemViewModel()
    {

    }
    public PortItemViewModel(IRuntimePort runtimePort)
    {
        UpdateRuntimePort(runtimePort);
    }

    public void UpdateRuntimePort(IRuntimePort runtimePort)
    {
        var config = runtimePort.Config;

        if (runtimePort.Config is ISerialPortConfig serialPort)
        {
            IsSerialPort = true;
            PortName = serialPort.PortName;
            BaudRate = serialPort.BaudRate;
            Parity = serialPort.Parity;
            DataBits = serialPort.DataBits;
            StopBits = serialPort.StopBits;
            DuplexMode = serialPort.DuplexMode;
        }
    }


    /// <summary>
    /// 从配置加载
    /// </summary>
    public void LoadConfig(IPortConfig config)
    {
        if(config is ISerialPortConfig serialPortConfig)
        {
            LoadConfig(serialPortConfig);
        }
    }

    /// <summary>
    /// 从串口配置加载
    /// </summary>
    public void LoadConfig(ISerialPortConfig config)
    {
        using var _ = _lock.EnterScope();

        IsSerialPort = true;
        PortName = config.PortName;
        BaudRate = config.BaudRate;
        Parity = config.Parity;
        DataBits = config.DataBits;
        StopBits = config.StopBits;
        DuplexMode = config.DuplexMode;
    }
}