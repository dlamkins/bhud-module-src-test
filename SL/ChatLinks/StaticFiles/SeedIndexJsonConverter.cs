using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SL.ChatLinks.StaticFiles
{
	public sealed class SeedIndexJsonConverter : JsonConverter<SeedIndex>
	{
		public override SeedIndex? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using JsonDocument json = JsonDocument.ParseValue(ref reader);
			JsonElement root = json.RootElement;
			if (root.ValueKind != JsonValueKind.Object)
			{
				return null;
			}
			JsonElement databasesElement = default(JsonElement);
			foreach (JsonProperty property in root.EnumerateObject())
			{
				if (property.NameEquals("databases"))
				{
					databasesElement = property.Value;
				}
			}
			if (databasesElement.ValueKind != JsonValueKind.Array)
			{
				return null;
			}
			List<SeedDatabase> databases = new List<SeedDatabase>();
			foreach (JsonElement item in databasesElement.EnumerateArray())
			{
				SeedDatabase database = JsonSerializer.Deserialize<SeedDatabase>(item.GetRawText(), options);
				if ((object)database == null)
				{
					return null;
				}
				databases.Add(database);
			}
			return new SeedIndex
			{
				Databases = databases
			};
		}

		public override void Write(Utf8JsonWriter writer, SeedIndex value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteStartArray("databases");
			foreach (SeedDatabase database in value.Databases)
			{
				JsonSerializer.Serialize(writer, database, options);
			}
			writer.WriteEndArray();
			writer.WriteEndObject();
		}
	}
}
