using System.Text.Json;
using ThabeSoft.Primitives;
using ThabeSoft.ProtocolGateway.Infrastructure.Json;

namespace ThabeSoft.ProtocolGateway.Configuration.Json;


/// <summary>
/// 配置Json序列化器
/// </summary>
internal sealed class ConfigJsonSerializer(ConfigJsonSerializerContext context)
{
    public async ValueTask<Result<GatewayConfig>> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        if (!stream.CanRead)
        {
            return Result.Error<GatewayConfig>("无法反序列化Json, 目标流不支持读取");
        }

        try
        {
            var value = await JsonSerializer.DeserializeAsync(stream, context.GatewayConfig, cancellationToken: cancellationToken);
            if (value is not null) return Result.Success(value);

            return Result.Error<GatewayConfig>("无法反序列化Json, 未解析到内容");
        }
        catch (Exception ex)
        {
            return Result.Error<GatewayConfig>($"无法反序列化Json, {ex.Message}");
        }
    }
    public async Task<Result> SerializeAsync(GatewayConfig config, Stream stream, CancellationToken cancellationToken = default)
    {
        if (!stream.CanWrite)
        {
            return Result.Error("无法序列化为Json, 目标流不支持写入");
        }

        try
        {
            await JsonSerializer.SerializeAsync(stream, config, context.GatewayConfig, cancellationToken: cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error<GatewayConfig>($"无法序列化为Json, {ex.Message}");
        }
    }


    public Result<GatewayConfig> Deserialize(string json)
    {
        try
        {
            var value = JsonSerializer.Deserialize(json, context.GatewayConfig);
            if (value is not null) return Result.Success(value);

            return Result.Error<GatewayConfig>("无法反序列化Json, 未解析到内容");
        }
        catch (Exception ex)
        {
            return Result.Error<GatewayConfig>($"无法反序列化Json, {ex.Message}");
        }
    }
    public Result<string> Serialize(GatewayConfig config)
    {
        try
        {
            var value = JsonSerializer.Serialize(config, context.GatewayConfig);
            if (!string.IsNullOrWhiteSpace(value)) return Result.Success(value);

            return Result.Error<string>("无法序列化为Json, 没有内容");
        }
        catch (Exception ex)
        {
            return Result.Error<string>($"无法序列化为Json, {ex.Message}");
        }
    }
}


internal static class ConfigJsonSerializerExtensions
{
    extension(ConfigJsonSerializer serializer)
    {
        public async ValueTask<Result<GatewayConfig>> DeserializeFromFileAsync(string fiePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(fiePath))
            {
                return Result.Error<GatewayConfig>($"无法反序列化Json, 文件不存在: {fiePath}");
            }

            try
            {
                await using var fs = new FileStream(fiePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                return await serializer.DeserializeAsync(fs, cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error<GatewayConfig>($"无法反序列化Json文件, {ex.Message}");
            }
        }

        public async Task<Result> SerializeToFileAsync(GatewayConfig config, string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

                await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
                await serializer.SerializeAsync(config, stream, cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error<GatewayConfig>($"无法序列化为Json文件, {ex.Message}");
            }
        }
    }
}