using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Taimi.UndaDaSea_BlishHUD
{
	public class Vector3Converter : JsonConverter<Vector3>
	{
		public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Invalid comparison between Unknown and I4
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			Vector3 result = default(Vector3);
			while (reader.Read())
			{
				if ((int)reader.get_TokenType() == 4)
				{
					switch (reader.get_Value().ToString())
					{
					case "X":
						result.X = (float)reader.ReadAsDouble().Value;
						break;
					case "Y":
						result.Y = (float)reader.ReadAsDouble().Value;
						break;
					case "Z":
						result.Z = (float)reader.ReadAsDouble().Value;
						break;
					}
				}
				else if ((int)reader.get_TokenType() == 13)
				{
					break;
				}
			}
			return result;
		}

		public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			writer.WriteStartObject();
			if ((int)serializer.get_TypeNameHandling() != 0)
			{
				writer.WritePropertyName("$type");
				writer.WriteValue($"{((object)value).GetType().ToString()}, {((object)value).GetType().Assembly.GetName().Name}");
			}
			writer.WritePropertyName("X");
			writer.WriteValue(value.X);
			writer.WritePropertyName("Y");
			writer.WriteValue(value.Y);
			writer.WritePropertyName("Z");
			writer.WriteValue(value.Z);
			writer.WriteEndObject();
		}
	}
}
