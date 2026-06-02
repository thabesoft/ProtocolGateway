using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.JsonConverters;
using ThabeSoft.ProtocolGateway.Options;

namespace ThabeSoft.ProtocolGateway;


/// <summary>
/// 通道配置
/// </summary>
public interface IChannelConfigRepository
{
    /// <summary>
    /// 加载所有
    /// </summary>
    ValueTask<IReadOnlyList<ChannelConfig>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存所有
    /// </summary>
    Task SaveAllAsync(IEnumerable<ChannelConfig> source, CancellationToken cancellationToken = default);
}

internal sealed class ChannelConfigRepository(IOptions<ConfigOptions> options) : IChannelConfigRepository
{
    public ValueTask<IReadOnlyList<ChannelConfig>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.ChannelsFilePath;
        var type_info = JsonContext.Default.ChannelConfigArray;

        return LoadFromFileAsync(config_path, type_info, cancellationToken);
    }
    public Task SaveAllAsync(IEnumerable<ChannelConfig> source, CancellationToken cancellationToken = default)
    {
        var config_path = options.Value.ChannelsFilePath;
        var type_info = JsonContext.Default.ChannelConfigArray;

        return SaveToFileAsync(source, config_path, type_info, cancellationToken);
    }


    public static async ValueTask<IReadOnlyList<ChannelConfig>> LoadFromFileAsync(string fiePath, JsonTypeInfo<ChannelConfig[]> typeInfo, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(fiePath)) return [];

        try
        {
            await using var fs = new FileStream(fiePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            return await LoadAsync(fs, typeInfo, cancellationToken);
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
    public static async Task SaveToFileAsync(IEnumerable<ChannelConfig> source, string filePath, JsonTypeInfo<ChannelConfig[]> typeInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

            await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await SaveAsync(source, stream, typeInfo, cancellationToken);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Failed to write config file: {filePath}", ex);
        }
    }


    public static async ValueTask<IReadOnlyList<ChannelConfig>> LoadAsync(Stream stream, JsonTypeInfo<ChannelConfig[]> typeInfo, CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync(stream, typeInfo, cancellationToken: cancellationToken) ?? [];
    }
    public static Task SaveAsync(IEnumerable<ChannelConfig> source, Stream stream, JsonTypeInfo<ChannelConfig[]> typeInfo, CancellationToken cancellationToken = default)
    {
        var values = source.OrderBy(x => x.Name).ToArray();
        return JsonSerializer.SerializeAsync(stream, values, typeInfo, cancellationToken: cancellationToken);
    }
}
