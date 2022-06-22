using System;
using System.Collections.ObjectModel;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable.Models.GW2API.Converter
{
	public abstract class RectangleConverter : JsonConverter<Rectangle>
	{
		private RectangleDirectionType DirectionType { get; }

		public RectangleConverter(RectangleDirectionType directionType)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DirectionType = directionType;
		}

		public override Rectangle ReadJson(JsonReader reader, Type objectType, Rectangle existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Invalid comparison between Unknown and I4
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			((Collection<JsonConverter>)(object)serializer.get_Converters()).Add((JsonConverter)(object)new CoordinatesConverter());
			if ((int)reader.get_TokenType() != 2)
			{
				throw new JsonException("Expected start of array");
			}
			Coordinates2[] values = (Coordinates2[])(object)new Coordinates2[2];
			for (int i = 0; i < 2; i++)
			{
				if (!reader.Read())
				{
					throw new JsonException("Unexpected end of array");
				}
				values[i] = serializer.Deserialize<Coordinates2>(reader);
			}
			if (!reader.Read() || (int)reader.get_TokenType() != 14)
			{
				throw new JsonException("Expected end of array");
			}
			return new Rectangle(values[0], values[1], DirectionType);
		}

		public override void WriteJson(JsonWriter writer, Rectangle value, JsonSerializer serializer)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartArray();
			serializer.Serialize(writer, (object)((Rectangle)(ref value)).get_BottomLeft());
			serializer.Serialize(writer, (object)((Rectangle)(ref value)).get_TopRight());
			writer.WriteEndArray();
		}
	}
}
