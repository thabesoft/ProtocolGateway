using System.Text.Json;
using System.Text.Json.Serialization;
using ThabeSoft.ProtocolGateway.Configuration;
using ThabeSoft.ProtocolGateway.Infrastructure.JsonConverters;

namespace ThabeSoft.ProtocolGateway.Services;


/// <summary>
/// 通道配置
/// </summary>
public interface IChannelConfigService
{
    /// <summary>
    /// 从流加载
    /// </summary>
    ValueTask<IReadOnlyList<ChannelConfig>> LoadAsync(Stream stream, CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存到流
    /// </summary>
    Task SaveAsync(IEnumerable<ChannelConfig> source, Stream stream, CancellationToken cancellationToken = default);
}

public static class ChannelConfigServiceExtensions
{
    extension(IChannelConfigService service)
    {
        /// <summary>
        /// 从Json文件加载
        /// </summary>
        /// <param name="fiePath">文件路径</param>
        /// <exception cref="InvalidOperationException">格式错误或者文件异常</exception>
        public async ValueTask<IReadOnlyList<ChannelConfig>> LoadFromFileAsync(string fiePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(fiePath)) return [];

            try
            {
                await using var fs = new FileStream(fiePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                return await service.LoadAsync(fs, cancellationToken);
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

        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="source">数据</param>
        /// <param name="filePath">文件路径</param>
        /// <exception cref="InvalidOperationException">文件异常</exception>
        public async Task SaveToFileAsync(IEnumerable<ChannelConfig> source, string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

                await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
                await service.SaveAsync(source, stream, cancellationToken);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Failed to write config file: {filePath}", ex);
            }
        }
    }
}


/// <summary>
/// 通道Json配置业务
/// </summary>
internal sealed class ChannelJsonConfigService : IChannelConfigService
{
    private readonly JsonSerializerOptions _options;

    public ChannelJsonConfigService()
    {
        _options = new();
        _options.Converters.Add(new JsonStringEnumConverter());
        _options.Converters.Add(new BaudRateConverter());
        _options.Converters.Add(new ChannelNameConverter());
        _options.Converters.Add(new FunctionCodeConverter());
    }

    public async ValueTask<IReadOnlyList<ChannelConfig>> LoadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync<ChannelConfig[]>(stream, _options, cancellationToken: cancellationToken) ?? [];
    }

    public Task SaveAsync(IEnumerable<ChannelConfig> source, Stream stream, CancellationToken cancellationToken = default)
    {
        var values = source.OrderBy(x => x.Name).ToArray();
        return JsonSerializer.SerializeAsync(stream, values, _options, cancellationToken: cancellationToken);
    }
}