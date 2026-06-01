using Microsoft.Extensions.Options;
using System.Text.Json;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Internal.Json;
using ThabeSoft.ProtocolGateway.Options;

namespace ThabeSoft.ProtocolGateway.Internal;

internal sealed class ChannelJsonConfigService(IOptions<ConfigOptions> options) : IChannelConfigService
{
    private readonly JsonContext _context = new();

    public ValueTask<IReadOnlyList<ChannelConfig>> LoadAsync(CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.ChannelsFilePath;
        return LoadFromFileAsync(config_path, cancellationToken);
    }
    public Task SaveAsync(IEnumerable<ChannelConfig> source, CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.ChannelsFilePath;
        return SaveToFileAsync(source, config_path, cancellationToken);
    }


    public async ValueTask<IReadOnlyList<ChannelConfig>> LoadFromFileAsync(string fiePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(fiePath)) return [];

        try
        {
            await using var fs = new FileStream(fiePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            return await LoadAsync(fs, cancellationToken);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Failed to load config from {fiePath}", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Invalid JSON format in {fiePath}", ex);
        }
    }
    public async Task SaveToFileAsync(IEnumerable<ChannelConfig> source, string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

            await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await SaveAsync(source, stream, cancellationToken);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Failed to write config file: {filePath}", ex);
        }
    }


    public async ValueTask<IReadOnlyList<ChannelConfig>> LoadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var type_info = _context.GetTypeInfo(typeof(ChannelConfig[]));
        if (type_info is null) throw new ArgumentNullException(nameof(type_info), "无法获取序列化类型信息");

        var json = await JsonSerializer.DeserializeAsync(stream, type_info, cancellationToken: cancellationToken);
        return json as ChannelConfig[] ?? [];
    }
    public Task SaveAsync(IEnumerable<ChannelConfig> source, Stream stream, CancellationToken cancellationToken = default)
    {
        var type_info = _context.GetTypeInfo(typeof(ChannelConfig[]));
        if (type_info is null) throw new ArgumentNullException(nameof(type_info), "无法获取序列化类型信息");

        var values = source.OrderBy(x => x.Name).ToArray();
        return JsonSerializer.SerializeAsync(stream, values, type_info, cancellationToken: cancellationToken);
    }
}