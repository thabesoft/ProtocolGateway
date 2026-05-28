using Avalonia.Controls;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Desktop.ViewModels;

namespace ThabeSoft.ProtocolGateway.Desktop.Views;


public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        var vm = new MainViewModel();

        NavigationItemViewModel.Create<MainViewModel>("Main", "通道")
            .Tap(vm.NavigationItems.Add);

        NavigationItemViewModel.Create<MainViewModel>("Main", "Modbus")
            .Tap(vm.NavigationItems.Add);

        NavigationItemViewModel.Create<MainViewModel>("Main", "扩展")
            .Tap(vm.NavigationItems.Add);

        DataContext = vm;
    }
}