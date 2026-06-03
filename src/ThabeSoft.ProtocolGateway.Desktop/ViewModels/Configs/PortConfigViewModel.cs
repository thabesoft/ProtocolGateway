using Avalonia.Threading;
using ThabeSoft.Mvvm;
using ThabeSoft.Ports;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.ViewModels;


/// <summary>
/// 端口配置
/// </summary>
public sealed class PortConfigViewModel : ViewModelBase
{
    private readonly Lock _lock = new();

    public Parity[] Paritys { get; } = [Parity.Odd, Parity.Even, Parity.Mark];
    public DuplexMode[] DuplexModes { get; } = [DuplexMode.FullDuplex, DuplexMode.HalfDuplex];
    public StopBits[] StopBitss { get; } = [StopBits.One, StopBits.OnePointFive, StopBits.Two];


    #region --SerialPort--

    public bool IsSerialPort { get; set => Apply(field, value, x => field = x); }

    // 串口名称
    public string PortName
    {
        get; private set => Change(field, value, x => field = x)
            .NotWhiteSpace(() => "名称不可为空")
            .Must(value.StartsWith("COM", StringComparison.OrdinalIgnoreCase), _ => "串口名称应以 COM 开头")
            .Apply();
    } = "None";

    // 波特率
    public int BaudRate
    {
        get; set => Change(field, value, x => field = x)
            .IsSuccess(x => Ports.BaudRate.Create(x))
            .Apply();
    } = 9600;

    // 校验模式
    public Parity Parity
    {
        get; set => Change(field, value, x => field = x)
            .IsDefined(x => $"不支持的校验模式 [{x}]")
            .Apply();
    } = Parity.None;

    // 数据位
    public int DataBits
    {
        get; set => Change(field, value, x => field = x)
            .InRange(5, 8, _ => "数据位必须在 [5~8] 之间")
            .Apply();
    } = 8;

    // 停止位
    public StopBits StopBits
    {
        get; set => Change(field, value, x => field = x)
            .IsDefined(_ => "不支持的停止位")
            .Apply();
    } = StopBits.One;

    // 双工模式
    public DuplexMode DuplexMode
    {
        get; set => Change(field, value, x => field = x)
            .IsDefined(_ => "不支持的双工模式")
            .Apply();
    } = DuplexMode.FullDuplex;

    public bool IsFullDuplexMode
    {
        get => field;
        set => Change(field, value, x => field = x)
            .AlsoNotify(nameof(DuplexMode))
            .Apply();
    }


    #endregion


    /// <summary>
    /// 从串口配置加载
    /// </summary>
    public void LoadConfig(SerialPortConfig config)
    {
        using var _ = _lock.EnterScope();

        Dispatcher.UIThread.Post(() =>
        {
            IsSerialPort = true;
            PortName = config.PortName;
            BaudRate = config.BaudRate;
            Parity = config.Parity;
            DataBits = config.DataBits;
            StopBits = config.StopBits;
            DuplexMode = config.DuplexMode;

        }, DispatcherPriority.Background);
    }
}