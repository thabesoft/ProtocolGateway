using System.Text.Json;
using System.Text.Json.Serialization;
using ThabeSoft.Ports.Options;

namespace ThabeSoft.ProtocolGateway.JsonConverters;

/// <summary>
/// 波特率转换器
/// </summary>
internal sealed class BaudRateConverter : JsonConverter<BaudRate>
{
    public override BaudRate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException($"Expected number, got {reader.TokenType}");

        var value = reader.GetInt32();

        var result = BaudRate.Create(value);
        if (!result.IsSuccess)
            throw new JsonException($"Invalid baud rate: {result.Message}");

        return result.Value;
    }

    public override void Write(Utf8JsonWriter writer, BaudRate value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue((int)value);
    }
}