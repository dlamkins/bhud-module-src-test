using System;
using Gw2Sharp.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models.GW2API.Converter
{
	public class CoordinatesConverter : JsonConverter<Coordinates2>
	{
		public override Coordinates2 ReadJson(JsonReader reader, Type objectType, Coordinates2 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Invalid comparison between Unknown and I4
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			if ((int)reader.get_TokenType() != 2)
			{
				throw new JsonException("Expected start of array");
			}
			if (!reader.Read())
			{
				throw new JsonException("Unexpected end of array");
			}
			double num = serializer.Deserialize<double>(reader);
			if (!reader.Read())
			{
				throw new JsonException("Unexpected end of array");
			}
			double y = serializer.Deserialize<double>(reader);
			if (!reader.Read() || (int)reader.get_TokenType() != 14)
			{
				throw new JsonException("Expected end of array");
			}
			return new Coordinates2(num, y);
		}

		public override void WriteJson(JsonWriter writer, Coordinates2 value, JsonSerializer serializer)
		{
			writer.WriteStartArray();
			serializer.Serialize(writer, (object)((Coordinates2)(ref value)).get_X());
			serializer.Serialize(writer, (object)((Coordinates2)(ref value)).get_Y());
			writer.WriteEndArray();
		}
	}
}
