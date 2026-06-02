using System.Text.Json;
using System.Text.Json.Serialization;

namespace ThabeSoft.ProtocolGateway.Infrastructure.JsonConverters;


/// <summary>
/// 通道名称转换器
/// </summary>
internal sealed class ChannelNameConverter : JsonConverter<ChannelName>
{
    public override ChannelName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"Expected string, got {reader.TokenType}");

        var str = reader.GetString();

        if (string.IsNullOrEmpty(str))
            throw new JsonException("Channel name cannot be null or empty");

        var result = ChannelName.Create(str);
        if (!result.IsSuccess)
            throw new JsonException($"Invalid channel name: {result.Message}");

        return result.Value;
    }

    public override void Write(Utf8JsonWriter writer, ChannelName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}