using System.Text.Json;
using ThabeSoft.ProtocolGateway.Infrastructure.Json;

namespace ThabeSoft.ProtocolGateway.Configuration.Internal.Json;


/// <summary>
/// 配置Json序列化器
/// </summary>
internal sealed class ConfigJsonSerializer(ConfigJsonSerializerContext context)
{
    public ValueTask<GatewayConfig?> LoadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.DeserializeAsync(stream, context.GatewayConfig, cancellationToken: cancellationToken);
    }
    public Task SaveAsync(IGatewayConfig config, Stream stream, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.SerializeAsync(stream, config, context.GatewayConfig, cancellationToken: cancellationToken);
    }
}


internal static class ConfigJsonSerializerExtensions
{
    extension(ConfigJsonSerializer serializer)
    {
        public async ValueTask<GatewayConfig?> LoadFromFileAsync(string fiePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(fiePath)) return default;

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
        public async Task SaveToFileAsync(IGatewayConfig config, string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

                await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
                await serializer.SaveAsync(config, stream, cancellationToken);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Failed to write config file: {filePath}", ex);
            }
        }
    }
}