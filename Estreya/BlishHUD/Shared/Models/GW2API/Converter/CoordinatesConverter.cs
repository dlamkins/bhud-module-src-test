using System;
using Gw2Sharp.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Models.GW2API.Converter
{
	public class CoordinatesConverter : JsonConverter<Coordinates2>
	{
		public override Coordinates2 ReadJson(JsonReader reader, Type objectType, Coordinates2 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw new JsonException("Expected start of array");
			}
			if (!reader.Read())
			{
				throw new JsonException("Unexpected end of array");
			}
			double? num = serializer.Deserialize<double>(reader);
			if (!reader.Read())
			{
				throw new JsonException("Unexpected end of array");
			}
			double y = serializer.Deserialize<double>(reader);
			if (!reader.Read() || reader.TokenType != JsonToken.EndArray)
			{
				throw new JsonException("Expected end of array");
			}
			return new Coordinates2(num, y);
		}

		public override void WriteJson(JsonWriter writer, Coordinates2 value, JsonSerializer serializer)
		{
			writer.WriteStartArray();
			serializer.Serialize(writer, ((Coordinates2)(ref value)).get_X());
			serializer.Serialize(writer, ((Coordinates2)(ref value)).get_Y());
			writer.WriteEndArray();
		}
	}
}
