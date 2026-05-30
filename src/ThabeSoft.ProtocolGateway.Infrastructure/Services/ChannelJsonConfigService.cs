using Microsoft.Extensions.Options;
using System.Text.Json;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Options;

namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 通道Json配置业务
/// </summary>
internal sealed class ChannelJsonConfigService(IOptions<ConfigOptions> options, IOptions<JsonSerializerOptions> jsonOptions) : IChannelConfigService
{
    public ValueTask<IReadOnlyList<ChannelConfig>> LoadAsync(CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.ChannelsFilePath;
        var json_options = jsonOptions.Value;

        return LoadFromFileAsync(config_path, json_options, cancellationToken);
    }
    public Task SaveAsync(IEnumerable<ChannelConfig> source, CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.ChannelsFilePath;
        var json_options = jsonOptions.Value;

        return SaveToFileAsync(source, config_path, json_options, cancellationToken);
    }


    public static async ValueTask<IReadOnlyList<ChannelConfig>> LoadFromFileAsync(string fiePath, JsonSerializerOptions options, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(fiePath)) return [];

        try
        {
            await using var fs = new FileStream(fiePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            return await LoadAsync(fs, options, cancellationToken);
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
    public static async Task SaveToFileAsync(IEnumerable<ChannelConfig> source, string filePath, JsonSerializerOptions options, CancellationToken cancellationToken = default)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

            await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await SaveAsync(source, stream, options, cancellationToken);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Failed to write config file: {filePath}", ex);
        }
    }


    public static async ValueTask<IReadOnlyList<ChannelConfig>> LoadAsync(Stream stream, JsonSerializerOptions options, CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync<ChannelConfig[]>(stream, options, cancellationToken: cancellationToken) ?? [];
    }
    public static Task SaveAsync(IEnumerable<ChannelConfig> source, Stream stream, JsonSerializerOptions options, CancellationToken cancellationToken = default)
    {
        var values = source.OrderBy(x => x.Name).ToArray();
        return JsonSerializer.SerializeAsync(stream, values, options, cancellationToken: cancellationToken);
    }
}