using System.Text.Json;
using ThabeSoft.ProtocolGateway.Configuration;

namespace ThabeSoft.ProtocolGateway.Infrastructure.Json;


/// <summary>
/// 配置Json序列化器
/// </summary>
internal sealed class ConfigJsonSerializer(ConfigJsonSerializerContext context)
{
    public async ValueTask<IReadOnlyList<ChannelConfig>> LoadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync(stream, context.ChannelConfigArray, cancellationToken: cancellationToken) ?? [];
    }
    public Task SaveAsync(IEnumerable<ChannelConfig> source, Stream stream, CancellationToken cancellationToken = default)
    {
        var values = source.OrderBy(x => x.Name).ToArray();
        return JsonSerializer.SerializeAsync(stream, values, context.ChannelConfigArray, cancellationToken: cancellationToken);
    }
}


internal static class ConfigJsonSerializerExtensions
{
    extension(ConfigJsonSerializer serializer)
    {
        public async ValueTask<IReadOnlyList<ChannelConfig>> LoadFromFileAsync(string fiePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(fiePath)) return [];

            try
            {
                await using var fs = new FileStream(fiePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                return await serializer.LoadAsync(fs, cancellationToken);
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
                await serializer.SaveAsync(source, stream, cancellationToken);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Failed to write config file: {filePath}", ex);
            }
        }
    }
}