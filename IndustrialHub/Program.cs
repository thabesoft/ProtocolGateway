using ThabeSoft.IndustrialHub;
using System.Reactive.Linq;

Console.WriteLine("Hello, World!");



IIndustriaHub hub = null;
hub.GetTag<int>("")
    .Subscribe(x => Console.Write(x));





//MyHubContext hub = new();




//class MyHubContext : HubContext
//{
//    protected override void OnDevideModelBuilder(IDeviceModelBuilder builder)
//    {
//        builder.UseModbusRtu("COM1", 19200)
//            .WithSlave<FishTank>(1, builder =>
//            {
//                builder.HoldingRegister(x => x.Temperature, 0)
//                    .PollInterval(TimeSpan.FromSeconds(1))
//                    .MapFrom<short>(x => x / 10.0)
//                    .Filter(x => x is > 0 and < 30)
//                    .Distinct();

//                builder.Coil(x => x.Light, 0)
//                    .PollInterval(TimeSpan.FromSeconds(2))
//                    .Distinct();
//            });
//    }
//}

