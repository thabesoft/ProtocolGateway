using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ThabeSoft.Modbus.Encoding;
using ThabeSoft.Modbus.Headers;
using ThabeSoft.Modbus.Layouts;

namespace ThabeSoft.Benchmark.Modbus.Encoding;


[MemoryDiagnoser(true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public  class RtuCodecBenchmark
{
    private readonly RtuReadRequestLayout _layout = RtuReadRequestLayout.Instance;
    private readonly ReadRequestHeader _requestHeader = ReadRequestHeader.Coils(10, 100, 10).Value;
    private readonly byte[] _buffer = new byte[256];
    private readonly byte[] _response = [0x01, 0x01, 0x00, 0x64, 0x00, 0x0A, 0xFD, 0xD2];


    [Benchmark]
    public void EncodeReadCoils()
    {
        RtuMasterReadCodec.EncodeRequest(_buffer, _requestHeader, _layout);
    }

    [Benchmark]
    public void DecodeReadCoils()
    {
        RtuSlaveReadCodec.DecodeRequest(_response, _layout);
    }

    [Benchmark]
    public void RoundTrip()
    {
        Span<byte> buffer = stackalloc byte[_layout.TotalLength];
        RtuMasterReadCodec.EncodeRequest(buffer, _requestHeader, _layout);
        RtuSlaveReadCodec.DecodeRequest(buffer, _layout);
    }
}
