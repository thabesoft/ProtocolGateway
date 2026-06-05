using System.Text.Json;
using ThabeSoft.ProtocolGateway.Infrastructure.Json;

namespace ThabeSoft.ProtocolGateway.Configuration.Internal.Json;


/// <summary>
/// 配置Json序列化器
/// </summary>
internal sealed class ConfigJsonSerializer(ConfigJsonSerializerContext context)
{
    public ValueTask<GatewayConfig?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.DeserializeAsync(stream, context.GatewayConfig, cancellationToken: cancellationToken);
    }
    public Task SerializeAsync(IGatewayConfig config, Stream stream, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.SerializeAsync(stream, config, context.GatewayConfig, cancellationToken: cancellationToken);
    }


    public GatewayConfig? Deserialize(string json)
    {
        return JsonSerializer.Deserialize(json, context.GatewayConfig);
    }
    public string Serialize(IGatewayConfig config)
    {
        return JsonSerializer.Serialize(config, context.GatewayConfig);
    }
}


internal static class ConfigJsonSerializerExtensions
{
    extension(ConfigJsonSerializer serializer)
    {
        public async ValueTask<GatewayConfig?> DeserializeFromFileAsync(string fiePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(fiePath)) return default;

            try
            {
                await using var fs = new FileStream(fiePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                return await serializer.DeserializeAsync(fs, cancellationToken);
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
        public async Task SerializeToFileAsync(IGatewayConfig config, string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

                await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
                await serializer.SerializeAsync(config, stream, cancellationToken);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Failed to write config file: {filePath}", ex);
            }
        }
    }
}