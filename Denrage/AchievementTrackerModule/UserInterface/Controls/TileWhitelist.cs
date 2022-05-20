using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class TileWhitelist
	{
		public class Continent
		{
			public List<Floor> Floors { get; set; } = new List<Floor>();

		}

		public class Floor
		{
			public int Id { get; set; }

			public List<string> Coordinates { get; set; } = new List<string>();

		}

		public class ContinentConverter : JsonConverter<Continent>
		{
			public override Continent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != JsonTokenType.StartObject)
				{
					throw new JsonException();
				}
				Continent continent = new Continent();
				while (reader.Read())
				{
					if (reader.TokenType == JsonTokenType.EndObject)
					{
						return continent;
					}
					Floor floor = new Floor();
					continent.Floors.Add(floor);
					if (reader.TokenType != JsonTokenType.PropertyName)
					{
						throw new JsonException();
					}
					floor.Id = int.Parse(reader.GetString());
					reader.Read();
					if (reader.TokenType != JsonTokenType.StartArray)
					{
						throw new JsonException();
					}
					while (true)
					{
						reader.Read();
						if (reader.TokenType == JsonTokenType.EndArray)
						{
							break;
						}
						floor.Coordinates.Add(reader.GetString());
					}
				}
				throw new JsonException();
			}

			public override void Write(Utf8JsonWriter writer, Continent value, JsonSerializerOptions options)
			{
				throw new NotImplementedException();
			}
		}

		[JsonPropertyName("1")]
		public Continent Tyria { get; set; }

		[JsonPropertyName("2")]
		public Continent Mists { get; set; }
	}
}
