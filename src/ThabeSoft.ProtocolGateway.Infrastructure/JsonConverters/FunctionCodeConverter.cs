using System.Text.Json;
using System.Text.Json.Serialization;
using ThabeSoft.Modbus;

namespace ThabeSoft.ProtocolGateway.JsonConverters;

/// <summary>
/// 功能码转换
/// </summary>
internal sealed class FunctionCodeConverter : JsonConverter<FunctionCode>
{
    public override FunctionCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException($"Expected number, got {reader.TokenType}");

        var value = reader.GetByte();

        var result = FunctionCode.FromCode(value);
        if (!result.IsSuccess)
            throw new JsonException($"Invalid function code: {result.Message}");

        return result.Value;
    }

    public override void Write(Utf8JsonWriter writer, FunctionCode value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue((byte)value);
    }
}