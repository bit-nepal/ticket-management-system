using System.Text.Json;
using System.Text.Json.Serialization;

public class EnumDictionaryConverter<TEnum, TValue> : JsonConverter<Dictionary<TEnum, TValue>> where TEnum : struct, Enum
{
  public override Dictionary<TEnum, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var result = new Dictionary<TEnum, TValue>();
    var dictionary = JsonSerializer.Deserialize<Dictionary<string, TValue>>(ref reader, options);

    foreach (var (key, value) in dictionary)
    {
      if (Enum.TryParse(key, out TEnum enumKey))
      {
        result[enumKey] = value;
      }
      else
      {
        throw new JsonException($"Unable to parse '{key}' to enum {typeof(TEnum).Name}");
      }
    }
    return result;
  }

  public override void Write(Utf8JsonWriter writer, Dictionary<TEnum, TValue> value, JsonSerializerOptions options)
  {
    throw new NotImplementedException();
  }
}
