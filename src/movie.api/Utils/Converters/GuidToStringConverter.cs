using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace movie_svc.Utils.Converters;

public class GuidToStringConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Guid.TryParse(reader.GetString(), out var guid) ? guid : Guid.Empty;
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}