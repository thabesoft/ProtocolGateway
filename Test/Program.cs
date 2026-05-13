using ThabeSoft.IndustriaHub;

Console.WriteLine("Hello, World!");



IIndustriaHub hub = null!;
var device = hub.UpdateDevice("FishTank", new ModbusRtuOptions()
{
    Port = "COM2",
    ...
    Tags = new Tag[]
    {
        new ColiTag() { Name = "Switch", Address = 0x01 }
        new RegisterTag() { Address = 0x01, Qua = 2, Endian = EndianType.Big }
    }
});

device.Subcribe("Switch", x => Console.WriteLine(x));
device.Handle("Switch", true);









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

